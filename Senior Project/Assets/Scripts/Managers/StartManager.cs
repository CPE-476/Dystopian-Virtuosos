using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject

            launchButton,
            systemButton,
            aboutButton,
            quitButton;

    GameObject Default;

    GameObject Launch;

    GameObject System;

    GameObject About;

    public GameObject Background;

    public float zoomAmount = 5f;

    public float fadeDuration = 3.0f;

    public Image fadeImage;

    public AnimationCurve zoomCurve;

    private bool isLaunching = false;

    private float startTime;

    void Start()
    {
        init();
    }

    void Update()
    {
        if (isLaunching)
        {
            float bg_y = Background.transform.position.y;

            float max_zoom_y = 600f;
            float max_zoom_scale = 4.0f;
            float time = 2.5f;
            if (Background.transform.localScale.x > 3.9f)
            {
                SceneManager.LoadScene("Game");
            }
            if (Background.transform.localScale.x > 3.0f)
            {
                FadeOut();
            }
            if (bg_y > max_zoom_y)
            {
                float t = (Time.time - startTime) / time;
                float curveValue = zoomCurve.Evaluate(t);

                // Calculate the new position and scale values based on the current position and maximum zoom values
                float new_y =
                    Mathf.Lerp(bg_y, max_zoom_y, curveValue * Time.deltaTime);
                float new_scale =
                    Mathf
                        .Lerp(Background.transform.localScale.x,
                        max_zoom_scale,
                        curveValue * Time.deltaTime);

                // Set the new position and scale values on the background
                Vector3 new_position =
                    new Vector3(Background.transform.position.x,
                        new_y,
                        Background.transform.position.z);
                Background.transform.position = new_position;
                Background.transform.localScale =
                    new Vector3(new_scale,
                        new_scale,
                        Background.transform.localScale.z);
            }
        }
    }

    public void toLaunch()
    {
        Default.SetActive(false);
        Launch.SetActive(true);
        System.SetActive(false);
        About.SetActive(false);

        // Load the next scene
        // Zoom the camera in
        isLaunching = true;
        startTime = Time.time;
    }

    public void toSystem()
    {
        // Show the system settings UI
        Default.SetActive(false);
        Launch.SetActive(false);
        System.SetActive(true);
        About.SetActive(false);
    }

    public void toAbout()
    {
        // Show the about UI
        Default.SetActive(false);
        Launch.SetActive(false);
        System.SetActive(false);
        About.SetActive(true);
    }

    public void toDefault()
    {
        // Show the default UI
        Default.SetActive(true);
        Launch.SetActive(false);
        System.SetActive(false);
        About.SetActive(false);
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }

    public void init()
    {
        Default = GameObject.Find("Default");
        Launch = GameObject.Find("Launch");
        System = GameObject.Find("System");
        About = GameObject.Find("About");
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;
        toDefault();
    }

    void FadeOut()
    {
        // Fade the screen to black
        StartCoroutine(FadeImage(fadeImage, 1f, fadeDuration));
    }

    IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        Color currentColor = image.color;
        Color targetColor =
            new Color(currentColor.r,
                currentColor.g,
                currentColor.b,
                targetAlpha);
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float normalizedTime = t / duration;
            image.color = Color.Lerp(currentColor, targetColor, normalizedTime);
            yield return null;
        }
    }
}
