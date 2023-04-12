using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponment;

    public NPC npc;

    public Image textbox;

    public string[] lines;

    public float textSpeed;

    private int index;

    private bool isStart = false;

    public CameraController cam;

    AudioSource audioSource; 


    // Start is called before the first frame update
    void Start()
    {
        textComponment.text = string.Empty;
        // StartDialogue();
        textComponment.enabled = false;
        textbox.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cam.isMoving)
        {
            if (!isStart){
                StartDialogue();
                /*audioSource.Play();*/
                isStart = true;
            }
            textComponment.enabled = true;
            textbox.enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                if (textComponment.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    audioSource.enabled = false;
                    textbox.enabled = false;
                    StopAllCoroutines();
                    textComponment.text = string.Empty;
                }
            }
        }
        else
        {
            index = 0;
            textComponment.enabled = false;
            textbox.enabled = false;
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        audioSource.Play();
        foreach (char c in lines[index].ToCharArray())
        {
            textComponment.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Pause();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponment.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        // else
        // {
        //     gameObject.SetActive(false);
        // }
    }
}
