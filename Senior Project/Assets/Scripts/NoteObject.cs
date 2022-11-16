using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public Spawner spawner;
    public Conductor conductor;
    public NoteTrigger notetrigger;
    float localSpot;
    public float hitSpot;
    public float endSpot;
    public bool dead = false;
    public float Xforce = -5.0f;
    public float Yforce = 10.0f;
    public bool Pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        localSpot = notetrigger.currentSpot;
        hitSpot = localSpot + (conductor.spotLength * 11);
        endSpot = hitSpot + notetrigger.outerThreshold;
        dead = false;
        //localSpot -= conductor.spotLength * 3;
        //GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float interpRatio = ((float)conductor.songPosition-localSpot)/(conductor.spotLength*12);
        float newRatio = 0;
        
        /*
        if (conductor.songPosition >= endSpot && dead == false)
        {
            dead = true;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Xforce, Yforce), ForceMode2D.Impulse);
        }
        */
        if (interpRatio > 1.0f && dead == false)
        {
            //newRatio = ((float)conductor.songPosition - hitSpot) / (conductor.spotLength * 14);
            //Vector3 interpedPostion = Vector3.Lerp(new Vector3(notetrigger.transform.position.x, spawner.transform.position.y, 0f), new Vector3(notetrigger.transform.position.x - (spawner.transform.position.x - notetrigger.transform.position.x), spawner.transform.position.y, 0f), newRatio);
            //transform.position = interpedPostion;
            Debug.Log(newRatio);
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(Xforce ,Yforce), ForceMode2D.Impulse);

            GetComponent<SpriteRenderer>().enabled = false;
        }
        else if(dead == false)
        {
            Vector3 interpedPostion = Vector3.Lerp(spawner.transform.position, new Vector3(notetrigger.transform.position.x, spawner.transform.position.y, 0f), interpRatio);
            transform.position = interpedPostion;
        }

        transform.localScale = new Vector3(0.2f, 0.2f, 0.0f);
    }
}
