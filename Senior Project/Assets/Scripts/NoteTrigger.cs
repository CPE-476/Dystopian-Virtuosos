using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
	public float minIntensity;
	public float maxIntensity;
	public bool startAtMin;
	public Conductor conductor;
	public bool canBePressed;
	public ParticleSystem particles;

	//Temporary Metronome
	public AudioSource[] sounds;

    float lastBeat;

	Light myLight;
	SpriteRenderer mySprite;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponents<AudioSource>();
        lastBeat = 0.0f;
		myLight = GetComponent<Light>();
		mySprite = GetComponent<SpriteRenderer>();
		myLight.intensity = startAtMin ? minIntensity : maxIntensity;
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKey(KeyCode.Space) && canBePressed)
		{
			sounds[1].Play();
            ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
			Destroy(clone.gameObject, 0.5f);
			canBePressed = false;
        }
        if (conductor.songPosition > lastBeat + conductor.crotchet)
		{
			ToggleLight();
			ToggleSprite(0);
			sounds[0].Play(); //metronome
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
	public void ToggleSprite(int score)
	{
		Color[] colors = {Color.black, Color.green, Color.yellow};

		if(mySprite.color !=  Color.white)
		{
			mySprite.color = Color.white;
		}
		else if(mySprite.color == Color.white)
		{
			mySprite.color = colors[score];
		}
		else
		{
			Debug.Log("LightToggler ERROR\n");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Note")
		{
			canBePressed = true;
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Note")
        {
            canBePressed = false;
        }
    }
}
