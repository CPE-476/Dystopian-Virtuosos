using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Conductor conductor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(Time.deltaTime/conductor.crotchet * 1, 0f, 0f);

        if (transform.position.x < 0 && transform.position.y < 10)
        {
            transform.localScale -= new Vector3(0.5f*Time.deltaTime, 0.5f * Time.deltaTime, 0.5f * Time.deltaTime);
        }
        else
            transform.localScale = new Vector3(0.5f, 0.5f, 0.0f);


    }
}
