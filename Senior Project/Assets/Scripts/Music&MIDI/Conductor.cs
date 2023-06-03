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

    // Level 1
    AudioSource background;
    AudioSource background2;
    public int current_background = 1;

    AudioSource drums;
    AudioSource guitar;
    AudioSource piano;

    // Level 2
    AudioSource l2_background1;
    AudioSource l2_background2;
    AudioSource l2_background3;
    AudioSource l2_background1alt;
    AudioSource l2_background2alt;
    AudioSource l2_background3alt;
    AudioSource l2_end;

    AudioSource l2_section1;
    AudioSource l2_section2;

    public float bpm;
    public float beatLength;
    public float spotLength;

    public double lastSongPosition;
    public double nextSpotTime;
    public double offset;
    public int beats_till_first_note;

    public int spotNumber;
    public int beatNumber;
    public int barNumber;

    public float latency_offset;

    private double bufferSchedulingOffset = 3.0;
    private double startTime;

    double backgroundDuration;
    double nextStartTime;
    double nextBackgroundOffset = 0.0;

    public bool playBackground;

    public bool playDrumsNextBar = false;
    public bool playGuitarNextBar = false;
    public bool playPianoNextBar = false;

    public bool playL2BG1 = false;
    public bool playL2Section1 = false;
    public bool playL2BG2 = false;
    public bool playL2Section2 = false;
    public bool playL2BG3 = false;
    public bool playL2End = false;

    void Start()
    {
        nextSpotTime = 0.0;
        spotNumber = 0;
        beatNumber = 0;
        barNumber = 0;
        beatLength = 60 / bpm;
        spotLength = beatLength / SPOTS_PER_BEAT;
        lastSongPosition = 0.0f;

        AudioSource[] audioSources = GetComponents<AudioSource>();
        background = audioSources[0];
        background2 = audioSources[1];
        drums = audioSources[2];
        guitar = audioSources[3];
        piano = audioSources[4];

        l2_background1 = audioSources[5];
        l2_background1alt = audioSources[6];
        l2_background2 = audioSources[7];
        l2_background2alt = audioSources[8];
        l2_background3 = audioSources[9];
        l2_background3alt = audioSources[10];
        l2_end = audioSources[11];
        l2_section1 = audioSources[12];
        l2_section2 = audioSources[13];

        startTime = (double)AudioSettings.dspTime + bufferSchedulingOffset;
        l2_background1.PlayScheduled(startTime);
        //background.PlayScheduled(startTime);

        // Looping variables
        backgroundDuration = beatLength * 16;
        nextStartTime = startTime + backgroundDuration - nextBackgroundOffset;
        l2_background1alt.PlayScheduled(nextStartTime);
        //background2.PlayScheduled(nextStartTime);

        latency_offset = PlayerPrefs.GetFloat("latency_offset");
    }

    // Returns the unequivocal position in the current song.
    public double GetSongPosition()
    {
        return AudioSettings.dspTime - startTime - (beats_till_first_note * beatLength) - offset + latency_offset;
    }

    public void Reset()
    {
        startTime = nextStartTime;

        nextSpotTime = 0.0;
        spotNumber = 0;
        beatNumber = 0;
        barNumber = 0;
        lastSongPosition = 0.0;
    }

    public void UpdateFields()
    {
        double newSongPosition = GetSongPosition();

        nextSpotTime += newSongPosition - lastSongPosition;
        lastSongPosition = newSongPosition;

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
        //Debug.Log(AudioSettings.dspTime);
        UpdateFields();
        if (AudioSettings.dspTime > nextStartTime - 3.0) {
            if(playBackground) {
                if(current_background == 1) {
                    background2.PlayScheduled(nextStartTime);
                    current_background = 2;
                }
                else {
                    background.PlayScheduled(nextStartTime);
                    current_background = 1;
                }
            }

            if(playBackground) {
                if(current_background == 1) {
                    l2_background1alt.PlayScheduled(nextStartTime);
                    current_background = 2;
                }
                else {
                    l2_background1.PlayScheduled(nextStartTime);
                    current_background = 1;
                }
            }
            if(playL2BG2) {
                if(current_background == 1) {
                    l2_background2alt.PlayScheduled(nextStartTime);
                    current_background = 2;
                }
                else {
                    l2_background2.PlayScheduled(nextStartTime);
                    current_background = 1;
                }
            }
            if(playL2BG3) {
                if(current_background == 1) {
                    l2_background3alt.PlayScheduled(nextStartTime);
                    current_background = 2;
                }
                else {
                    l2_background3.PlayScheduled(nextStartTime);
                    current_background = 1;
                }
            }

            // Check for different tracks.
            if(playDrumsNextBar) {
                playDrumsNextBar = false;
                drums.PlayScheduled(nextStartTime);
            }
            if(playGuitarNextBar) {
                playGuitarNextBar = false;
                guitar.PlayScheduled(nextStartTime);
            }
            if(playPianoNextBar) {
                playPianoNextBar = false;
                piano.PlayScheduled(nextStartTime);
            }
            if(playL2Section1) {
                playL2Section1 = false;
                playL2BG1 = false;
                l2_section1.PlayScheduled(nextStartTime);
            }
            if(playL2Section2) {
                playL2Section2 = false;
                playL2BG2 = false;
                l2_section2.PlayScheduled(nextStartTime);
            }
            if(playL2End) {
                playL2End = false;
                playL2BG3 = false;
                l2_end.PlayScheduled(nextStartTime);
            }

            nextStartTime = nextStartTime + backgroundDuration - nextBackgroundOffset;
        }
    }
}