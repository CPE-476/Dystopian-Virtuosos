using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    public float speed;

    public bool isMoving;

    public GameObject staticBackground;

    public float cameraX;

    public float farBackgroundSpeed;

    // Use this for initialization
    void Start()
    {
        cameraX = transform.position.x;
        isMoving = true;
        farBackgroundSpeed = 0.03f;
        staticBackground.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            isMoving = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            isMoving = true;
        }

        /*        if (transform.position.x >= 6.0f)
        {
            transform.Translate(new Vector3(-28f, 0, 0));
        }*/
        if (isMoving)
        {
            staticBackground
                .transform
                .Translate(new Vector3(-speed *
                    Time.deltaTime *
                    farBackgroundSpeed,
                    0,
                    0));
        }
        // if (Input.GetKey(KeyCode.Space))
        // {
        //     Debug.Log(cameraX);
        // }
        // cameraX = transform.position.x;
    }
}
