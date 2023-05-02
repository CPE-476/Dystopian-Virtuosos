using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum HitCategory
{
    NONE,
    WEAK,
    GOOD
}

public class NoteTrigger : MonoBehaviour
{
    public Conductor conductor;
    public MIDIReader midiReader;

    public ParticleSystem particles;
    ParticleSystem.MainModule psmain;

    public ParticleSystem hittext;

    public PlayerController character;

    public TMPro.TextMeshProUGUI text;

    public SpawnMaster spawnMaster;

    public Animator anim;

    public SpriteRenderer top;
    public SpriteRenderer high;
    public SpriteRenderer low;
    public SpriteRenderer bot;

    public Color perfect;
    public Color weak;
    public Color fail;

    public SFX sfx;

    private bool[] updateHold = { false, false, false, false };
    private bool[] goodHold = { false, false, false, false };
    private ushort[] holdLengths;
    private int holdScore = 1;

    public ScoreManager scoreManager;
    public ComboManager comboManager;

    public float currentSpot;

    public float noteEnd;

    public float innerThreshold;
    public float outerThreshold;

    private float lowerGoodBound;
    private float lowerWeakBound;
    private float upperGoodBound;
    private float upperWeakBound;

    int transpose = 0;

    int[] notes = new int[] { 7, 4, 0, -12 };

    [SerializeReference]
    private HitCategory current_hit_category;

    bool[] hasBeenPressed = { false, false, false, false };

    public int index;
    public int newIndex;

    public bool flickUpLeft = false;
    public bool flickDownLeft = false;
    public bool flickUpRight = false;
    public bool flickDownRight = false;

    // Start is called before the first frame update
    void Start()
    {
        holdLengths = new ushort[4];
        currentSpot = conductor.spotLength;
        noteEnd = conductor.spotLength + conductor.spotLength * 0.5f;
        hasBeenPressed = new bool[] { false, false, false, false };
        psmain = particles.main;
        index = midiReader.index;
        newIndex = index;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkHit(KeyCode.Joystick1Button3, KeyCode.H, 0, top, 0.7f))
            hasBeenPressed[0] = true;

        if (checkHit(KeyCode.Joystick1Button2, KeyCode.J, 1, high, 0.7f))
            hasBeenPressed[1] = true;

        if (checkHit(KeyCode.Joystick1Button0, KeyCode.K, 2, low, 0.7f))
            hasBeenPressed[2] = true;

        if (checkHit(KeyCode.Joystick1Button1, KeyCode.L, 3, bot, 0.7f))
            hasBeenPressed[3] = true;


