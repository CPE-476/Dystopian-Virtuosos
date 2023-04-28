using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Conductor conductor;
    private MIDIReader midiReader;
    private NoteTrigger noteTrigger;
    private Transform parentTransform;

    public NoteObject note;
    public NoteObject obstacle;
    public NoteObject collectible;
    public NoteObject hold;
    public NoteObject holdsquare;
    public NoteObject threshold;

    public int spawnNum = 0;
    public int holdNum;
    public bool incremeted;

    public int index;
    public int newIndex;

    private void Start()
    {
        incremeted = false;
        parentTransform = GetComponent<Transform>();
        conductor = (Conductor)GameObject.Find("/Conductor").GetComponent("Conductor");
        midiReader = (MIDIReader)GameObject.Find("/MIDIReader").GetComponent("MIDIReader");
        noteTrigger = (NoteTrigger)GameObject.Find("/Tracks/NoteTrigger").GetComponent("NoteTrigger");
        index = midiReader.index;
        newIndex = index;
    }
    private void Update()
    {
        newIndex = midiReader.index;

        if (holdNum >= 1 && newIndex > index)
        {
            NoteObject clone = (NoteObject)Instantiate(holdsquare, transform);
            clone.transform.position = transform.position;
            clone.transform.rotation = Quaternion.identity;
            holdNum--;
            index = newIndex;
        }
    }

    IEnumerator VisualizeThresholds(double targetSongPosition)
    {
        // Wait for the next frame to ensure that the note object has been spawned
        yield return null;

        // Loop until the current song position reaches the target position
        while (conductor.songPosition < targetSongPosition)
        {
            yield return null;
        }
        Debug.Log("spawning threshold");
        NoteObject clone = (NoteObject)Instantiate(threshold, transform);
        clone.transform.position = transform.position;
        clone.transform.rotation = Quaternion.identity;
    }

    private void SetupNoteObject(NoteObject obj)
    {
        NoteObject clone = (NoteObject)Instantiate(obj, transform);
        clone.transform.position = transform.position;
        clone.transform.rotation = Quaternion.identity;
        clone.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(VisualizeThresholds(conductor.songPosition + noteTrigger.outerThreshold));
        spawnNum++;
    }

        public void Spawn(int spawn_type, int spawn_length)
    {
        if(spawn_type == 1)
            SetupNoteObject(note);
        else if (spawn_type == 2)
        {
            SetupNoteObject(hold);
            holdNum = spawn_length;
        }
        else if (spawn_type == 3)
            SetupNoteObject(obstacle);
        else if (spawn_type == 4)
            SetupNoteObject(collectible);
        else if (spawn_type == 5)
        {
            SetupNoteObject(threshold);
        }
    }
}
