using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    private Transform parentTransform;
    private Conductor conductor;
    private NoteTrigger notetrigger;
    float localSpot;

    // Start is called before the first frame update
    void Start()
    {
        conductor = (Conductor)GameObject.Find("/Conductor").GetComponent("Conductor");
        notetrigger = (NoteTrigger)GameObject.Find("/Tracks/NoteTrigger").GetComponent("NoteTrigger");
        parentTransform = transform.parent;
        localSpot = notetrigger.currentSpot;
    }

    // Update is called once per frame
    void Update()
    {
        float interpRatio = ((float)conductor.songPosition - localSpot) / (conductor.spotLength * 12);

        Vector3 interpedPostion = Vector3.Lerp(parentTransform.position, new Vector3(notetrigger.transform.position.x, parentTransform.position.y, 0f), interpRatio);
        transform.position = interpedPostion;

        if(interpRatio > 1.0f)
            GetComponent<SpriteRenderer>().enabled = false;

        transform.localScale = new Vector3(1f, 1f, 0.0f);
    }
}
