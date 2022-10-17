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
    public const int BEATS_PER_BAR = 4;

    AudioSource audioSource;

    public float bpm = 100;
    public float crotchet;
    public float spotLength;

    public double songPosition;
    double nextSpotTime = 0.0f;
    public float offset = 0.3f;
    public bool offseted = false;

    public int spotNumber = 1;
    public int beatNumber = 1;
    public int barNumber = 1;

    private double tempSongPosition;

    // Start is called before the first frame update
    void Start()
    { 
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        tempSongPosition = (double)AudioSettings.dspTime * audioSource.pitch;
        crotchet = 60 / bpm;
        spotLength = crotchet / SPOTS_PER_BEAT;
        songPosition = 0.0f;
    }

    public void Update()
    {
        double newSongPosition = AudioSettings.dspTime * audioSource.pitch - offset - tempSongPosition;
                                                    // NOTE(alex): - dsptimesong) might be needed
                                                    // "Every frame that I play the song, I record the dspTime at that moment."

        nextSpotTime += newSongPosition - songPosition;
        songPosition = newSongPosition;

        if(nextSpotTime > spotLength)
        {
            ++spotNumber;
            nextSpotTime = nextSpotTime - spotLength;
            if(spotNumber > SPOTS_PER_BEAT)
            {
                spotNumber = 1;
                ++beatNumber;
                if(beatNumber > BEATS_PER_BAR)
                {
                    beatNumber = 1;
                    ++barNumber;
                }
            }
        }
    }
}
