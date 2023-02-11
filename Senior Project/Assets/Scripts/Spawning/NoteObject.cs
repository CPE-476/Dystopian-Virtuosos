using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Spawner spawner;
    public Conductor conductor;
    public NoteTrigger notetrigger;
    float localSpot;

    // Start is called before the first frame update
    void Start()
    {
        localSpot = notetrigger.currentSpot;
        //localSpot -= conductor.spotLength * 3;
        //GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float interpRatio = ((float)conductor.songPosition - localSpot) / (conductor.spotLength * 12);

        Vector3 interpedPostion = Vector3.Lerp(spawner.transform.position, new Vector3(notetrigger.transform.position.x, spawner.transform.position.y, 0f), interpRatio);
        transform.position = interpedPostion;

        if (interpRatio > 1.0f)
            GetComponent<SpriteRenderer>().enabled = false;

        transform.localScale = new Vector3(0.5f, 0.5f, 0.0f);
    }
}
