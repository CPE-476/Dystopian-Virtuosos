using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Conductor conductor;
    public TrackMaster trackmaster;
    public NoteObject note;
    public int spawnNum = 0;
    float lastBeat;

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
            NoteObject clone = (NoteObject)Instantiate(note, new Vector3(5f, 0f, 0f), Quaternion.identity);
            Destroy(clone.gameObject, 7f);
            lastBeat += conductor.spotLength;
            spawnNum++;
        }
        else if(conductor.songPosition > lastBeat + conductor.crotchet)
        {
            lastBeat += conductor.spotLength;
        }
    }
}
