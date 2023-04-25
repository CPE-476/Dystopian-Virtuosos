using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public int combo = 0;
    string scoreText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (combo != 0)
        {
            scoreText = "Combo: " + combo;
        }
        else
        {
            scoreText = "";
        }
        GetComponent<TMPro.TextMeshProUGUI>().text = scoreText;
    } 
}
