using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMaster : MonoBehaviour
{
    public Spawner spawner1;
    public Spawner spawner2;
    public Spawner spawner3;
    public Spawner spawner4;
    public MIDIReader MIDIReader;
    public Conductor conductor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var curVal = MIDIReader.SpotTrack[(MIDIReader.index+12) % MIDIReader.SpotTrack.Length];
        if (curVal.one != null)
        {
            if (spawner1.spawn == false)
                spawner1.spawn = true;
        }
        else
            spawner1.spawn = false;
        if (curVal.two != null)
        {
            if (spawner2.spawn == false)
                spawner2.spawn = true;
        }
        else
            spawner2.spawn = false;
        if (curVal.three != null)
        {
            if (spawner3.spawn == false)
                spawner3.spawn = true;
        }
        else
            spawner3.spawn = false;
        if (curVal.four != null)
        {
            if (spawner4.spawn == false)
                spawner4.spawn = true;
        }
        else
            spawner4.spawn = false;
    }
}
