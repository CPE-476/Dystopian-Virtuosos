using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject countIn;

    Coroutine resumeCountdownCoroutine;

    private TMP_Text countInText;

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
        AudioListener.pause = true;
        Time.timeScale = 0f;
        StopResumeCountdownCoroutine(); // Stop the coroutine if it's already running
    }

    public void toResume()
    {
        pauseMenu.SetActive(false);
        countIn.SetActive(true);
        countInText = countIn.GetComponent<TMP_Text>();
        resumeCountdownCoroutine = StartCoroutine(ResumeCountdownCoroutine()); // Start the coroutine
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
