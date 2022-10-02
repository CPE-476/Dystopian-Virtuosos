using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;

struct InputLegend
{
    public bool Space;
}

[RequireComponent(typeof(AudioSource))]
public class Conductor : MonoBehaviour
{
    InputLegend input;
    AudioSource audioSource;

    public float bpm = 100;
    public float crotchet;
    public float songPosition;
    public float deltaSongPosition;
    public float lastHit;           // The last snapped to beat time the spacebar was pressed.
    public float actualLastHit;
    float nextBeatTime = 0.0f;
    public float offset = 0.2f;
    public bool offseted = false;

    public int beatNumber = 1;
    public int barNumber = 1;
    public int beatsPerBar = 4;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        crotchet = 60 / bpm;
        songPosition = 0.0f;
    }

    public void OutputDebugString()
    {
        string debugString = "Song Position: ";

        debugString += songPosition;

        GetComponent<TextMesh>().text = debugString;
    }

    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            input.Space = true;
        }
    }

    public void Update()
    {
        // Change Debug Text Per Frame
        OutputDebugString();

        // Check Inputs
        CheckInput();

        float oldSongPosition = songPosition;
        songPosition = (float)AudioSettings.dspTime * audioSource.pitch - offset;
                                            // NOTE(alex): - dsptimesong) might be needed
                                            // "Every frame that I play the song, I record the dspTime at that moment."
        deltaSongPosition = songPosition - oldSongPosition;

        nextBeatTime += deltaSongPosition;

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
