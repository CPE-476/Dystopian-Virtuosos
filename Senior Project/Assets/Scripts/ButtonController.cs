using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour
{
    public Button[] menuButtons;
    private int selectedButtonIndex = 0;

    void Start()
    {
        menuButtons = FindObjectsOfType<Button>();
    }

    void Update()
    {
        
    }
}
