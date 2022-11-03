using System;
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
	public MIDIReader trackMaster;
	public ParticleSystem particles;
	public ParticleSystem hittext;
	public TMPro.TextMeshProUGUI text;

	public SpriteRenderer top;
	public SpriteRenderer high;
	public SpriteRenderer low;

	public SFX sfx;

	public ScoreManager scoreManager;

    public float currentSpot;
	public float noteEnd;
	public float lowerGoodBound;
	public float lowerWeakBound;
	public float upperGoodBound;
	public float upperWeakBound;
	public float innerThreshold = 0.05f;
	public float outerThreshold = 0.10f;

	[SerializeReference]
	private HitCategory hc;

	bool[] hasBeenPressed;

Light myLight;

    // Start is called before the first frame update
    void Start()
    {
        currentSpot = conductor.spotLength;
		noteEnd = conductor.spotLength + conductor.spotLength * 0.5f;
		myLight = GetComponent<Light>();
		hasBeenPressed = new bool[] { false, false, false, false};
	}

    // Update is called once per frame
    void Update()
    {	
		checkHit(KeyCode.Joystick1Button3, 0, top, KeyCode.H);
		checkHit(KeyCode.Joystick1Button2, 1, high, KeyCode.J);
		checkHit(KeyCode.Joystick1Button1, 2, low, KeyCode.K);

		// Set the current Hit Category
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

		// Set the next beat when current beat is over.
		if (conductor.songPosition > noteEnd)
		{
			currentSpot += conductor.spotLength;
			noteEnd += conductor.spotLength;
			lowerGoodBound = currentSpot - innerThreshold;
			lowerWeakBound = currentSpot - outerThreshold;
			upperGoodBound = currentSpot + innerThreshold;
			upperWeakBound = currentSpot + outerThreshold;
			trackMaster.changePressable();

		}
	}

	// Checks for a hit on a given keycode.
	// Returns true if hit, false if not.
	private void checkHit(KeyCode kc, int trackNumber, SpriteRenderer sprite, KeyCode kb)
	{
		if (Input.GetKeyDown(kc) || Input.GetKeyDown(kb))
		{
			if(trackMaster.pressable[trackNumber])
			{
				if (hc == HitCategory.WEAK)
				{
					sfx.sounds[2].Play();
					particles.startColor = Color.blue;
					ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
					text.text = "Weak";
					ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext, new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6), Quaternion.identity);
					sprite.color = Color.blue;
					Destroy(clone.gameObject, 0.5f);
					Destroy(clone2.gameObject, 1.0f);
					scoreManager.score += 1;
				}
				else if (hc == HitCategory.GOOD)
				{
					sfx.sounds[1].Play();
					particles.startColor = Color.green;
					ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
					text.text = "Perfect";
					ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext, new Vector3(sprite.transform.position.x, sprite.transform.position.y ,-6), Quaternion.identity);
					sprite.color = Color.green;
					Destroy(clone.gameObject, 0.5f);
					Destroy(clone2.gameObject, 1.0f);
					Debug.Log(scoreManager.score);
					scoreManager.score += 2;
					Debug.Log(scoreManager.score);
				}
			}
			else
			{
				sfx.sounds[3].Play();
				particles.startColor = Color.red;
				ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
				sprite.color = Color.red;
				Destroy(clone.gameObject, 0.5f);
				scoreManager.score += -1;
			}
			
			
		}
	}
}
