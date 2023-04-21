using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public Gradient color;

    public Image fill;

    public CameraController cam;

    private GameObject[] healthSprite;

    public void Start(){
        healthSprite = GameObject.FindGameObjectsWithTag("health");
    }

    public void Update()
    {
        if (cam.isMoving)
        {
            foreach (GameObject item in healthSprite)
            {
                item.GetComponent<Image>().enabled = true;
            }
        }
        else {
            foreach (GameObject item in healthSprite)
            {
                item.GetComponent<Image>().enabled = false;
            }
        }
    }

    public void setMaxHealth(int maxhealth)
    {
        slider.maxValue = maxhealth;
        slider.value = maxhealth;
        color.Evaluate(1f);
    }

    // Start is called before the first frame update
    public void setHealth(int health)
    {
        slider.value = health;
        fill.color = color.Evaluate(slider.normalizedValue);
    }
}
