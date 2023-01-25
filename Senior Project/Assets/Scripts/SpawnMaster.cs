using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMaster : MonoBehaviour
{
    public Spawner spawner1;
    public Spawner spawner2;
    public Spawner spawner3;
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
        Debug.Log("VALUE" + curVal.two.velocity);
        if (curVal.one.velocity == 64)
        {
            if (spawner1.spawn == 0)
                spawner1.spawn = 1;
        }
        else if (curVal.one.velocity == 80)
        {
            if (spawner1.spawn == 0)
                spawner1.spawn = 2;
        }
        else
            spawner1.spawn = 0;

        if (curVal.two.velocity == 64)
        {
            if (spawner2.spawn == 0)
                spawner2.spawn = 1;
        }
        else if (curVal.two.velocity == 80)
        {
            if (spawner2.spawn == 0)
                spawner2.spawn = 2;
        }
        else
            spawner2.spawn = 0;

        if (curVal.three.velocity == 64)
        {
            if (spawner3.spawn == 0)
                spawner3.spawn = 1;
        }
        else if (curVal.three.velocity == 80)
        {
            if (spawner3.spawn == 0)
                spawner3.spawn = 2;
        }
        else        
            spawner3.spawn = 0;
    }
}
