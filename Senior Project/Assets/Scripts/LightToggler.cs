using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggler : MonoBehaviour
{
	public float minIntensity;
	public float maxIntensity;
	public bool startAtMin;
	public Conductor conductor;

	float lastBeat;

	Light myLight;
	SpriteRenderer mySprite;

    // Start is called before the first frame update
    void Start()
    {
		lastBeat = 0.0f;
		myLight = GetComponent<Light>();
		mySprite = GetComponent<SpriteRenderer>();
		myLight.intensity = startAtMin ? minIntensity : maxIntensity;
    }

    // Update is called once per frame
    void Update()
    {
		if(conductor.songPosition > lastBeat + conductor.crotchet)
		{
			ToggleLight();
			ToggleSprite();
			lastBeat += conductor.crotchet;
		}
	}

	// function to toggle between two intensities
	public void ToggleLight()
	{
		if(myLight.intensity == minIntensity)
		{
			myLight.intensity = maxIntensity;
		}
		else if(myLight.intensity == maxIntensity)
		{
			myLight.intensity = minIntensity;
		}
		else
		{
			Debug.Log("LightToggler ERROR\n");
		}
	}

	// function to toggle a cube on/off
	public void ToggleSprite()
	{
		if(mySprite.color == Color.black)
		{
			mySprite.color = Color.white;
		}
		else if(mySprite.color == Color.white)
		{
			mySprite.color = Color.black;
		}
		else
		{
			Debug.Log("LightToggler ERROR\n");
		}
	}
}
