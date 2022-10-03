using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public Conductor conductor;
    public float offset = 0.07f;
    public AudioClip song;

    // Start is called before the first frame update
    void Start()
    {
        conductor.offset = offset;
        conductor.GetComponent<AudioSource>().clip = song;
    }
}
