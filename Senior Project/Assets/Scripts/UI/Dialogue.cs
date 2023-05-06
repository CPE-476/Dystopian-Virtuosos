using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Character {
    RIKA,
    BRONTE,
    THREE,
    SHOPKEEPER,
};

public class DialogueLine {
    public Character character;
    public string line;

    public DialogueLine(Character char_in, string line_in) {
        character = char_in;
        line = line_in;
    }
};

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponment;
    public TextMeshProUGUI name_field;

    public Image rika_image;
    public Image bronte_image;
    public Image three_image;
    public Image shopkeeper_image;

    public Image textbox;

    public DialogueLine[] lines = 
    {
        new DialogueLine(Character.RIKA, "Hi, I'm Rika!"),
        new DialogueLine(Character.RIKA, "I love you, Lucas!"),
        new DialogueLine(Character.BRONTE, "I'm here too!"),
        new DialogueLine(Character.RIKA, "Go Virtuosos!"),
    };

    public float textSpeed;

    AudioSource audioSource; 

    // Implementation Variables
    private int index;

    void Start()
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
        audioSource.enabled = true;
        textbox.enabled = true;

        index = -1;
        NextLine();
    }

    public void Disable()
    {
        audioSource.enabled = false;
        textComponment.enabled = false;
        textbox.enabled = false;
        StopAllCoroutines();
        textComponment.text = string.Empty;

        rika_image.enabled = false;
        bronte_image.enabled = false;
        three_image.enabled = false;
        shopkeeper_image.enabled = false;
    }

    // Returns false if no lines left.
    public bool NextLine()
    {
        if (index < lines.Length - 1) {
            if(index >= 0) GetImage(lines[index].character).enabled = false;
            index++;
            GetImage(lines[index].character).enabled = true;

            name_field.text = GetName(lines[index].character);
            textComponment.text = string.Empty;
            StartCoroutine(EmitLine());
            return true;
        }

        GetImage(lines[index].character).enabled = false;
        name_field.enabled = false;
        return false;
    }

    IEnumerator EmitLine()
    {
        audioSource.Play();
        Debug.Log(index);
        foreach (char c in lines[index].line.ToCharArray())
        {
            textComponment.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        audioSource.Pause();
    }

    public string GetName(Character character) {
        switch(character)
        {
            case Character.RIKA:
                return "Rika";
            case Character.BRONTE:
                return "Bronte";
            case Character.THREE:
                return "Three";
            case Character.SHOPKEEPER:
                return "Shopkeeper";
            default:
                return "ERROR";
        }
    }

    public Image GetImage(Character character) {
        switch(character)
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
