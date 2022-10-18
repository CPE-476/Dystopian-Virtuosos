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
	public TrackMaster trackMaster;
	public ParticleSystem particles;

	public SpriteRenderer top;
	public SpriteRenderer high;
	public SpriteRenderer low;
	public SpriteRenderer bot;

	public SFX sfx;

	public ScoreManager scoreManager;

    public float currentSpot;
	public float lowerGoodBound;
	public float lowerWeakBound;
	public float upperGoodBound;
	public float upperWeakBound;
	public float innerThreshold;
	public float outerThreshold;

	[SerializeReference]
	private HitCategory hc;

	Light myLight;

    // Start is called before the first frame update
    void Start()
    {
		innerThreshold = 0.025f;
		outerThreshold = 0.05f;
        currentSpot = 0.0f;
		myLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
		for(int curTrack = 0; curTrack < TrackMaster.NUM_TRACKS; ++curTrack)
		{
			checkHit(KeyCode.H, 0, top);
			checkHit(KeyCode.J, 1, high);
			checkHit(KeyCode.K, 2, low);
			checkHit(KeyCode.L, 3, bot);
		}

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
		if (conductor.songPosition > currentSpot + (conductor.spotLength / 2.0f))
		{
			currentSpot += conductor.spotLength;
			lowerGoodBound = currentSpot - innerThreshold;
			lowerWeakBound = currentSpot - innerThreshold - outerThreshold;
			upperGoodBound = currentSpot + innerThreshold;
			upperWeakBound = currentSpot + innerThreshold + outerThreshold;
		}

		// metronome
		if (conductor.songPosition > currentSpot + conductor.spotLength)
		{
			sfx.sounds[0].Play();
		}
	}

	// Checks for a hit on a given keycode.
	// Returns true if hit, false if not.
	private void checkHit(KeyCode kc, int trackNumber, SpriteRenderer sprite)
	{
		if (Input.GetKeyDown(kc))
		{
			if(trackMaster.pressable[trackNumber])
			{
				if (hc == HitCategory.WEAK)
				{
					sfx.sounds[2].Play();
					ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
					sprite.color = Color.blue;
					Destroy(clone.gameObject, 0.5f);
					scoreManager.score += 1;
				}
				else if (hc == HitCategory.GOOD)
				{
					sfx.sounds[1].Play();
					ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
					sprite.color = Color.green;
					Destroy(clone.gameObject, 0.5f);
					Debug.Log(scoreManager.score);
					scoreManager.score += 2;
					Debug.Log(scoreManager.score);
				}
			}
			else
			{
				sfx.sounds[3].Play();
				ParticleSystem clone = (ParticleSystem)Instantiate(particles, transform.position, Quaternion.identity);
				sprite.color = Color.red;
				Destroy(clone.gameObject, 0.5f);
				scoreManager.score += -1;
			}
		}
	}
}
