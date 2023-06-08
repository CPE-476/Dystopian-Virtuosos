using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Narrative : MonoBehaviour
{
    public TextMeshProUGUI textComponment;

    public Image textbox;

    public GameObject image;

    public string[] lines;

    public Sprite[] images;

    public Image fadePanel;

    public int[] sectionBreak;

    public int currentSection = 0;

    public float textSpeed;

    public int narrative_index = -1;

    public int image_index = 0;

    public float fadeTime = 2.0f;

    public float fadeTime_2 = 1.0f;

    private float currentTime = 0.0f;

    private bool sceneStarting = true;

    private string dialoguePath;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        dialoguePath = Application.streamingAssetsPath + "/Dialogue/";

        textComponment.text = string.Empty;
        fadePanel.color =
            new Color(fadePanel.color.r,
                fadePanel.color.g,
                fadePanel.color.b,
                1.0f);

        if(1 == PlayerPrefs.GetInt("level_number")) {
            readNarrativeFile("cutscene_1.txt");
        }
        if(2 == PlayerPrefs.GetInt("level_number")) {
            readNarrativeFile("cutscene_2.txt");
        }
    }

    private void readNarrativeFile(string file_name)
    {
        List<string> list = new List<string>();
        StreamReader inp_stm = new StreamReader(dialoguePath + file_name);
        while(!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            if(inp_ln.Length != 0 && (inp_ln[0] != '#')) {
                list.Add(inp_ln);
            }
        }
        inp_stm.Close();

        lines = list.ToArray();
    }

    public void Enable()
    {
        textComponment.enabled = true;
        textbox.enabled = true;

        if (currentSection != 0)
        {
            narrative_index = sectionBreak[currentSection - 1] - 1;
        }
        else
        {
            narrative_index = -1;
            image_index = 0;
        }
        NextLine();
    }

    public void Disable()
    {
        textComponment.enabled = false;
        textbox.enabled = false;
        StopAllCoroutines();
        textComponment.text = string.Empty;
    }

    public int NextLine()
    {
        if (narrative_index < lines.Length - 1)
        {
            narrative_index++;
            if(lines[narrative_index] == "---") {
                NextImage();
                narrative_index++;
            }
            textComponment.text = string.Empty;
            StopAllCoroutines();
            StartCoroutine(EmitLine());
            return narrative_index;
        }
        return -1;
    }

    public IEnumerator EmitLine()
    {
        char[] curLine = lines[narrative_index].ToCharArray();
        foreach (char c in curLine)
        {
            textComponment.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public bool NextImage()
    {
        if (image_index < images.Length - 1)
        {
            image_index++;
            image.GetComponent<Image>().sprite = images[image_index];
            return true;
        }
        return false;
    }

    public void FadeToClear()
    {
        // Reduce the alpha of the panel by the amount of time that has passed
        fadePanel.color =
            Color
                .Lerp(fadePanel.color,
                Color.clear,
                Time.deltaTime / fadeTime_2);

        // Once the alpha is close to zero, disable the panel and stop fading
        if (fadePanel.color.a <= 0.05f)
        {
            fadePanel.color = Color.clear;
        }
    }

    public void FadeToBlack()
    {
        // Increase the alpha of the panel by the amount of time that has passed
        fadePanel.color =
            Color
                .Lerp(fadePanel.color,
                Color.black,
                Time.deltaTime / (fadeTime_2 / 2.0f));

        audioSource.volume = Mathf.Lerp(audioSource.volume, 0.0f, Time.deltaTime / (fadeTime_2 / 2.0f));

        // Once the alpha is close to one, load the next scene
        if (fadePanel.color.a >= 0.95f)
        {
            SceneManager.LoadScene("Game");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneStarting)
        {
            // Fade the panel out over the specified duration
            FadeToClear();
        }
        else
        {
            FadeToBlack();
        }

        currentTime += Time.deltaTime;

        // Calculate the alpha value based on the current time and fade time
        float alpha = currentTime / fadeTime;
        alpha = Mathf.Clamp01(alpha);

        Color color = textbox.GetComponent<Image>().color;
        color.a = alpha;

        if (color.a < 0.75f)
        {
            textbox.GetComponent<Image>().color = color;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            int result = NextLine();
            if (result == -1)
            {
                Disable();
                sceneStarting = false;
            }
        }
    }
}
