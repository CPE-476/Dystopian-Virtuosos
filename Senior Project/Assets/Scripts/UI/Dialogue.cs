using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public Spine spine;

    public TextMeshProUGUI textComponment;
    public TextMeshProUGUI name_field;

    public Image rika_image;
    public Image bronte_image;
    public Image three_image;
    public Image shopkeeper_image;
    public Image textbox;

    public float textSpeed;

    AudioSource audioSource;

    // Implementation Variables
    public int line_index = -1;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        textComponment.text = string.Empty;
        textComponment.enabled = false;
        audioSource.enabled = false;
        textbox.enabled = false;

        rika_image.enabled = false;
        bronte_image.enabled = false;
        three_image.enabled = false;
        shopkeeper_image.enabled = false;
    }

    public void Enable()
    {
        textComponment.enabled = true;
        name_field.enabled = true;
        audioSource.enabled = true;
        textbox.enabled = true;

        line_index = -1;

        NextLine();
    }

    public void Disable()
    {
        audioSource.enabled = false;
        textComponment.enabled = false;
        textbox.enabled = false;
        StopAllCoroutines();
        textComponment.text = string.Empty;
        name_field.enabled = false;
        rika_image.enabled = false;
        bronte_image.enabled = false;
        three_image.enabled = false;
        shopkeeper_image.enabled = false;
    }

    // Returns false if no lines left.
    public int NextLine()
    {
        if (line_index < spine.sections[spine.section_index].conversation.Length - 1)
        {
            if (line_index >= 0) GetImage(spine.sections[spine.section_index].conversation[line_index].character).enabled = false;
            line_index++;
            GetImage(spine.sections[spine.section_index].conversation[line_index].character).enabled = true;

            name_field.text = GetName(spine.sections[spine.section_index].conversation[line_index].character);
            textComponment.text = string.Empty;
            StopAllCoroutines();
            StartCoroutine(EmitLine());
            return line_index;
        }

        // Conversation is over.
        GetImage(spine.sections[spine.section_index].conversation[line_index - 1].character).enabled = false;
        name_field.enabled = false;
        return -1;
    }

    public IEnumerator EmitLine()
    {
        audioSource.Play();
        char[] curLine = spine.sections[spine.section_index].conversation[line_index].line.ToCharArray();
        foreach (char c in curLine)
        {
            textComponment.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Pause();
    }

    public string GetName(Character character)
    {
        switch (character)
        {
            case Character.RIKA:
                return "RIKA";
            case Character.BRONTE:
                return "BRONTE";
            case Character.THREE:
                return "Three";
            case Character.SHOPKEEPER:
                return "Shopkeeper";
            default:
                return "ERROR";
        }
    }

    public Image GetImage(Character character)
    {
        switch (character)
        {
            case Character.RIKA:
                return rika_image;
            case Character.BRONTE:
                return bronte_image;
            case Character.THREE:
                return three_image;
            case Character.SHOPKEEPER:
                return shopkeeper_image;
            default:
                return rika_image;
        }
    }
}
