using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;

struct InputLegend
{
    public bool H;
    public bool J;
    public bool K;
    public bool L;
}

public class PlayerBattle : MonoBehaviour
{
    InputLegend input;

    public int bpm = 100;
    public int frame = 0;
    public int note;
    public int notesPerBar = 4;
    public float offset = 0.2f;
    public float timeElapsed = 0;
    public int chances = 1;
    public float songPosition;

    public bool pressable = false;

    // Start is called before the first frame update
    void Start()
    {
        note = 60 / bpm;
    }

    public void OutputDebugString()
    {
        string debugString = "DEBUG: ";
        debugString += frame + " | ";
        debugString += timeElapsed + " | ";
        debugString += Time.deltaTime + "\n";


        debugString += songPosition;

        GetComponent<TextMesh>().text = debugString;
    }

    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            input.H = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            input.J = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            input.K = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            input.L = true;
        }
        if (input.H)
        {
            Debug.Log("H!\n");
        }
    }

    public void CheckPressable()
    {
    }

    public void Update()
    {
        // Change Debug Text Per Frame
        OutputDebugString();

        // Check Inputs
        CheckInput();

        // Check for pressable
        CheckPressable();

        // Update Variables Per Frame
        frame += 1;
        timeElapsed += Time.deltaTime;
        songPosition = (float)(AudioSettings.dspTime) - offset;
    }
}
