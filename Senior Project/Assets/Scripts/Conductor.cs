using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Conductor : MonoBehaviour
{
    AudioSource audioSource;

    public float bpm = 95;
    public float crotchet;
    public float songPosition;
    float nextBeatTime = 0.0f;
    public float offset = 0.2f;
    public bool offseted = false;

    public int beatNumber = 1;
    public int barNumber = 1;
    public int beatsPerBar = 4;

    private float tempSongPosition;

    // Start is called before the first frame update
    void Start()
    { 
        audioSource = GetComponent<AudioSource>();
        tempSongPosition = (float)AudioSettings.dspTime * audioSource.pitch;
        crotchet = 60 / bpm;
        songPosition = 0.0f;
        audioSource.Play();
    }

    public void OutputDebugString()
    {
        string debugString = "Song Position: ";

        debugString += songPosition;
        
        GetComponent<TextMesh>().text = debugString;
    }

    public void Update()
    {
        // Change Debug Text Per Frame
        OutputDebugString();
        songPosition = (float)AudioSettings.dspTime * audioSource.pitch - offset - tempSongPosition;
                                            // NOTE(alex): - dsptimesong) might be needed
                                            // "Every frame that I play the song, I record the dspTime at that moment."

        if(nextBeatTime > crotchet)
        {
            nextBeatTime = 0.0f;
            ++beatNumber;
            if(beatNumber > beatsPerBar)
            {
                beatNumber = 1;
                ++barNumber;
            }
        }
    }
}
