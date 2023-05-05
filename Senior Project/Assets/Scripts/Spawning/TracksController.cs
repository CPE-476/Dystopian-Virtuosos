using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksController : MonoBehaviour
{
    public GameObject Track1;

    public GameObject Track2;

    public GameObject Track3;

    public GameObject Track4;

    public GameObject Joystick1;
    public GameObject Joystick2;

    [SerializeField]
    public float[] DrumPositions;

    [SerializeField]
    public float[] GuitarPositions;

    [SerializeField]
    public float[] PianoPositions;

    // Start is called before the first frame update
    void Start()
    {
        DrumerTracks();
        Track3.transform.position =
            new Vector3(Track3.transform.position.x,
                DrumPositions[0],
                Track3.transform.position.z);
        Track4.transform.position =
            new Vector3(Track4.transform.position.x,
                DrumPositions[1],
                Track4.transform.position.z);
    }
    
    public void drumSwitch()
    {
        DrumerTracks();
        Track3.transform.position =
            new Vector3(Track3.transform.position.x,
                DrumPositions[0],
                Track3.transform.position.z);
        Track4.transform.position =
            new Vector3(Track4.transform.position.x,
                DrumPositions[1],
                Track4.transform.position.z);
    }

    public void guitarSwitch()
    {
        GuitaristTracks();
        Track2.transform.position =
            new Vector3(Track2.transform.position.x,
                GuitarPositions[0],
                Track2.transform.position.z);
        Track3.transform.position =
            new Vector3(Track3.transform.position.x,
                GuitarPositions[1],
                Track3.transform.position.z);
        Track4.transform.position =
            new Vector3(Track4.transform.position.x,
                GuitarPositions[2],
                Track4.transform.position.z);
    }

    public void pianoSwitch()
    {
        PianistTracks();
        Track1.transform.position =
            new Vector3(Track1.transform.position.x,
                PianoPositions[0],
                Track1.transform.position.z);
        Track2.transform.position =
            new Vector3(Track2.transform.position.x,
                PianoPositions[1],
                Track2.transform.position.z);
        Track3.transform.position =
            new Vector3(Track3.transform.position.x,
                PianoPositions[2],
                Track3.transform.position.z);
        Track4.transform.position =
            new Vector3(Track4.transform.position.x,
                PianoPositions[3],
                Track4.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DrumerTracks();
            Track3.transform.position =
                new Vector3(Track3.transform.position.x,
                    DrumPositions[0],
                    Track3.transform.position.z);
            Track4.transform.position =
                new Vector3(Track4.transform.position.x,
                    DrumPositions[1],
                    Track4.transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GuitaristTracks();
            Track2.transform.position =
                new Vector3(Track2.transform.position.x,
                    GuitarPositions[0],
                    Track2.transform.position.z);
            Track3.transform.position =
                new Vector3(Track3.transform.position.x,
                    GuitarPositions[1],
                    Track3.transform.position.z);
            Track4.transform.position =
                new Vector3(Track4.transform.position.x,
                    GuitarPositions[2],
                    Track4.transform.position.z);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PianistTracks();
            Track1.transform.position =
                new Vector3(Track1.transform.position.x,
                    PianoPositions[0],
                    Track1.transform.position.z);
            Track2.transform.position =
                new Vector3(Track2.transform.position.x,
                    PianoPositions[1],
                    Track2.transform.position.z);
            Track3.transform.position =
                new Vector3(Track3.transform.position.x,
                    PianoPositions[2],
                    Track3.transform.position.z);
            Track4.transform.position =
                new Vector3(Track4.transform.position.x,
                    PianoPositions[3],
                    Track4.transform.position.z);
        }
    }

    private void DrumerTracks()
    {
        Track1.SetActive(false);
        Track2.SetActive(false);
        Track3.SetActive(true);
        Track4.SetActive(true);

        Joystick1.SetActive(false);
        Joystick2.SetActive(false);
    }

    private void GuitaristTracks()
    {
        Track1.SetActive(false);
        Track2.SetActive(true);
        Track3.SetActive(true);
        Track4.SetActive(true);

        Joystick1.SetActive(false);
        Joystick2.SetActive(false);
    }

    private void PianistTracks()
    {
        Track1.SetActive(true);
        Track2.SetActive(true);
        Track3.SetActive(true);
        Track4.SetActive(true);

        Joystick1.SetActive(true);
        Joystick2.SetActive(true);
    }
}
