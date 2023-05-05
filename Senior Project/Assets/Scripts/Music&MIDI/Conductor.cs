using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Conductor : MonoBehaviour
{
    // Constants
    public const int SPOTS_PER_BEAT = 4; // Sixteenth Notes
    public const int BEATS_PER_BAR = 4;  // 4/4 Time

    AudioSource background;
    AudioSource background2;
    int current_background = 1;
    AudioSource drums;

    public float bpm;
    public float beatLength;
    public float spotLength;

    public double songPosition;
    public double nextSpotTime;
    public double offset;
    public int beats_till_first_note;

    public int spotNumber;
    public int beatNumber;
    public int barNumber;

    public double latency_offset;

    private double bufferSchedulingOffset = 3.0;
    private double startTime;

    double backgroundDuration;
    double nextStartTime;
    double nextBackgroundOffset = 0.1;

    public bool playDrumsNextBar = false;
    public bool playGuitarNextBar = false;
    public bool playPianoNextBar = false;

    void Start()
    { 
        nextSpotTime = 0.0;
        spotNumber = 0;
        beatNumber = 0;
        barNumber = 0;
        beatLength = 60 / bpm;
        spotLength = beatLength / SPOTS_PER_BEAT;
        songPosition = 0.0f;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        background = audioSources[0];
        background2 = audioSources[1];
        drums = audioSources[2];

        startTime = (double)AudioSettings.dspTime + bufferSchedulingOffset;
        background.PlayScheduled(startTime);
        //drums.PlayScheduled(startTime);

        // Looping fix
        backgroundDuration = (double)background.clip.samples / background.clip.frequency;
        nextStartTime = startTime + backgroundDuration - nextBackgroundOffset;
        background2.PlayScheduled(nextStartTime);
    }

    // Returns the unequivocal position in the current song.
    public double GetSongPosition()
    {
        return AudioSettings.dspTime - startTime - (beats_till_first_note * beatLength) - offset - latency_offset;
    }

    public void UpdateFields()
    {
        double newSongPosition = GetSongPosition();

        nextSpotTime += newSongPosition - songPosition;
        songPosition = newSongPosition;

        // Updates Bar, Beat, Spot based on Song Position.
        if(nextSpotTime > spotLength)
        {
            ++spotNumber;
            nextSpotTime = nextSpotTime - spotLength;
            if(spotNumber == SPOTS_PER_BEAT)
            {
                spotNumber = 0;
                ++beatNumber;
                if(beatNumber == BEATS_PER_BAR)
                {
                    beatNumber = 0;
                    ++barNumber;
                }
            }
        }
    }

    public void Update()
    {
        UpdateFields();

        if(AudioSettings.dspTime > nextStartTime - 3.0) {
            if(current_background == 1) {
                background2.PlayScheduled(nextStartTime);
                current_background = 2;
            }
            else {
                background.PlayScheduled(nextStartTime);
                current_background = 1;
            }
            nextStartTime = nextStartTime + backgroundDuration - nextBackgroundOffset;
        }
    }
}