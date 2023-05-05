using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;
    public bool isMoving;
    public float cameraX;

    public float farBackgroundSpeed;

    void Start()
    {
        cameraX = transform.position.x;
        isMoving = true;
        farBackgroundSpeed = 0.03f;
    }

    void Update()
    {
    }
}
