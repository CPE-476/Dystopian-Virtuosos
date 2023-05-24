using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public string joystickType;

    public KeyCode north;

    public KeyCode south;

    public KeyCode east;

    public KeyCode west;

    private Vector3 initialPosition;

    public InputAction leftJoystick;
    public InputAction rightJoystick;

    private void OnEnable()
    {
        leftJoystick.Enable();
        rightJoystick.Enable();
    }

    private void OnDisable()
    {
        leftJoystick.Disable();
        rightJoystick.Disable();
    }

    void Awake()
    {
        string[] joystickNames = Input.GetJoystickNames();

        if (joystickNames.Length != 0)
        {
            if (!string.IsNullOrEmpty(joystickNames[0]))
            {
                string firstJoystickName = joystickNames[0];
                if (firstJoystickName.Contains("Xbox"))
                {
                    Debug.Log("Xbox controller detected.");

                    // Handle Xbox controller input
                    joystickType = "VerticalRightXbox";
                    north = KeyCode.Joystick1Button3;
                    south = KeyCode.Joystick1Button0;
                    east = KeyCode.Joystick1Button1;
                    west = KeyCode.Joystick1Button2;
                }
                else
                {
                    Debug.Log("PS controller detected.");
                    Debug.Log (firstJoystickName);

                    // Handle other controller types or generic input
                    joystickType = "VerticalRightPS";
                    north = KeyCode.Joystick1Button3;
                    south = KeyCode.Joystick1Button1;
                    east = KeyCode.Joystick1Button2;
                    west = KeyCode.Joystick1Button0;
                }
            }
        }
        else
        {
            Debug.Log("No controller detected.");

            // Handle keyboard and mouse input or no input devices
            joystickType = "VerticalRightPS";
            north = KeyCode.Joystick1Button3;
            south = KeyCode.Joystick1Button1;
            east = KeyCode.Joystick1Button2;
            west = KeyCode.Joystick1Button0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        initialPosition =
            (trackAbove.transform.position + trackBelow.transform.position) / 2;
        maxDistanceFromCenter =
            Vector3.Distance(initialPosition, trackAbove.transform.position);
        if (joystickNum == 1)
            verticalInput = leftJoystick.ReadValue<float>();
        else
            verticalInput = rightJoystick.ReadValue<float>();

        joystickInput = new Vector3(0, verticalInput, 0);

        Vector3 newPosition =
            initialPosition + joystickInput * maxDistanceFromCenter;

        transform.position =
            Vector3
                .Lerp(transform.position, newPosition, speed * Time.deltaTime);
    }
}
