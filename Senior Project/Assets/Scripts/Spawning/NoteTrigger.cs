using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HitCategory
{
    MISS,
    WEAK,
    GOOD
}

public class NoteTrigger : MonoBehaviour
{
    public Spine spine;
    public Conductor conductor;
    public MIDIReader midiReader;
    public PlayerController character;

    ParticleSystem.MainModule psmain;
    public ParticleSystem particles;
    public ParticleSystem hittext;

    public TMPro.TextMeshProUGUI text;

    public SpawnMaster spawnMaster;

    public Animator anim;

    public JoystickControl controllerControl;

    public SpriteRenderer top;
    public SpriteRenderer high;
    public SpriteRenderer low;
    public SpriteRenderer bot;

    public Color perfect_color;
    public Color weak_color;
    public Color fail_color;
    public SFX sfx;

    private bool[] updateHold = { false, false, false, false };
    private bool[] goodHold = { false, false, false, false };
    private ushort[] holdLengths;
    private int holdScore = 1;

    public ScoreManager scoreManager;
    public ComboManager comboManager;

    public float last_spot;
    public float next_spot;
    public float perfect;
    public float good;

    int transpose = 0;

    int[] notes = new int[] { 7, 4, 0, -12 };

    public List<bool[]> hit_notes;

    public int index;

    public bool flickUpLeft = false;

    public bool flickDownLeft = false;

    public bool flickUpRight = false;

    public bool flickDownRight = false;

    public InputActionReference bottomControls, lowControls, topControls, highControls, leftJoystick, rightJoystick;

    void Start()
    {
        holdLengths = new ushort[4];
        psmain = particles.main;

        hit_notes = new List<bool[]>();
        foreach(NoteType[] notes in midiReader.beatmap) {
            bool[] current_spot_hit = new bool[4] {false, false, false, false};

            hit_notes.Add(current_spot_hit);
        }

        Reset();
    }

    public void Reset()
    {
        last_spot = conductor.spotLength;
        next_spot = conductor.spotLength + conductor.spotLength;
        index = 0;

        hit_notes = new List<bool[]>();
        foreach(NoteType[] notes in midiReader.beatmap) {
            bool[] current_spot_hit = new bool[4] {false, false, false, false};

            hit_notes.Add(current_spot_hit);
        }
    }

