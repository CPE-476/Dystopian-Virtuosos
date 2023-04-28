using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject System;

    [SerializeField]
    GameObject Base;

    [SerializeField]
    GameObject countIn;

    [SerializeField]
    GameObject comboHub;

    [SerializeField]
    float moveSpeed;

    Coroutine resumeCountdownCoroutine;

    private TMP_Text countInText;

    private Vector3 originalPosition;

    private Image hub;

    public GameObject pauseFirstButton, optionsFirstButton, backButton;

    public void Start()
    {
        originalPosition = comboHub.transform.position;
        hub = comboHub.GetComponent<Image>();
    }

    void Update()
    {
        if (
            SceneManager.GetActiveScene().name == "Game" &&
            Input.GetKeyDown(KeyCode.Escape)
        )
        {
            toPause();
        }
    }

    public void toPause()
    {
        pauseMenu.SetActive(true);
        System.SetActive(false);
        AudioListener.pause = true;
        hub.enabled = false;
        Time.timeScale = 0f;
        StopResumeCountdownCoroutine(); // Stop the coroutine if it's already running

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);

        // Add the following code to move the comboHub down
        if (comboHub.activeSelf)
        {
            StartCoroutine(MoveComboHubCoroutine(originalPosition,
            new Vector3(originalPosition.x,
                originalPosition.y - 860.0f,
                originalPosition.z)));
        }
    }

    public void toResume()
    {
        pauseMenu.SetActive(false);
        countIn.SetActive(true);
        hub.enabled = true;
        countInText = countIn.GetComponent<TMP_Text>();
        resumeCountdownCoroutine = StartCoroutine(ResumeCountdownCoroutine()); // Start the coroutine
        EventSystem.current.SetSelectedGameObject(null);

        // Add the following code to move the comboHub up
        if (comboHub.activeSelf)
        {
            StartCoroutine(MoveComboHubCoroutine(new Vector3(originalPosition.x,
                originalPosition.y - 100f,
                originalPosition.z),
            originalPosition));
        }
    }

    public void toSystem()
    {
        Base.SetActive(false);
        System.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void toBase()
    {
        System.SetActive(false);
        Base.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    IEnumerator ResumeCountdownCoroutine()
    {
        const float countdownTime = 3.0f; // Countdown time in seconds
        float remainingTime = countdownTime;

        while (remainingTime > 0.5f)
        {
            countInText.text = remainingTime.ToString("F0");
            yield return new WaitForSecondsRealtime(0.1f); // Update the remaining time every 0.1 seconds
            remainingTime -= 0.1f;
        }
        countIn.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }

    IEnumerator
    MoveComboHubCoroutine(Vector3 startPosition, Vector3 endPosition)
    {
        float t = 0f;
        while (t < 1f)
        {
            comboHub.transform.position =
                Vector3.Lerp(startPosition, endPosition, t);
            t += Time.unscaledDeltaTime * moveSpeed;
            yield return null;
        }
        comboHub.transform.position = endPosition;
    }

    void StopResumeCountdownCoroutine()
    {
        if (resumeCountdownCoroutine != null)
        {
            StopCoroutine (resumeCountdownCoroutine);
        }
    }

    public void toStart(int id)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene (id);
    }
}
