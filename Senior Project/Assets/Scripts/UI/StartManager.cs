using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum CURRENTPAGE
{
    DEFAULT,
    SYSTEM,
    GRAPHIC,
    SOUND,
    CALIBRATE,
    ABOUT
}

public class StartManager : MonoBehaviour
{

    public GameObject systemTitle;
    public GameObject backButton;

    public GameObject Default;
    public GameObject Launch;
    public GameObject System;
    public GameObject Sound;
    public GameObject Graphic;
    public GameObject About;

    public GameObject systemLC;

    private GameObject graphicButton;
    private GameObject soundButton;
    private GameObject calibrationButton;


    GameObject sLC;
    GameObject sLatency;

    public GameObject Background;

    public float zoomAmount = 5f;

    public float fadeDuration = 3.0f;

    public Image fadeImage;

    public AnimationCurve zoomCurve;

    public bool isLaunching = false;

    private float startTime;

    public GameObject curGraphicsButton, curLaunchButton, curBackButton, curFull, curMaster;

    public GameObject cursorImage;

    public CURRENTPAGE currentPage;

    public AudioSource audioSource;

    void Awake()
    {
        init();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(curLaunchButton);
    }

    void Update()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;

        if (currentButton != null)
        {
            RectTransform currentButtonRectTransform = currentButton.GetComponent<RectTransform>();
            RectTransform cursorRectTransform = cursorImage.GetComponent<RectTransform>();
            float leftmostX = currentButtonRectTransform.position.x - (currentButtonRectTransform.pivot.x * currentButtonRectTransform.rect.width);
            cursorRectTransform.position = new Vector3(leftmostX,
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
                GameObject musicGameObject = audioSource.gameObject;
                Destroy(musicGameObject);
                SceneManager.LoadScene("CutScene");
            }
            if(Background.transform.localScale.x > 3.0f)
            {
                FadeOut();
                float interp_factor = (Background.transform.localScale.x - 3.0f);
                audioSource.volume = 1 - interp_factor;
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

    public void init()
    {
        currentPage = CURRENTPAGE.DEFAULT;
        systemTitle = GameObject.Find("SystemTitle");
        sLC = GameObject.Find("LatencyCalibrator");
        sLatency = GameObject.Find("Latency");

        systemTitle.GetComponent<TextMeshProUGUI>().enabled = false;

        graphicButton = GameObject.Find("GraphicButton");
        graphicButton.GetComponent<TextMeshProUGUI>().enabled = false;
        graphicButton.GetComponent<Button>().interactable = false;

        soundButton = GameObject.Find("SoundButton");
        soundButton.GetComponent<TextMeshProUGUI>().enabled = false;
        soundButton.GetComponent<Button>().interactable = false;

        calibrationButton = GameObject.Find("CalibrateButton");
        calibrationButton.GetComponent<TextMeshProUGUI>().enabled = false;
        calibrationButton.GetComponent<Button>().interactable = false;

        backButton.GetComponent<TextMeshProUGUI>().enabled = false;
        backButton.GetComponent<Button>().interactable = false;

        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;
        toDefault();
    }

    public void toLaunch()
    {
        Default.SetActive(false);
        Launch.SetActive(true);
        SetCalibration(false);
        About.SetActive(false);
        cursorImage.SetActive(false);
        // Load the next scene
        // Zoom the camera in
        isLaunching = true;
        startTime = Time.time;
    }

    public void toSystem()
    {
        currentPage = CURRENTPAGE.SYSTEM;
        // Show the system settings UI
        Default.SetActive(false);
        Launch.SetActive(false);
        About.SetActive(false);
 
        systemTitle.GetComponent<TextMeshProUGUI>().enabled = true;
        systemTitle.GetComponent<TextMeshProUGUI>().text = "SYSTEM";
        graphicButton.GetComponent<TextMeshProUGUI>().enabled = true;
        graphicButton.GetComponent<Button>().interactable = true;

        soundButton.GetComponent<TextMeshProUGUI>().enabled = true;
        soundButton.GetComponent<Button>().interactable = true;

        calibrationButton.GetComponent<TextMeshProUGUI>().enabled = true;
        calibrationButton.GetComponent<Button>().interactable = true;

        backButton.GetComponent<TextMeshProUGUI>().enabled = true;
        backButton.GetComponent<Button>().interactable = true;

        EventSystem.current.SetSelectedGameObject(curGraphicsButton);
    }

    public void back()
    {
        if (currentPage == CURRENTPAGE.SYSTEM)
        {
            // update title
            systemTitle.GetComponent<TextMeshProUGUI>().enabled = false;

            // disable buttons
            graphicButton.GetComponent<TextMeshProUGUI>().enabled = false;
            graphicButton.GetComponent<Button>().interactable = false;

            soundButton.GetComponent<TextMeshProUGUI>().enabled = false;
            soundButton.GetComponent<Button>().interactable = false;

            calibrationButton.GetComponent<TextMeshProUGUI>().enabled = false;
            calibrationButton.GetComponent<Button>().interactable = false;

            backButton.GetComponent<TextMeshProUGUI>().enabled = false;
            backButton.GetComponent<Button>().interactable = false;

            // back to Default
            toDefault();


        }
        else if (currentPage == CURRENTPAGE.ABOUT)
        {
            // update title
            systemTitle.GetComponent<TextMeshProUGUI>().enabled = false;

            // disable buttons
            backButton.GetComponent<TextMeshProUGUI>().enabled = false;
            backButton.GetComponent<Button>().interactable = false;

            // back to Default
            toDefault();
        }
        else if (currentPage == CURRENTPAGE.GRAPHIC || currentPage == CURRENTPAGE.SOUND || currentPage == CURRENTPAGE.CALIBRATE)
        {
            SetCalibration(false);
            Sound.SetActive(false);
            Graphic.SetActive(false);
            toSystem();
            
        }
        else
        {
            Debug.Log("Shouldn't be here");
        }
    }

    public void toGraphic()
    {
        currentPage = CURRENTPAGE.GRAPHIC;
        graphicButton.GetComponent<TextMeshProUGUI>().enabled = false;
        graphicButton.GetComponent<Button>().interactable = false;

        soundButton.GetComponent<TextMeshProUGUI>().enabled = false;
        soundButton.GetComponent<Button>().interactable = false;

        calibrationButton.GetComponent<TextMeshProUGUI>().enabled = false;
        calibrationButton.GetComponent<Button>().interactable = false;

        systemTitle.GetComponent<TextMeshProUGUI>().text = "GRAPHIC";
        Graphic.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(curFull);
    }

        public void toSound()
    {
        currentPage = CURRENTPAGE.SOUND;

        // disable buttons
        graphicButton.GetComponent<TextMeshProUGUI>().enabled = false;
        graphicButton.GetComponent<Button>().interactable = false;

        soundButton.GetComponent<TextMeshProUGUI>().enabled = false;
        soundButton.GetComponent<Button>().interactable = false;

        calibrationButton.GetComponent<TextMeshProUGUI>().enabled = false;
        calibrationButton.GetComponent<Button>().interactable = false;

        systemTitle.GetComponent<TextMeshProUGUI>().text = "SOUND";
        Sound.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(curMaster);
    }

    public void toCalibration()
    {
        currentPage = CURRENTPAGE.CALIBRATE;
        // Show the calibration settings UI
        SetCalibration(true);

        // update title
        systemTitle.GetComponent<TextMeshProUGUI>().text = "CALIBRATE";

        // disable buttons
        graphicButton.GetComponent<TextMeshProUGUI>().enabled = false;
        graphicButton.GetComponent<Button>().interactable = false;

        soundButton.GetComponent<TextMeshProUGUI>().enabled = false;
        soundButton.GetComponent<Button>().interactable = false;

        calibrationButton.GetComponent<TextMeshProUGUI>().enabled = false;
        calibrationButton.GetComponent<Button>().interactable = false;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(curBackButton);
    }

    public void toAbout()
    {
        currentPage = CURRENTPAGE.ABOUT;
        // Show the about UI
        Default.SetActive(false);
        Launch.SetActive(false);
        SetCalibration(false);
        About.SetActive(true);

        systemTitle.GetComponent<TextMeshProUGUI>().enabled = true;
        systemTitle.GetComponent<TextMeshProUGUI>().text = "ABOUT";

        backButton.GetComponent<TextMeshProUGUI>().enabled = true;
        backButton.GetComponent<Button>().interactable = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(curBackButton);
    }

    public void toDefault()
    {
        // Show the default UI
        Default.SetActive(true);
        Launch.SetActive(false);
        SetCalibration(false);
        About.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(curLaunchButton);
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }

    void SetCalibration(bool b)
    {
        if (b)
        {
            sLatency.GetComponent<TextMeshProUGUI>().enabled = true;
            foreach (Transform child in sLC.transform)
            {
                child.gameObject.GetComponent<Image>().enabled = true;
            }
        }
        else
        {
            sLatency.GetComponent<TextMeshProUGUI>().enabled = false;
            foreach (Transform child in sLC.transform)
            {
                child.gameObject.GetComponent<Image>().enabled = false;
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
