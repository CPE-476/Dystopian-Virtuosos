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
	public MIDIReader midiReader;
	public ParticleSystem particles;
	public ParticleSystem hittext;
	public PlayerController character;
	public TMPro.TextMeshProUGUI text;
	public Animator anim;

	public SpriteRenderer top;
	public SpriteRenderer high;
	public SpriteRenderer low;

	public Color perfect;
	public Color weak;
	public Color fail;


	public SFX sfx;

	public ScoreManager scoreManager;

    public float currentSpot;
	public float noteEnd;
	public float innerThreshold = 0.05f;
	public float outerThreshold = 0.10f;

	private float lowerGoodBound;
	private float lowerWeakBound;
	private float upperGoodBound;
	private float upperWeakBound;

	int transpose = 0;
	int[] notes = new int[] { 7, 3, 0 };

	[SerializeReference]
	private HitCategory hc;

	bool[] hasBeenPressed;

//Light myLight;

    // Start is called before the first frame update
    void Start()
    {
        currentSpot = conductor.spotLength;
		noteEnd = conductor.spotLength + conductor.spotLength * 0.5f;
		//myLight = GetComponent<Light>();
		hasBeenPressed = new bool[] { false, false, false, false};
	}

    // Update is called once per frame
    void Update()
    {	
		checkHit(KeyCode.Joystick1Button3, 0, top, KeyCode.H);
		checkHit(KeyCode.Joystick1Button2, 1, high, KeyCode.J);
		checkHit(KeyCode.Joystick1Button0, 2, low, KeyCode.K);

		// Set the current Hit Category
		if (conductor.songPosition > lowerGoodBound &&
			conductor.songPosition < upperGoodBound)
		{
			//myLight.color = perfect;
			hc = HitCategory.GOOD;
		}
		else if (conductor.songPosition > lowerWeakBound &&
			conductor.songPosition < upperWeakBound)
		{
			//myLight.color = weak;
			hc = HitCategory.WEAK;
		}
		else
		{
			//myLight.color = fail; ;
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
			midiReader.changePressable();
		}
	}

	// Checks for a hit on a given keycode.
	// Returns true if hit, false if not.
	private void checkHit(KeyCode kc, int trackNumber, SpriteRenderer sprite, KeyCode kb)
	{
		if (Input.GetKeyDown(kc) || Input.GetKeyDown(kb))
		{
			if (midiReader.pressable[trackNumber])
			{
				if (hc == HitCategory.WEAK)
				{
					sfx.sounds[2].pitch = Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));
					sfx.sounds[2].Play();
					particles.startColor = weak;
					ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
					text.text = "Weak";
					ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext, new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6), Quaternion.identity);
					sprite.color = weak;
					Destroy(clone.gameObject, 0.5f);
					Destroy(clone2.gameObject, 1.0f);
					scoreManager.score += 50;
				}
				else if (hc == HitCategory.GOOD)
				{
					sfx.sounds[1].pitch = Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));
					sfx.sounds[1].Play();
					perfect.a = 1f;
					particles.startColor = perfect;
					ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
					text.text = "Perfect";
					ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext, new Vector3(sprite.transform.position.x, sprite.transform.position.y ,-6), Quaternion.identity);
					perfect.a = 0.75f;
					sprite.color = perfect;
					Destroy(clone.gameObject, 0.5f);
					Destroy(clone2.gameObject, 1.0f);
					Debug.Log(scoreManager.score);
					scoreManager.score += 100;
					Debug.Log(scoreManager.score);
				}
			}
			else
			{
				anim.SetTrigger("hurt");
				sfx.sounds[3].pitch = Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));
				sfx.sounds[3].Play();
				particles.startColor = fail;
				ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
				text.text = "Miss";
				ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext, new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6), Quaternion.identity);
				sprite.color = fail;
				Destroy(clone.gameObject, 0.5f);
				Destroy(clone2.gameObject, 1.0f);
				scoreManager.score += -50;
			}
			
			
		}
	}
}
