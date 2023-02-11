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

    // Start is called before the first frame update
    void Start()
    {
    }

    public void spawn(int spawn_type)
    {
        if(spawn_type == 1)
        {
            NoteObject clone = (NoteObject)Instantiate(note, transform.position, Quaternion.identity);
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

    // Update is called once per frame
    void Update()
    {
    }
}
