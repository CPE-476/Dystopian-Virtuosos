using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;
using UnityEngine;

public class MIDIReader : MonoBehaviour
{
    /*

     ----------------x----x-----x-----x-----                                > piano (64)
     ------x-x---x-----x--x--------x--x-----                > guitar (60)   > piano (63)
     --x-x-----x-x-----------x--x--x--x-----   > drum (57)  > guitar (59)   > piano (62)
     x---x---x-x-x-x-x-x--x--x--x--x--x-----   > drum (56)  > guitar (58)   > piano (61)
     1 2 3 4 5 6 7 8 9 10 11 12 13 14 15

    */

    TempoMap tempoMap;
    int BEATS_PER_BAR;
    int SPOTS_PER_BEAT = 4;
    int SixteenthLength = 119;

    MidiFile midiFile = MidiFile.Read("Assets/Music/test_beatmap.mid");

    List<NoteElement> trackInfo = new List<NoteElement>();

    uint[] BinaryTrack;

    /*
        xxxx  xxxx  xxxx  xxxx  xxxx  xxxx  xxxx  xxxx 
        < number >  <velocity>  <       length       >
    */

    struct NoteElement
    {
        /* 
         DRUM: 56 (G), 57 (A)
         GUITAR: 58 (A#), 59 (B), 60 (C)
         PIANO: 61 (C#), 62(D), 63 (D#), 64 (E)
        */
        public byte number;
        
        public ushort[] pos;

        // in units of 16th note length of 119, for a quarter note, length = 4
        public ushort Length;

        /*
            64: Hit note
            72: Hold note
            80: obstacles
            88: bonus
            96: health
            104: ...

        */
        public byte Velocity;
    }

    void Start()
    {
        InitNoteElement();
        InitBinaryTrack();
    }

    void InitNoteElement()
    {
        
        var notes = midiFile.GetNotes();
        tempoMap = midiFile.GetTempoMap();
        
        foreach (var note in notes)
        {
            NoteElement newNote = new NoteElement();
            // track number and instrument type
            newNote.number = note.NoteNumber;
            Debug.Log(newNote.number);
            // note position
            var pos = note.TimeAs(TimeSpanType.BarBeatTicks, tempoMap).ToString();
            var temp = pos.Split(".");
            newNote.pos = new ushort[3];
            int i = 0;
            foreach (var num in temp)
            {
                newNote.pos[i] = Convert.ToUInt16(num);
                i++;
            }
            Debug.Log("Bar: " + newNote.pos[0]);
            Debug.Log("Beat: " + newNote.pos[1]);
            Debug.Log("Ticks: " + newNote.pos[2]);
            // note length for press & hold
            newNote.Length = ((ushort)(note.Length / SixteenthLength));
            Debug.Log("Length: " + newNote.Length);
            // note types
            newNote.Velocity = note.Velocity;
            trackInfo.Append(newNote);
        }
    }

    void InitBinaryTrack() {

        var firstTrackChunk = midiFile.GetTrackChunks().First();
        var firstTimeSignatureEvent = firstTrackChunk
            .Events
            .OfType<TimeSignatureEvent>()
            .FirstOrDefault();

        BEATS_PER_BAR = firstTimeSignatureEvent.Numerator;

        var barBeatTimeOfLastEvent = midiFile.GetTimedEvents().Last().TimeAs<BarBeatTicksTimeSpan>(tempoMap);
        var totalBars = barBeatTimeOfLastEvent.Bars;
        if (barBeatTimeOfLastEvent.Beats > 0 || barBeatTimeOfLastEvent.Ticks > 0)
            totalBars = barBeatTimeOfLastEvent.Bars + 1;

        BinaryTrack = new uint[totalBars * BEATS_PER_BAR * SPOTS_PER_BEAT];
    }
}