        //Update Hold Note Score 
        if (updateHold[0] && holdLengths[0] >= 1 && ((Input.GetKey(KeyCode.H) || Input.GetKey(KeyCode.Joystick1Button3) || Input.GetAxis("Vertical") >= 0.7f)))
        {
            var em = top.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = true;
            if (goodHold[0] == false)
                holdScore += 1;
            goodHold[0] = true;
            
        }
        else
        {
            var em = top.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            if (goodHold[0] == true)
                holdScore -= 1;
            goodHold[0] = false;
            updateHold[0] = false;
        }
        if (updateHold[1] && holdLengths[1] >= 1 && ((Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.Joystick1Button2) || -Input.GetAxis("Vertical") >= 0.7f)))
        {
            var em = high.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = true;
            if (goodHold[1] == false)
                holdScore += 1;
            goodHold[1] = true;
        }
        else
        {
            var em = high.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            if (goodHold[1] == true)
                holdScore -= 1;
            goodHold[1] = false;
            updateHold[1] = false;
        }
        if (updateHold[2] && holdLengths[2] >= 1 && ((Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.Joystick1Button0) || Input.GetAxis("VerticalRight") >= 0.7f)))
        {
            var em = low.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = true;
            if (goodHold[2] == false)
                holdScore += 1;
            goodHold[2] = true;
        }
        else
        {
            var em = low.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            if (goodHold[2] == true)
                holdScore -= 1;
            goodHold[2] = false;
            updateHold[2] = false;
        }
        if (updateHold[3] && holdLengths[3] >= 1 && ((Input.GetKey(KeyCode.L) || -Input.GetAxis("VerticalRight") >= 0.7f)))
        {
            var em = bot.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = true;
            if (goodHold[3] == false)
                holdScore += 1;
            goodHold[3] = true;
        }
        else
        {
            var em = bot.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            if (goodHold[3] == true)
                holdScore -= 1;
            goodHold[3] = false;
            updateHold[3] = false;
        }


        if (goodHold.Any(b => b))
        {
            newIndex = midiReader.index;

            if (newIndex > index)
            {
                holdLengths[0]--;
                holdLengths[1]--;
                holdLengths[2]--;
                holdLengths[3]--;
                scoreManager.score += holdScore;
                index = newIndex;
            }
        }

        // Set the current Hit Category
        if (
            conductor.songPosition >= lowerGoodBound &&
            conductor.songPosition <= upperGoodBound
        )
        {
            current_hit_category = HitCategory.GOOD;
        }
        else if (
            conductor.songPosition >= lowerWeakBound &&
            conductor.songPosition <= upperWeakBound
        )
        {
            current_hit_category = HitCategory.WEAK;
        }
        else
        {
            current_hit_category = HitCategory.NONE;
        }

        // Set the next beat when current beat is over.
        if (conductor.songPosition > noteEnd)
        {
            currentSpot += conductor.spotLength;
            noteEnd += conductor.spotLength;

            lowerGoodBound = currentSpot - innerThreshold * conductor.spotLength;
            upperGoodBound = currentSpot + innerThreshold * conductor.spotLength;

            lowerWeakBound = currentSpot - outerThreshold;
            upperWeakBound = currentSpot + outerThreshold;

            // BELOW: Consequences for missing things
            switch (midiReader.track_state[0])
            {
                case NoteType.NOTE:
                {
                    if (!hasBeenPressed[0])
                        ResolveMiss(top, 0);
                } break;
                case NoteType.OBSTACLE:
                {
                    if (character.playerState == 3)
                        ResolveHitObstacle(top, 0);
                } break;
                case NoteType.HOLD:
                {
                    if (!hasBeenPressed[0])
                        ResolveMiss(top, 0);
                } break;
            }
            switch (midiReader.track_state[1])
            {
                case NoteType.NOTE:
                {
                    if (!hasBeenPressed[1])
                        ResolveMiss(high, 1);
                } break;
                case NoteType.OBSTACLE:
                {
                    if (character.playerState == 2)
                        ResolveHitObstacle(high, 1);
                } break;
            }
            switch (midiReader.track_state[2])
            {
                case NoteType.NOTE:
                {
                    if (!hasBeenPressed[2])
                        ResolveMiss(low, 2);
                } break;
                case NoteType.OBSTACLE:
                {
                    if (character.playerState == 1)
                        ResolveHitObstacle(low, 2);
                } break;
            }
            switch (midiReader.track_state[3])
            {
                case NoteType.NOTE:
                {
                    if (!hasBeenPressed[3])
                        ResolveMiss(bot, 3);
                } break;
                case NoteType.OBSTACLE:
                {
                    if (character.playerState == 0)
                        ResolveHitObstacle(bot, 3);
                } break;
            }

            hasBeenPressed[0] = false;
            hasBeenPressed[1] = false;
            hasBeenPressed[2] = false;
            hasBeenPressed[3] = false;
            midiReader.updateTrackState();
        }
    }

    private void ResolveHit(SpriteRenderer sprite, int trackNumber, bool isHold)
    {

        Debug.Log("Resolve Hit");
        if (current_hit_category == HitCategory.WEAK  && isHold)
        {
            updateHold[trackNumber] = true;
            holdScore = 1;
            holdLengths[trackNumber] = spawnMaster.lengths[trackNumber];
        }
        else if (current_hit_category == HitCategory.GOOD && isHold)
        {
            updateHold[trackNumber] = true;
            holdScore = 2;
            holdLengths[trackNumber] = spawnMaster.lengths[trackNumber];
        }

        if (current_hit_category == HitCategory.WEAK)
        {
            sfx.sounds[1].pitch =
                Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));
            sfx.sounds[1].Play();
            psmain.startColor = weak;
            ParticleSystem clone =
                (ParticleSystem)
                Instantiate(particles,
                sprite.transform.position,
                Quaternion.identity);
            text.text = "GOOD";
            ParticleSystem clone2 =
                (ParticleSystem)
                Instantiate(hittext,
                new Vector3(sprite.transform.position.x,
                    sprite.transform.position.y,
                    -6),
                Quaternion.identity);
            sprite.color = weak;
            Destroy(clone.gameObject, 0.5f);
            Destroy(clone2.gameObject, 1.0f);
            scoreManager.score += 1;
        }
        else if (current_hit_category == HitCategory.GOOD)
        {
            sfx.sounds[1].pitch =
                Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));
            sfx.sounds[1].Play();
            perfect.a = 1f;
            psmain.startColor = perfect;
            ParticleSystem clone =
                (ParticleSystem)
                Instantiate(particles,
                sprite.transform.position,
                Quaternion.identity);
            text.text = "PERFECT";
            ParticleSystem clone2 =
                (ParticleSystem)
                Instantiate(hittext,
                new Vector3(sprite.transform.position.x,
                    sprite.transform.position.y,
                    -6),
                Quaternion.identity);
            perfect.a = 0.75f;
            sprite.color = perfect;
            Destroy(clone.gameObject, 0.5f);
            Destroy(clone2.gameObject, 1.0f);
            scoreManager.score += 2;
        }
        comboManager.comboNumber++;
    }

    private void ResolveMiss(SpriteRenderer sprite, int trackNumber)
    {
        anim.SetTrigger("hurt");
        comboManager.comboNumber = 0;
        character.curHealth -= 10;
        character.hb.setHealth(character.curHealth);

        // TODO (Alex): Should a miss incur a sound effect?
        sfx.sounds[3].pitch = Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));
        sfx.sounds[3].Play();
        psmain.startColor = fail;
        ParticleSystem clone =
            (ParticleSystem)
            Instantiate(particles,
            sprite.transform.position,
            Quaternion.identity);
        text.text = "MISS";
        ParticleSystem clone2 =
            (ParticleSystem)
            Instantiate(hittext,
            new Vector3(sprite.transform.position.x,
                sprite.transform.position.y,
                -6),
            Quaternion.identity);
        sprite.color = fail;
        Destroy(clone.gameObject, 0.5f);
        Destroy(clone2.gameObject, 1.0f);
        scoreManager.score -= 2;
    }

    private void ResolveHitObstacle(SpriteRenderer sprite, int trackNumber)
    {
        anim.SetTrigger("hurt");

        character.curHealth -= 10;
        character.hb.setHealth(character.curHealth);

        sfx.sounds[3].pitch =
            Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));
        sfx.sounds[3].Play();
        psmain.startColor = fail;
        ParticleSystem clone =
            (ParticleSystem)
            Instantiate(particles,
            sprite.transform.position,
            Quaternion.identity);
        text.text = "MISS";
        ParticleSystem clone2 =
            (ParticleSystem)
            Instantiate(hittext,
            new Vector3(sprite.transform.position.x,
                sprite.transform.position.y,
                -6),
            Quaternion.identity);
        sprite.color = fail;
        Destroy(clone.gameObject, 0.5f);
        Destroy(clone2.gameObject, 1.0f);
        scoreManager.score -= 2;
    }

    // Checks for a hit on a given keycode.
    // Returns true if hit, false if not.
    private bool
    checkHit(KeyCode kc, KeyCode kb, int trackNumber, SpriteRenderer sprite, float pianoThreshold)
    {
        float joystickAxis = 0.0f;

        bool pianoBool = true;
        if (trackNumber == 0)
        {
            pianoBool = flickUpLeft;
            joystickAxis = Input.GetAxis("Vertical");
        }
        else if (trackNumber == 1)
        {
            pianoBool = flickDownLeft;
            joystickAxis = -Input.GetAxis("Vertical");
        }
        else if (trackNumber == 2)
        {
            pianoBool = flickUpRight;
            joystickAxis = Input.GetAxis("VerticalRight");
        }
        else if (trackNumber == 3)
        {
            pianoBool = flickDownRight;
            joystickAxis = -Input.GetAxis("VerticalRight");
        }

        if (joystickAxis <= pianoThreshold)
        {
            pianoBool = false;
            if (trackNumber == 0)
                 flickUpLeft = false;
            else if (trackNumber == 1)
                flickDownLeft = false;
            else if (trackNumber == 2)
                flickUpRight = false;
            else if (trackNumber == 3)
                flickDownRight = false;
        }
        if (Input.GetKeyDown(kc) || Input.GetKeyDown(kb) || (joystickAxis >= pianoThreshold && !pianoBool))
        {
            pianoBool = true;
            if (trackNumber == 0)
                flickUpLeft = true;
            else if (trackNumber == 1)
                flickDownLeft = true;
            else if (trackNumber == 2)
                flickUpRight = true;
            else if (trackNumber == 3)
                flickDownRight = true;
            //Debug.Log("Track States: " + midiReader.track_state[trackNumber]);
            switch (midiReader.track_state[trackNumber])
            {
                case NoteType.NOTE:
                {
                        Debug.Log("Note");
                        ResolveHit (sprite, trackNumber, false);
                    return true;
                }
                case NoteType.HOLD:
                {
                        Debug.Log("Hold");
                        ResolveHit(sprite, trackNumber, true);
                    return true;
                }
                case NoteType.OBSTACLE:
                {
                    return true;
                }
                case NoteType.COLLECTIBLE:
                {
                    return true;
                }
                case NoteType.EMPTY:
                {
                    return false;
                }
            }
        }
        return false;
    }
}
