using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour {
    public float speed;
    public float clampLeft;
    public float clampRight;
    public bool isMoving;

    public float cameraX;


    // Use this for initialization
    void Start () {
        cameraX = transform.position.x;
		isMoving = true;
	}
	
	// Update is called once per frame
	void Update () {
        // if (Input.GetKey(KeyCode.D) && transform.position.x < clampRight)
        // {
        //     transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        // }
        // if (Input.GetKey(KeyCode.A) && transform.position.x > clampLeft)
        // {
        //     transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        // }
        if (Input.GetKey(KeyCode.S))
        {
            isMoving = false;
        }
        if (Input.GetKey(KeyCode.D)){
            isMoving = true;
        }
        if (transform.position.x >= 6.0f)
        {
            transform.Translate(new Vector3(-28f, 0, 0));
        }
        if (isMoving)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log(cameraX);
        }
    }
}
