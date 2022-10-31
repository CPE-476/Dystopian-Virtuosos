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
    TempoMap tempoMap;
    int BEATS_PER_BAR;
    int SPOTS_PER_BEAT = 4;
    List<NoteElement> trackInfo = new List<NoteElement>();

    struct NoteElement
    {
        public byte number;
        public String pos;
        public long Length;
        public byte Velocity;
    }

    void Start()
    {
        /*        InitializeOutputDevice();*/
        var midiFile = MidiFile.Read("Assets/Music/test_beatmap.mid");
        var notes = midiFile.GetNotes();
        tempoMap = midiFile.GetTempoMap();
        var firstTrackChunk = midiFile.GetTrackChunks().First();
        var firstTimeSignatureEvent = firstTrackChunk
            .Events
            .OfType<TimeSignatureEvent>()
            .FirstOrDefault();

        BEATS_PER_BAR = firstTimeSignatureEvent.Numerator;

        var barBeatTimeOfLastEvent = midiFile.GetTimedEvents().Last().TimeAs<BarBeatFractionTimeSpan>(tempoMap);
        var totalBars = barBeatTimeOfLastEvent.Bars;
        if (barBeatTimeOfLastEvent.Beats > 0)
            totalBars = barBeatTimeOfLastEvent.Bars + 1;
        foreach (var note in notes)
        {
            NoteElement newNote = new NoteElement();
            // track number and instrument type
            newNote.number = note.NoteNumber;
            // note position
            newNote.pos = note.TimeAs(TimeSpanType.BarBeatTicks, tempoMap).ToString();
            // note length for press & hold
            newNote.Length = note.Length;
            // note types
            newNote.Velocity = note.Velocity;
            trackInfo.Append(newNote);
        }
    }
}