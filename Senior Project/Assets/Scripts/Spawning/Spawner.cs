using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Conductor conductor;

    public NoteObject note;

    /*    public NoteObject hold;*/
    public NoteObject obstacle;

    public NoteObject hold;

    public int spawnNum = 0;

    public void spawn(int spawn_type)
    {
        if(spawn_type == 1)
        {
            NoteObject clone = (NoteObject)Instantiate(note, transform.position, Quaternion.identity);
            clone.GetComponent<SpriteRenderer>().enabled = true;
            Destroy(clone.gameObject, 7f);
            spawnNum++;
        }
        if (spawn_type == 2)
        {
            NoteObject clone = (NoteObject)Instantiate(hold, transform.position, Quaternion.identity);
            clone.GetComponent<SpriteRenderer>().enabled = true;
            Destroy(clone.gameObject, 7f);
            spawnNum++;
        }
        else if(spawn_type == 3)
        {
            NoteObject clone = (NoteObject)Instantiate(obstacle, transform.position, Quaternion.identity);
            clone.GetComponent<SpriteRenderer>().enabled = true;
            Destroy(clone.gameObject, 7f);
            spawnNum++;
        }
    }
}
