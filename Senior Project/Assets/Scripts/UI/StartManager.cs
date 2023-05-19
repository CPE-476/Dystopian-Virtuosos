using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject

            launchButton,
            aboutButton,
            quitButton,
            systemTitle,
            systemLC,
            systemBack;

    GameObject Default;

    GameObject Launch;

    GameObject stitle;
    GameObject sLC;
    GameObject sBack;
    GameObject sLatency;

    GameObject About;

    public GameObject Background;

    public float zoomAmount = 5f;

    public float fadeDuration = 3.0f;

    public Image fadeImage;

    public AnimationCurve zoomCurve;

    private bool isLaunching = false;

    private float startTime;

    public GameObject optionsBackButton, firstButton, extraBackButton;

    public GameObject cursorImage;

    void Awake()
    {
        init();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    void Update()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;

        if (currentButton != null)
        {
            RectTransform currentButtonRectTransform = currentButton.GetComponent<RectTransform>();
            RectTransform cursorRectTransform = cursorImage.GetComponent<RectTransform>();
            cursorRectTransform.position = new Vector3(currentButtonRectTransform.position.x - (currentButtonRectTransform.rect.width/2)*0.1f -20,
                                                       currentButtonRectTransform.position.y,
                                                       currentButtonRectTransform.position.z);
        }
        if (isLaunching)
        {
            //Debug.Log(Background);
            float bg_y = Background.transform.position.y;

            float max_zoom_y = 250f;
            float max_zoom_scale = 4.0f;
            float time = 2.5f;
            //Debug.Log(Background.transform.localScale.x);
            if (Background.transform.localScale.x > 3.9f)
            {
                SceneManager.LoadScene("CutScene");
            }
            if (Background.transform.localScale.x > 3.0f)
            {
                FadeOut();
            }
            //Debug.Log(bg_y);
            if (bg_y > max_zoom_y)
            {
                //Debug.Log(bg_y);
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
        SetSystem(false);
        About.SetActive(false);
        cursorImage.SetActive(false);
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
        SetSystem(true);
        About.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsBackButton);
    }

    public void toAbout()
    {
        // Show the about UI
        Default.SetActive(false);
        Launch.SetActive(false);
        SetSystem(false);
        About.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(extraBackButton);
    }

    public void toDefault()
    {
        // Show the default UI
        Default.SetActive(true);
        Launch.SetActive(false);
        SetSystem(false);
        About.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
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
        stitle = GameObject.Find("SystemTitle");
        sLC = GameObject.Find("LatencyCalibrator");
        sBack = GameObject.Find("BackButton");
        sLatency = GameObject.Find("Latency");
        About = GameObject.Find("About");
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;
        toDefault();
    }

    void SetSystem(bool b)
    {
        if (b)
        {
            stitle.SetActive(true);
            sBack.SetActive(true);
            sLatency.SetActive(true);
            foreach (Transform child in sLC.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            stitle.SetActive(false);
            sBack.SetActive(false);
            sLatency.SetActive(false);
            foreach (Transform child in sLC.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
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
