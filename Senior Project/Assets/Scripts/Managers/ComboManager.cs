using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public int combo = 0;

    public GameObject comment;

    string scoreText;

    string commentText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (combo != 0)
        {
            scoreText = combo.ToString();
            if (combo >= 200)
            {
                commentText = "VIRTUOSO!";
            }
            else if (combo >= 150)
            {
                commentText = "MAESTRO!";
            }
            else if (combo >= 100)
            {
                commentText = "GROOVE!";
            }
            else if (combo >= 75)
            {
                commentText = "ELITE!";
            }
            else if (combo >= 50)
            {
                commentText = "RHYTHM!";
            }
            else if (combo >= 30)
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
            scoreText = "";
            commentText = "";
        }
        GetComponent<TMPro.TextMeshProUGUI>().text = scoreText;
        comment.GetComponent<TMPro.TextMeshProUGUI>().text = commentText;
    }
}
