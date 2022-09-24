using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Player : MonoBehaviour

{

    public float moveSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");
        transform.Translate(hMove*Time.deltaTime * moveSpeed, vMove*Time.deltaTime * moveSpeed, 0);
    }
}
