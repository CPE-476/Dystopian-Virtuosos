using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSFX : MonoBehaviour
{
    public AudioSource buttonClick;
    public AudioSource buttonHover;
    public AudioSource zoom;

    public void PlaybuttonClick()
    {
        buttonClick.Play();
    }

    public void PlaybuttonHover()
    {
        buttonHover.Play();
    }

    public void Playzoom()
    {
        zoom.Play();
    }

}
