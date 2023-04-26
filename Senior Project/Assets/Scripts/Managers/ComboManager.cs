using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public int comboNumber = 0;

    public GameObject combo;

    public GameObject comment;

    public GameObject comboHub;

    public float moveSpeed = 1.0f;

    private Vector3 originalPosition;

    string comboText;

    string commentText;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = comboHub.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (comboNumber != 0)
        {
            comboHub.transform.position =
                Vector3
                    .Lerp(comboHub.transform.position,
                    originalPosition,
                    Time.deltaTime * moveSpeed);
            comboText = comboNumber.ToString();
            if (comboNumber >= 200)
            {
                commentText = "MAESTRO!";
            }
            else if (comboNumber >= 150)
            {
                commentText = "VIRTUOSO!";
            }
            else if (comboNumber >= 100)
            {
                commentText = "GROOVE!";
            }
            else if (comboNumber >= 75)
            {
                commentText = "ELITE!";
            }
            else if (comboNumber >= 50)
            {
                commentText = "RHYTHM!";
            }
            else if (comboNumber >= 30)
            {
                commentText = "FINESSE!";
            }
            else
            {
                commentText = "NOVICE!";
            }
        }
        else
        {
            Vector3 targetPosition = originalPosition;
            targetPosition.x -= 700.0f;
            comboHub.transform.position =
                Vector3
                    .Lerp(comboHub.transform.position,
                    targetPosition,
                    Time.deltaTime * moveSpeed);
            comboText = "";
            commentText = "";
        }
        combo.GetComponent<TMPro.TextMeshProUGUI>().text = comboText;
        comment.GetComponent<TMPro.TextMeshProUGUI>().text = commentText;
    }
}
