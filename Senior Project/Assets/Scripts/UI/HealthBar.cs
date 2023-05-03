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

    public float moveSpeed;

    public float targetY;

    public void Start()
    {
        //targetX = 2000;
    }

    public void Update()
    {
        if (transform.position.y < targetY)
        {
            transform.position =
                Vector3
                    .Lerp(transform.position,
                    new Vector3(transform.position.x,
                        targetY,
                        transform.position.z),
                    Time.deltaTime * moveSpeed);
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
