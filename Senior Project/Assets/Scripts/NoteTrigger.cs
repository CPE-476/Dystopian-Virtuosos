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
	public SFX sfx;

	public ScoreManager scoreManager;

    public float currentBeat;
	public float lowerGoodBound;
	public float lowerWeakBound;
	public float upperGoodBound;
	public float upperWeakBound;
	public float innerThreshold = 0.05f;
	public float outerThreshold = 0.10f;

	[SerializeReference]
	private HitCategory hc;

	Light myLight;
	SpriteRenderer mySprite;

    // Start is called before the first frame update
    void Start()
    {
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

		checkHit(KeyCode.H);
		checkHit(KeyCode.J);
		checkHit(KeyCode.K);
		checkHit(KeyCode.L);

		// Set the next beat when current beat is over.
        if (conductor.songPosition > currentBeat + (conductor.crotchet / 2.0f))
		{
			currentBeat += conductor.crotchet;
			lowerGoodBound = currentBeat - innerThreshold;
			lowerWeakBound = currentBeat - innerThreshold - outerThreshold;
			upperGoodBound = currentBeat + innerThreshold;
			upperWeakBound = currentBeat + innerThreshold + outerThreshold;
			canBePressed = true;
		}

		// metronome
		if (conductor.songPosition > currentBeat + conductor.crotchet)
		{
			sfx.sounds[0].Play();
		}
	}

	// Checks for a hit on a given keycode.
	// Returns true if hit, false if not.
	private bool checkHit(KeyCode kc)
	{
		if (Input.GetKeyDown(kc) && canBePressed)
		{
			if (hc == HitCategory.NONE)
			{
				sfx.sounds[3].Play();
				ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
				mySprite.color = Color.red;
				Destroy(clone.gameObject, 0.5f);
				scoreManager.score += -1;
			}
			else if (hc == HitCategory.WEAK)
			{
				sfx.sounds[2].Play();
				ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
				mySprite.color = Color.blue;
				Destroy(clone.gameObject, 0.5f);
				scoreManager.score += 1;
			}
			else if (hc == HitCategory.GOOD)
			{
				sfx.sounds[1].Play();
				ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
				mySprite.color = Color.green;
				Destroy(clone.gameObject, 0.5f);
				Debug.Log(scoreManager.score);
				scoreManager.score += 2;
				Debug.Log(scoreManager.score);
			}

			canBePressed = false;
			return true;
		}
		return false;
	}

}
