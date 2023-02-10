using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Conductor conductor;
    public NoteObject note;
/*    public NoteObject hold;*/
    public NoteObject obstacle;
    public int spawnNum = 0;
    float lastBeat;
    public byte spawn = 0;

    // Start is called before the first frame update
    void Start()
    {
        lastBeat = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (conductor.songPosition > lastBeat + conductor.crotchet)
        {
            if(spawn == 1)
            {
                NoteObject clone = (NoteObject)Instantiate(note, transform.position, Quaternion.identity);
                clone.GetComponent<SpriteRenderer>().enabled = true;
                Destroy(clone.gameObject, 7f);
                lastBeat += conductor.spotLength;
                spawnNum++;
            }
            if(spawn == 3)
            {
                NoteObject clone = (NoteObject)Instantiate(obstacle, transform.position, Quaternion.identity);
                clone.GetComponent<SpriteRenderer>().enabled = true;
                Destroy(clone.gameObject, 7f);
                lastBeat += conductor.spotLength;
                spawnNum++;
            }
            spawn = 0;
        }
        else if(conductor.songPosition > lastBeat + conductor.crotchet)
        {
            lastBeat += conductor.spotLength;
        }
    }
}
