using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Conductor conductor;
    private Transform parentTransform;

    public NoteObject note;
    public NoteObject obstacle;
    public NoteObject collectible;
    public NoteObject hold;

    public int spawnNum = 0;

    private void Start()
    {
        parentTransform = GetComponent<Transform>();
        conductor = (Conductor)GameObject.Find("/Conductor").GetComponent("Conductor");
    }

    private void SetupNoteObject(NoteObject obj)
    {
        NoteObject clone = (NoteObject)Instantiate(obj, transform);
        clone.transform.position = transform.position;
        clone.transform.rotation = Quaternion.identity;
        clone.GetComponent<SpriteRenderer>().enabled = true;
        Destroy(clone.gameObject, 7f);
        spawnNum++;
    }

    public void Spawn(int spawn_type)
    {
        if(spawn_type == 1)
            SetupNoteObject(note);
        if (spawn_type == 2)
            SetupNoteObject(hold);
        else if(spawn_type == 3)
            SetupNoteObject(obstacle);
        else if(spawn_type == 4)
            SetupNoteObject(collectible);
    }
}
