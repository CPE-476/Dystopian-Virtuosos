using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1.0f;
    public float maxDistanceFromCenter = 5.0f;
    public Vector3 joystickInput;
    float verticalInput;
    public int joystickNum = 1;

    public GameObject trackAbove;
    public GameObject trackBelow;

    private Vector3 initialPosition;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        initialPosition = (trackAbove.transform.position + trackBelow.transform.position) / 2;
        maxDistanceFromCenter = Vector3.Distance(initialPosition, trackAbove.transform.position);
        if (joystickNum == 1)
            verticalInput = Input.GetAxis("Vertical");
        else
            verticalInput = Input.GetAxis("VerticalRight");

        joystickInput = new Vector3(0, verticalInput, 0);

        Vector3 newPosition = initialPosition + joystickInput * maxDistanceFromCenter;

        transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
    }
}
