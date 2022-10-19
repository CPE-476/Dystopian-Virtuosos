using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Conductor conductor;
    public NoteTrigger notetrigger;
    float localSpot;

    // Start is called before the first frame update
    void Start()
    {
        localSpot = notetrigger.currentSpot;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float interpRatio = ((float)conductor.songPosition-localSpot)/conductor.spotLength;

        Debug.Log("INTERP RATIO: " + interpRatio);
        Debug.Log((float)conductor.songPosition + " notetrigger.currentSpot: " + localSpot);

        Vector3 interpedPostion = Vector3.Lerp(new Vector3(5f, 0f, 0f), new Vector3(0f, 0f, 0f), interpRatio);
        transform.position = interpedPostion;

        if (interpRatio > 1.0f)
            GetComponent<SpriteRenderer>().enabled = false;

        if (transform.position.x < 0 && transform.position.y < 10)
        {
            transform.localScale -= new Vector3(0.5f*Time.deltaTime, 0.5f * Time.deltaTime, 0.5f * Time.deltaTime);
        }
        else
            transform.localScale = new Vector3(0.5f, 0.5f, 0.0f);
    }
}
