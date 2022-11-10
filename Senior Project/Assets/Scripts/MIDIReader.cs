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
    public Conductor conductor;

    public bool[] pressable;

    public int index;

    TempoMap tempoMap;

    int BEATS_PER_BAR;

    int SPOTS_PER_BEAT = 4;

    int SixteenthLength = 119;

    byte[] oneNotes = new byte[] { 56, 58, 61 };

    byte[] twoNotes = new byte[] { 57, 59, 62 };

    byte[] threeNotes = new byte[] { 60, 63 };

    byte[] fourNotes = new byte[] { 64 };

    MidiFile midiFile = MidiFile.Read("Assets/Music/demo_beatmap.mid");

    List<NoteElement> trackInfo = new List<NoteElement>();

    public SpotElement[] SpotTrack;

    /*
        xxxx  xxxx  xxxx  xxxx  xxxx  xxxx  xxxx  xxxx 
        < number >  <velocity>  <       length       >
    */
    public struct NoteElement
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

    public struct SpotElement
    {
        public NoteElement? four;

        public NoteElement? three;

        public NoteElement? two;

        public NoteElement? one;
    }

    void Start()
    {
        InitNoteElement();
        InitSpotTrack();

        pressable = new bool[] { false, false, false, false };
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

            // note position
            var pos =
                note.TimeAs(TimeSpanType.BarBeatTicks, tempoMap).ToString();
            var temp = pos.Split(".");
            newNote.pos = new ushort[3];
            int i = 0;
            foreach (var num in temp)
            {
                newNote.pos[i] = Convert.ToUInt16(num);
                i++;
            }

            // note length for press & hold
            newNote.Length = ((ushort)(note.Length / SixteenthLength));

            // note types
            newNote.Velocity = note.Velocity;

            trackInfo.Add (newNote);
        }
    }

    void InitSpotTrack()
    {
        var firstTrackChunk = midiFile.GetTrackChunks().First();
        var firstTimeSignatureEvent =
            firstTrackChunk
                .Events
                .OfType<TimeSignatureEvent>()
                .FirstOrDefault();

        BEATS_PER_BAR = firstTimeSignatureEvent.Numerator;

        var barBeatTimeOfLastEvent =
            midiFile
                .GetTimedEvents()
                .Last()
                .TimeAs<BarBeatTicksTimeSpan>(tempoMap);
        var totalBars = barBeatTimeOfLastEvent.Bars;
        if (barBeatTimeOfLastEvent.Beats > 0 || barBeatTimeOfLastEvent.Ticks > 0
        ) totalBars = barBeatTimeOfLastEvent.Bars + 1;

        SpotTrack = new SpotElement[totalBars * BEATS_PER_BAR * SPOTS_PER_BEAT];

        foreach (NoteElement note in trackInfo)
        {
            // ticks: 0 -> 1 120->2, 240->3, 360->4
            int index =
                (note.pos[0] - 1) * BEATS_PER_BAR * SPOTS_PER_BEAT +
                (note.pos[1] - 1) * SPOTS_PER_BEAT +
                (note.pos[2] / SixteenthLength) +
                1;

            if (oneNotes.Contains(note.number))
            {
                SpotTrack[index].one = note;
            }
            else if (twoNotes.Contains(note.number))
            {
                SpotTrack[index].two = note;
            }
            else if (threeNotes.Contains(note.number))
            {
                SpotTrack[index].three = note;
            }
            else if (fourNotes.Contains(note.number))
            {
                SpotTrack[index].four = note;
            }
        }
    }

    public void changePressable()
    {
        index =
            conductor.barNumber *
            Conductor.BEATS_PER_BAR *
            Conductor.SPOTS_PER_BEAT +
            conductor.beatNumber * Conductor.SPOTS_PER_BEAT +
            conductor.spotNumber;
        SpotElement curVal = SpotTrack[index];
        Array.Clear(pressable, 0, pressable.Length);
        if (curVal.one != null)
        {
            pressable[0] = true;
        }
        if (curVal.two != null)
        {
            pressable[1] = true;
        }
        if (curVal.three != null)
        {
            pressable[2] = true;
        }
        if (curVal.four != null)
        {
            pressable[3] = true;
        }
    }
}