    void Update()
    {
        if (spine.state != InterfaceState.GAMEPLAY) return;

        checkHit(topControls, 0, top, 0.7f);
        checkHit(highControls, 1, high, 0.7f);
        checkHit(lowControls, 2, low, 0.7f);
        checkHit(bottomControls, 3, bot, 0.7f);

        Debug.Log("leftJoystick VAL: " + leftJoystick.action.ReadValue<float>());
        //Update Hold Note Score
        if (updateHold[0] && holdLengths[0] >= 1 && (topControls.action.ReadValue<float>() > 0.0f || leftJoystick.action.ReadValue<float>() >= 0.7))
        {
            var em = top.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = true;
            if (goodHold[0] == false) holdScore += 1;
            goodHold[0] = true;
        }
        else
        {
            var em = top.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            // if (goodHold[0] == true) holdScore -= 1;
            goodHold[0] = false;
            updateHold[0] = false;
        }
        if (updateHold[1] && holdLengths[1] >= 1 && (highControls.action.ReadValue<float>() > 0.0f || -leftJoystick.action.ReadValue<float>() >= 0.7))
        {
            var em = high.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = true;
            if (goodHold[1] == false) holdScore += 1;
            goodHold[1] = true;
        }
        else
        {
            var em = high.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            // if (goodHold[1] == true) holdScore -= 1;
            goodHold[1] = false;
            updateHold[1] = false;
        }
        if (updateHold[2] && holdLengths[2] >= 1 && (lowControls.action.ReadValue<float>() > 0.0f || rightJoystick.action.ReadValue<float>() >= 0.7))
        {
            var em = low.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = true;
            if (goodHold[2] == false) holdScore += 1;
            goodHold[2] = true;
        }
        else
        {
            var em = low.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            // if (goodHold[2] == true) holdScore -= 1;
            goodHold[2] = false;
            updateHold[2] = false;
        }
        if (updateHold[3] && holdLengths[3] >= 1 && (bottomControls.action.ReadValue<float>() > 0.0f || -rightJoystick.action.ReadValue<float>() >= 0.7))
        {
            var em = bot.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = true;
            if (goodHold[3] == false) holdScore += 1;
            goodHold[3] = true;
        }
        else
        {
            var em = bot.gameObject.transform.Find("HitSpot/HoldParticle").GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            // if (goodHold[3] == true) holdScore -= 1;
            goodHold[3] = false;
            updateHold[3] = false;
        }

        if(midiReader.index == 0)
        {
            index = midiReader.index;
            for(int i = 0; i < 4; i++)
            {
                goodHold[i] = false;
                updateHold[i] = false;
            }
        }

        if (goodHold.Any(b => b))
        {
            int newIndex = midiReader.index;

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

        // Set the next beat when current beat is over.
        if (conductor.GetSongPosition() > next_spot)
        {
            last_spot += conductor.spotLength;
            next_spot += conductor.spotLength;

            // BELOW: Consequences for missing things
            switch (midiReader.beatmap[midiReader.index][0])
            {
                case NoteType.NOTE:
                    {
                        if (!hit_notes[midiReader.index][0]) ResolveMiss(top, 0);
                    }
                    break;
                case NoteType.OBSTACLE:
                    {
                        if (character.current_track_position == 3)
                            ResolveHitObstacle(top, 0);
                    }
                    break;
                case NoteType.HOLD:
                    {
                        if (!hit_notes[midiReader.index][0]) ResolveMiss(top, 0);
                    }
                    break;
            }
            switch (midiReader.beatmap[midiReader.index][1])
            {
                case NoteType.NOTE:
                    {
                        if (!hit_notes[midiReader.index][1]) ResolveMiss(high, 1);
                    }
                    break;
                case NoteType.OBSTACLE:
                    {
                        if (character.current_track_position == 2)
                            ResolveHitObstacle(high, 1);
                    }
                    break;
                case NoteType.HOLD:
                    {
                        if (!hit_notes[midiReader.index][1]) ResolveMiss(high, 1);
                    }
                    break;
            }
            switch (midiReader.beatmap[midiReader.index][2])
            {
                case NoteType.NOTE:
                    {
                        if (!hit_notes[midiReader.index][2]) ResolveMiss(low, 2);
                    }
                    break;
                case NoteType.OBSTACLE:
                    {
                        if (character.current_track_position == 1)
                            ResolveHitObstacle(low, 2);
                    }
                    break;
                case NoteType.HOLD:
                    {
                        if (!hit_notes[midiReader.index][2]) ResolveMiss(low, 2);
                    }
                    break;
            }
            switch (midiReader.beatmap[midiReader.index][3])
            {
                case NoteType.NOTE:
                    {
                        if (!hit_notes[midiReader.index][3]) ResolveMiss(bot, 3);
                    }
                    break;
                case NoteType.OBSTACLE:
                    {
                        if (character.current_track_position == 0)
                            ResolveHitObstacle(bot, 3);
                    }
                    break;
                case NoteType.HOLD:
                    {
                        if (!hit_notes[midiReader.index][3]) ResolveMiss(bot, 3);
                    }
                    break;
            }

            midiReader.updateTrackState();
            spawnMaster.SpawnNotes();
        }
    }

    private void ResolveHit(SpriteRenderer sprite, int trackNumber, HitCategory hitCategory, bool isHold)
    {
        if (hitCategory == HitCategory.WEAK && isHold)
        {       
            updateHold[trackNumber] = true;
            holdScore = 1;
            holdLengths[trackNumber] = spawnMaster.lengths[trackNumber];
        }
        else if (hitCategory == HitCategory.GOOD && isHold)
        {
            updateHold[trackNumber] = true;
            holdScore = 2;
            holdLengths[trackNumber] = spawnMaster.lengths[trackNumber];
        }

        if (hitCategory == HitCategory.WEAK)
        {
            sfx.sounds[1].pitch =
                Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));

            //sfx.sounds[1].Play();
            psmain.startColor = weak_color;
            ParticleSystem clone = (ParticleSystem)Instantiate(particles,
                new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6),
                Quaternion.identity);
            text.text = "GOOD";
            ParticleSystem clone2 =(ParticleSystem)Instantiate(hittext,
                new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6),
                Quaternion.identity);
            sprite.color = weak_color;
            Destroy(clone.gameObject, 0.5f);
            Destroy(clone2.gameObject, 1.0f);
            scoreManager.score += 1;
        }
        else if (hitCategory == HitCategory.GOOD)
        {
            sfx.sounds[1].pitch = Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));

            //sfx.sounds[1].Play();
            perfect_color.a = 1f;
            psmain.startColor = perfect_color;
            ParticleSystem clone = (ParticleSystem)Instantiate(particles,
                new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6),
                Quaternion.identity);
            text.text = "PERFECT";
            ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext,
                new Vector3(sprite.transform.position.x,
                sprite.transform.position.y, -6),
                Quaternion.identity);
            perfect_color.a = 0.75f;
            sprite.color = perfect_color;
            Destroy(clone.gameObject, 0.5f);
            Destroy(clone2.gameObject, 1.0f);
            scoreManager.score += 2;
        }
        else if (hitCategory == HitCategory.MISS)
        {
            ResolveMiss(sprite, trackNumber);
            return;
        }

        // Note: this only applies if you don't miss. See return above.
        comboManager.comboNumber++;
    }

    private void ResolveMiss(SpriteRenderer sprite, int trackNumber)
    {
        anim.SetTrigger("hurt");
        comboManager.comboNumber = 0;
        character.curHealth -= 2;
        character.hb.setHealth(character.curHealth);

        // TODO (Alex): Should a miss incur a sound effect?
        sfx.sounds[3].pitch = Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));

        //sfx.sounds[3].Play();
        psmain.startColor = fail_color;
        ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
        text.text = "MISS";
        ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext,
            new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6),
            Quaternion.identity);
        sprite.color = fail_color;
        Destroy(clone.gameObject, 0.5f);
        Destroy(clone2.gameObject, 1.0f);
    }

    private void ResolveHitObstacle(SpriteRenderer sprite, int trackNumber)
    {
        anim.SetTrigger("hurt");

        character.curHealth -= 5;
        character.hb.setHealth(character.curHealth);

        sfx.sounds[3].pitch =
            Mathf.Pow(2, (float)((notes[trackNumber] + transpose) / 12.0));

        //sfx.sounds[3].Play();
        psmain.startColor = fail_color;
        ParticleSystem clone = (ParticleSystem)Instantiate(particles, sprite.transform.position, Quaternion.identity);
        text.text = "OUCH";
        ParticleSystem clone2 = (ParticleSystem)Instantiate(hittext,
            new Vector3(sprite.transform.position.x, sprite.transform.position.y, -6),
            Quaternion.identity);
        sprite.color = fail_color;
        Destroy(clone.gameObject, 0.5f);
        Destroy(clone2.gameObject, 1.0f);
    }

    // Checks for a hit on a given keycode.
    // Returns true if hit, false if not.
    private void checkHit(InputActionReference controls, int trackNumber, SpriteRenderer sprite, float pianoThreshold)
    {
        float joystickAxis = 0.0f;

        bool pianoBool = true;
        if (trackNumber == 0)
        {
            pianoBool = flickUpLeft;
            joystickAxis = leftJoystick.action.ReadValue<float>();
        }
        else if (trackNumber == 1)
        {
            pianoBool = flickDownLeft;
            joystickAxis = -leftJoystick.action.ReadValue<float>();
        }
        else if (trackNumber == 2)
        {
            pianoBool = flickUpRight;
            joystickAxis = rightJoystick.action.ReadValue<float>();
        }
        else if (trackNumber == 3)
        {
            pianoBool = flickDownRight;
            joystickAxis = -rightJoystick.action.ReadValue<float>();
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
            else if (trackNumber == 3) flickDownRight = false;
        }
        if (controls.action.WasPressedThisFrame() || (joystickAxis >= pianoThreshold && !pianoBool))
        {
            pianoBool = true;
            if (trackNumber == 0)
                flickUpLeft = true;
            else if (trackNumber == 1)
                flickDownLeft = true;
            else if (trackNumber == 2)
                flickUpRight = true;
            else if (trackNumber == 3) flickDownRight = true;

            int [] to_check;
            double song_position = conductor.GetSongPosition();
            double since_last_spot = song_position - last_spot;
            double till_next_spot = next_spot - song_position;
            if(since_last_spot < till_next_spot) {
                // -2 -1  1  2
                //  3  1  2  4
                to_check = new int [] {0, 1, -1, 2};
            }
            else {
                // -2 -1  1  2
                //  4  2  1  3
                to_check = new int [] {1, 0, 2, -1};
            }

            // TODO: Guard against multiple presses
            foreach(int i in to_check) {
                int index = midiReader.index + i;
                if(i < 0 || i > midiReader.beatmap.Count()) continue; // Array bounds-checking

                float time_away_from_this_hit = 0.0f;
                if(i == 0) time_away_from_this_hit = (float)since_last_spot;
                else if(i == 1) time_away_from_this_hit = (float)till_next_spot;
                else if(i == -1) time_away_from_this_hit = (float)since_last_spot + conductor.spotLength;
                else if(i == 2) time_away_from_this_hit = (float)till_next_spot + conductor.spotLength;
                else {
                    Debug.Assert(false, "Should not get here!\n");
                }
                HitCategory hit_category;
                if(time_away_from_this_hit < perfect * conductor.spotLength) {
                    hit_category = HitCategory.GOOD;
                }
                else if(time_away_from_this_hit < good * conductor.spotLength) {
                    hit_category = HitCategory.WEAK;
                }
                else {
                    // never get here
                    hit_category = HitCategory.MISS;
                }

                switch (midiReader.beatmap[index][trackNumber])
                {
                    case NoteType.NOTE:
                    {
                        if(hit_category != HitCategory.MISS && !hit_notes[index][trackNumber]) {
                            ResolveHit(sprite, trackNumber, hit_category, false);
                            hit_notes[index][trackNumber] = true;
                        }
                    } break;
                    case NoteType.HOLD:
                    {
                        if(hit_category != HitCategory.MISS && !hit_notes[index][trackNumber]) {
                            ResolveHit(sprite, trackNumber, hit_category, true);
                            hit_notes[index][trackNumber] = true;
                        }
                    } break;
                }
            }
        }
    }
}
