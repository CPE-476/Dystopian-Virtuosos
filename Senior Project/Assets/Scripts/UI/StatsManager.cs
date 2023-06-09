using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;

public class StatsManager : MonoBehaviour
{
    public NoteTrigger noteTrigger;
    public ScoreManager scoreManager;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI perfectText;
    public TextMeshProUGUI goodText;
    public TextMeshProUGUI missText;
    public TextMeshProUGUI maxcomboText;
    public TextMeshProUGUI accuracyText;

    public float onChangePannelRatio;
    public float onChangeLeftRatio;
    public float onChangeRightRatio;

    private float targetPannelPos;
    private float targetLeftPos;
    private float targetRightPos;

    void Start()
    {
        targetPannelPos = Screen.width * onChangePannelRatio;
        targetLeftPos = Screen.width * onChangeLeftRatio;
        targetRightPos = Screen.width * onChangeRightRatio;
        //DisableStats();
    }


    void Update()
    {
        scoreText.text = scoreManager.score.ToString();
        perfectText.text = noteTrigger.perfectNum.ToString();
        goodText.text = noteTrigger.goodNum.ToString();
        missText.text = noteTrigger.missNum.ToString();
        maxcomboText.text = noteTrigger.maxCombo.ToString();
        accuracyText.text = noteTrigger.accuracy.ToString("0.#") + " %";
/*        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            StartCoroutine(displayStatsUI(0.5f, 0.5f, 0.5f));
        }*/
    }

    public void EnableStats()
    {
        foreach (GameObject child in gameObject.transform)
        {
            foreach (GameObject text in child.transform)
            {
                // Disable the child GameObject
                if (text.GetComponent<TextMeshProUGUI>() != null)
                {
                    text.GetComponent<TextMeshProUGUI>().enabled = true;
                }
            }
        }

    }

    public void DisableStats()
    {
        foreach (GameObject child in gameObject.transform)
        {
            foreach (GameObject text in child.transform)
            {
                // Disable the child GameObject
                if (text.GetComponent<TextMeshProUGUI>() != null)
                {
                    text.GetComponent<TextMeshProUGUI>().enabled = false;
                }
            }
        }
    }

    public IEnumerator displayPannel(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            Vector3 startPannelPosition = gameObject.transform.GetChild(0).transform.position;
            Vector3 endPannelPosition = new Vector3(targetPannelPos,
                                                gameObject.transform.GetChild(0).transform.position.y,
                                                startPannelPosition.z);
            gameObject.transform.GetChild(0).transform.position = Vector3.Lerp(startPannelPosition, endPannelPosition, t);

            yield return null;
        }
    }

    public IEnumerator displayRight(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            for (int i = 1; i < 5; i++)
            {
                Vector3 startLeftPosition = gameObject.transform.GetChild(i).transform.position;
                Vector3 endLeftPosition = new Vector3(targetLeftPos,
                                                    gameObject.transform.GetChild(i).transform.position.y,
                                                    startLeftPosition.z);
                gameObject.transform.GetChild(i).transform.position = Vector3.Lerp(startLeftPosition, endLeftPosition, t);
            }

            yield return null;
        }
    }

    public IEnumerator displayLeft(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            for (int i = 5; i < 7; i++)
            {
                Vector3 startRightPosition = gameObject.transform.GetChild(i).transform.position;
                Vector3 endRightPosition = new Vector3(targetRightPos,
                                                    gameObject.transform.GetChild(i).transform.position.y,
                                                    startRightPosition.z);
                gameObject.transform.GetChild(i).transform.position = Vector3.Lerp(startRightPosition, endRightPosition, t);
            }

            yield return null;
        }
    }



    public IEnumerator displayStatsUI(float t1, float t2, float t3)
    {
        StartCoroutine(displayPannel(t1));
        yield return new WaitForSeconds(t1);
        StartCoroutine(displayRight(t2));
        yield return new WaitForSeconds(t2);
        StartCoroutine(displayLeft(t3));
        yield return new WaitForSeconds(t3);
    }
}
