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
        if(MIDIReader.changed)
        {
            MIDIReader.changed = false;
            var curVal = MIDIReader.SpotTrack[(MIDIReader.index+12) % MIDIReader.SpotTrack.Length];

            // Note 1
            if (curVal.one.velocity == 64)
            {
                spawner1.spawn(1);
            }
            else if (curVal.one.velocity == 72)
            {
                spawner1.spawn(2);
            }
            else if (curVal.one.velocity == 80)
            {
                spawner1.spawn(3);
            }

            // Note 2
            if (curVal.two.velocity == 64)
            {
                spawner2.spawn(1);
            }
            else if (curVal.two.velocity == 72)
            {
                spawner2.spawn(2);
            }
            else if (curVal.two.velocity == 80)
            {
                spawner2.spawn(3);
            }

            // Note 3
            if (curVal.three.velocity == 64)
            {
                spawner3.spawn(1);
            }
            else if (curVal.three.velocity == 72)
            {
                spawner3.spawn(2);
            }
            else if (curVal.three.velocity == 80)
            {
                spawner3.spawn(3);
            }

            // Note 4
            if (curVal.four.velocity == 64)
            {
                spawner4.spawn(1);
            }
            else if (curVal.four.velocity == 72)
            {
                spawner4.spawn(2);
            }
            else if (curVal.four.velocity == 80)
            {
                spawner4.spawn(3);
            }
        }
    }
}
