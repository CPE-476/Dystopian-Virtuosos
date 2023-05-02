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

public enum NoteType
{
    EMPTY,
    NOTE,
    HOLD,
    OBSTACLE,
    COLLECTIBLE
}

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

    public NoteType[] track_state;

    public int index;

    TempoMap tempoMap;

    int BEATS_PER_BAR;

    int SPOTS_PER_BEAT = 4;

    int SixteenthLength = 119;

    // byte[] oneNotes = new byte[] { 56, 58, 61 };
    // byte[] twoNotes = new byte[] { 57, 59, 62 };
    // byte[] threeNotes = new byte[] { 60, 63 };
    // byte[] fourNotes = new byte[] { 64 };
    byte[] oneNotes = new byte[] { 64 };

    byte[] twoNotes = new byte[] { 60, 63 };

    byte[] threeNotes = new byte[] { 57, 59, 62 };

    byte[] fourNotes = new byte[] { 56, 58, 61 };

    public string midiFilePath;

    private MidiFile midiFile;

    List<NoteElement> trackInfo = new List<NoteElement>();

    public struct SpotElement
    {
        public NoteElement four;

        public NoteElement three;

        public NoteElement two;

        public NoteElement one;
    }

    public SpotElement[] SpotTrack;

    public bool changed = false;

    /*
        A Midi file has 32 bits per note. Here is how it's laid out.
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
        public ushort length;

        /*
            64: Hit note
            72: Hold note
            80: obstacles
            88: collectible
            96: health
            104: ...
        */
        public byte velocity;
    }

    void Start()
    {
        midiFile = MidiFile.Read(midiFilePath);
        InitNoteElement();
        InitSpotTrack();

        track_state =
            new NoteType[] {
                NoteType.EMPTY,
                NoteType.EMPTY,
                NoteType.EMPTY,
                NoteType.EMPTY
            };
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
            for (int i = 0; i < temp.Length; ++i)
            {
                newNote.pos[i] = Convert.ToUInt16(temp[i]);
            }

            // note length for press & hold
            newNote.length = ((ushort)(note.Length / SixteenthLength));

            // note types
            newNote.velocity = note.Velocity;

            Debug.Log(newNote.velocity);
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

    public bool hasNoteInFuture(int spots)
    {
        int future_index =
            conductor.barNumber *
            Conductor.BEATS_PER_BAR *
            Conductor.SPOTS_PER_BEAT +
            conductor.beatNumber * Conductor.SPOTS_PER_BEAT +
            conductor.spotNumber +
            spots;

        //Debug.Log(future_index);
        //Debug.Log(index);
        SpotElement curVal = SpotTrack[future_index];

        if (
            curVal.one.velocity == 64 ||
            curVal.two.velocity == 64 ||
            curVal.three.velocity == 64 ||
            curVal.four.velocity == 64
        )
        {
            return true;
        }
        return false;
    }

    public void updateTrackState()
    {
        changed = true;
        index =
            conductor.barNumber *
            Conductor.BEATS_PER_BAR *
            Conductor.SPOTS_PER_BEAT +
            conductor.beatNumber * Conductor.SPOTS_PER_BEAT +
            conductor.spotNumber;
        SpotElement curVal = SpotTrack[index];
        Array.Clear(track_state, 0, 4);

        // track 1
        if (curVal.one.velocity == 64)
            track_state[0] = NoteType.NOTE;
        else if (curVal.one.velocity == 72)
            track_state[0] = NoteType.HOLD;
        else if (curVal.one.velocity == 80)
            track_state[0] = NoteType.OBSTACLE;
        else if (curVal.one.velocity == 88)
            track_state[0] = NoteType.COLLECTIBLE;

        // track 2
        if (curVal.two.velocity == 64)
            track_state[1] = NoteType.NOTE;
        else if (curVal.two.velocity == 72)
            track_state[1] = NoteType.HOLD;
        else if (curVal.two.velocity == 80)
            track_state[1] = NoteType.OBSTACLE;
        else if (curVal.two.velocity == 88)
            track_state[1] = NoteType.COLLECTIBLE;

        // track 3
        if (curVal.three.velocity == 64)
            track_state[2] = NoteType.NOTE;
        else if (curVal.three.velocity == 72)
            track_state[2] = NoteType.HOLD;
        else if (curVal.three.velocity == 80)
            track_state[2] = NoteType.OBSTACLE;
        else if (curVal.three.velocity == 88)
            track_state[2] = NoteType.COLLECTIBLE;

        // track 4
        if (curVal.four.velocity == 64)
            track_state[3] = NoteType.NOTE;
        else if (curVal.four.velocity == 72)
            track_state[3] = NoteType.HOLD;
        else if (curVal.four.velocity == 80)
            track_state[3] = NoteType.OBSTACLE;
        else if (curVal.four.velocity == 88)
            track_state[3] = NoteType.COLLECTIBLE;
    }
}
