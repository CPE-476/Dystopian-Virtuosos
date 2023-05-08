using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Character
{
    RIKA,
    BRONTE,
    THREE,
    SHOPKEEPER
}

public class DialogueLine
{
    public Character character;

    public string line;

    public DialogueLine(Character char_in, string line_in)
    {
        character = char_in;
        line = line_in;
    }
}

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponment;

    public TextMeshProUGUI name_field;

    public Image rika_image;

    public Image bronte_image;

    public Image three_image;

    public Image shopkeeper_image;

    public Image textbox;

    public DialogueLine[]
        lines =
        {
            new DialogueLine(Character.RIKA,
                "Hey, have you heard the news about the planet?"),
            new DialogueLine(Character.BRONTE, "No, what's going on?"),
            new DialogueLine(Character.RIKA,
                "It's bad. Scientists just confirmed that the planet is heading towards a catastrophic event. They've been monitoring it for months, and it's just getting worse."),
            new DialogueLine(Character.RIKA,
                "Well, from what I've heard, the planet's core is destabilizing, and it's causing massive earthquakes and volcanic eruptions. "),
            new DialogueLine(Character.BRONTE,
                "That sounds terrible. Is there anything that can be done?"),
            new DialogueLine(Character.RIKA,
                "Unfortunately, no. The scientists have been working on solutions, but they say it's too late. The damage is already done, and there's nothing we can do to stop it. It's just a matter of waiting for the end."),
            new DialogueLine(Character.RIKA,
                "It's a tragedy. I just hope that the people on the planet have some kind of plan to evacuate or something. It's just too awful to think about."),
            new DialogueLine(Character.BRONTE,
                "Yeah, I agree. It's really depressing to think about an entire planet being destroyed like that."),
            new DialogueLine(Character.RIKA,
                "I know. It just goes to show how fragile life can be, and how we need to take care of our own planet to make sure something like this never happens to us.")
        };

    public int[] sectionBreak;

    public int currentSection = 0;

    public float textSpeed;

    AudioSource audioSource;

    // Implementation Variables
    public int index;

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
        name_field.enabled = true;
        audioSource.enabled = true;
        textbox.enabled = true;

        if (currentSection != 0)
        {
            index = sectionBreak[currentSection - 1] - 1;
        }
        else
        {
            index = -1;
        }
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
        if (index < lines.Length - 1)
        {
            if (index >= 0) GetImage(lines[index].character).enabled = false;
            index++;
            GetImage(lines[index].character).enabled = true;

            name_field.text = GetName(lines[index].character);
            textComponment.text = string.Empty;
            StartCoroutine(EmitLine());
            return index;
        }

        GetImage(lines[index].character).enabled = false;
        name_field.enabled = false;
        return -1;
    }

    public IEnumerator EmitLine()
    {
        audioSource.Play();
        char[] curLine = lines[index].line.ToCharArray();
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
