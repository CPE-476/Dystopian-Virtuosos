using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum HitCategory
{
	NONE,
	WEAK,
	GOOD
};

public class NoteTrigger : MonoBehaviour
{
	public Conductor conductor;
	public bool canBePressed;
	public ParticleSystem particles;

	//Temporary Metronome
	public AudioSource[] sounds;

    public float currentBeat;
	public float lowerGoodBound;
	public float lowerWeakBound;
	public float upperGoodBound;
	public float upperWeakBound;
	public float threshold = 0.1f;

	public HitCategory hc;

	Light myLight;
	SpriteRenderer mySprite;

    // Start is called before the first frame update
    void Start()
    {
        sounds = GetComponents<AudioSource>();
        currentBeat = 0.0f;
		myLight = GetComponent<Light>();
		mySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		if (conductor.songPosition > lowerGoodBound &&
			conductor.songPosition < upperGoodBound)
		{
			myLight.color = Color.green;
			hc = HitCategory.GOOD;
		}
		else if (conductor.songPosition > lowerWeakBound &&
			conductor.songPosition < upperWeakBound)
		{
			myLight.color = Color.blue;
			hc = HitCategory.WEAK;
		}
		else
        {
			myLight.color = Color.red;
			hc = HitCategory.NONE;
        }

		if (Input.GetKeyDown(KeyCode.Space) && canBePressed)
		{
			if (hc == HitCategory.NONE)
			{
				sounds[1].Play();
				ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
				mySprite.color = Color.red;
				Destroy(clone.gameObject, 0.5f);
			}
			else if (hc == HitCategory.WEAK)
			{
				sounds[2].Play();
				ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
				mySprite.color = Color.blue;
				Destroy(clone.gameObject, 0.5f);
			}
			else if (hc == HitCategory.GOOD)
			{
				sounds[3].Play();
				ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
				clone.startColor = Color.green;
				mySprite.color = Color.green;
				Destroy(clone.gameObject, 0.5f);
			}
			canBePressed = false;
		}

		// Set the next beat when current beat is over.
        if (conductor.songPosition > currentBeat + (conductor.crotchet / 2.0f))
		{
			sounds[0].Play(); //metronome
			currentBeat += conductor.crotchet;
			lowerGoodBound = currentBeat - threshold;
			lowerWeakBound = currentBeat - threshold * 2;
			upperGoodBound = currentBeat + threshold;
			upperWeakBound = currentBeat + threshold * 2;
			canBePressed = true;
		}
	}
}
