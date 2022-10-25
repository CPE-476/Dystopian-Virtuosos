using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMaster : MonoBehaviour
{
    public Conductor conductor;

    public const int NUM_TRACKS = 4;
    public uint[] track;
    public bool[] pressable;

    public int index;

    // Start is called before the first frame update
    void Start()
    {
        track = new uint[] {1, 0, 1, 0,
                            2, 0, 2, 0,
                            10, 0, 5, 0,
                            8, 0, 8, 0};
        pressable = new bool[] {false, false, false, false};
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void changePressable()
    {
        index = conductor.barNumber * Conductor.BEATS_PER_BAR * Conductor.SPOTS_PER_BEAT
              + conductor.beatNumber * Conductor.SPOTS_PER_BEAT
              + conductor.spotNumber;
        uint curVal = track[index % track.Length];
        Array.Clear(pressable, 0, pressable.Length);
        if ((curVal & 1) > 0)
        {
            pressable[0] = true;
        }
        if ((curVal & 2) > 0)
        {
            pressable[1] = true;
        }
        if ((curVal & 4) > 0)
        {
            pressable[2] = true;
        }
        if ((curVal & 8) > 0)
        {
            pressable[3] = true;
        }
    }
}
