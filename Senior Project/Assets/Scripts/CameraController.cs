using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
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
    }
}
