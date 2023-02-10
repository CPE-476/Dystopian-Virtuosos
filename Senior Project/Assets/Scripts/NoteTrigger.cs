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

	bool[] hasBeenPressed = { false, false, false, false };

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
		if (checkHit(KeyCode.Joystick1Button3, 0, top, KeyCode.H))
			hasBeenPressed[0] = true;

		if(checkHit(KeyCode.Joystick1Button2, 1, high, KeyCode.J))
			hasBeenPressed[1] = true;

		if(checkHit(KeyCode.Joystick1Button0, 2, low, KeyCode.K))
			hasBeenPressed[2] = true;


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
			// BELOW: Consequences for missing things
			switch(midiReader.pressable[0])
			{
				case NoteType.NOTE:
				{
					if (!hasBeenPressed[0])
					{
						ResolveMiss(top, 0);
					}
				} break;
				case NoteType.OBSTACLE:
				{
					if (character.playerState == 2)
					{
							Debug.Log("Here1");
						ResolveHitObstacle(top, 0);
					}
				} break;
			}
			switch(midiReader.pressable[1])
			{
				case NoteType.NOTE:
				{
					if (!hasBeenPressed[1])
					{
						ResolveMiss(high, 1);
					}
				} break;
				case NoteType.OBSTACLE:
				{
					if (character.playerState == 1)
					{
							Debug.Log("Here2");
							ResolveHitObstacle(high, 1);
					}
				} break;
			}
			switch(midiReader.pressable[2])
			{
				case NoteType.NOTE:
				{
					if (!hasBeenPressed[2])
					{
						ResolveMiss(low, 2);
					}
				} break;
				case NoteType.OBSTACLE:
				{
					if (character.playerState == 0)
					{
							Debug.Log("Here3");
							ResolveHitObstacle(low, 2);
					}
				} break;
			}

			hasBeenPressed[0] = false;
			hasBeenPressed[1] = false;
			hasBeenPressed[2] = false;
			hasBeenPressed[3] = false;
			midiReader.changePressable();
		}
	}

	private void ResolveHit(HitCategory hc, SpriteRenderer sprite, int trackNumber)
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
			scoreManager.score += 1;
		}
		else if (hc == HitCategory.GOOD)
		{
			sfx.sounds[1].pitch = Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));
			sfx.sounds[1].Play();
			perfect.a = 1f;
			particles.startColor = perfect;
			ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
			text.text = "Perfect";
			ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext, new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6), Quaternion.identity);
			perfect.a = 0.75f;
			sprite.color = perfect;
			Destroy(clone.gameObject, 0.5f);
			Destroy(clone2.gameObject, 1.0f);
			scoreManager.score += 2;
		}
	}

	private void ResolveMiss(SpriteRenderer sprite, int trackNumber)
	{
		anim.SetTrigger("hurt");
		// TODO (Alex): Should a miss incur a sound effect?
		//sfx.sounds[3].pitch = Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));
		//sfx.sounds[3].Play();
		particles.startColor = fail;
		ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
		text.text = "Miss";
		ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext, new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6), Quaternion.identity);
		sprite.color = fail;
		Destroy(clone.gameObject, 0.5f);
		Destroy(clone2.gameObject, 1.0f);
		scoreManager.score -= 2;
	}
	
	private void ResolveHitObstacle(SpriteRenderer sprite, int trackNumber)
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
		scoreManager.score -= 2;
	}

	// Checks for a hit on a given keycode.
	// Returns true if hit, false if not.
	private bool checkHit(KeyCode kc, int trackNumber, SpriteRenderer sprite, KeyCode kb)
	{
		if (Input.GetKeyDown(kc) || Input.GetKeyDown(kb))
		{
			if (midiReader.pressable[trackNumber] == NoteType.NOTE)
			{
				ResolveHit(hc, sprite, trackNumber);
				return true;
			}
			else if (midiReader.pressable[trackNumber] == NoteType.OBSTACLE)
			{
				/* TODO */
				return true;
			}
			else if (midiReader.pressable[trackNumber] == NoteType.COLLECTIBLE)
			{
				/* TODO */
				return true;
			}
			else
			{
				Debug.Log("Unimplemented NoteType in NoteTrigger.CheckHit()");
				//ResolveMiss(sprite, trackNumber);
			}
		}
		return false;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Obstacle")
        {
			Debug.Log("enter collision");
			ResolveHitObstacle(low, 2);

		}
    }
}
