using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Conductor conductor;
    public NoteObject note;
    public int spawnNum = 0;
    float lastBeat;
    bool spawn = false;

    // Start is called before the first frame update
    void Start()
    {
        lastBeat = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (conductor.songPosition > lastBeat + conductor.crotchet && spawn)
        {
            NoteObject clone = (NoteObject)Instantiate(note, new Vector3(10f, 0f, 0f), Quaternion.identity);
            Destroy(clone.gameObject, 7f);
            lastBeat += conductor.crotchet;
            spawn = false;
            spawnNum++;
        }
        else if(conductor.songPosition > lastBeat + conductor.crotchet)
        {
            spawn = true;
            lastBeat += conductor.crotchet;
        }
    }
}
