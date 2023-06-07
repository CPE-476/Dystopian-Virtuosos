using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Spine spine;

    public Conductor conductor;

    public MIDIReader midiReader;

    public NoteTrigger noteTrigger;

    public TracksController tracksController;

    public Dialogue dialogue;

    public Tutorial tutorial;

    public GameObject collector;

    public JoystickControl controllerControl;

    public float movePower = 2f;

    public float jumpPower = 5f; //Set Gravity Scale in Rigidbody2D Component to 5

    public int maxHealth;

    public int curHealth;

    public HealthBar hb;

    private Rigidbody2D rb;

    public Animator anim;

    public Animator anim2;

    public Animator anim3;

    public GameObject guitarist;
    public GameObject pianist;

    public float drumXVal = 0;
    public float guitarXval = -5;
    public float pianoXval = -5;

    bool isJumping = false;

    public float startX;

    public bool reset = false;

    public uint current_track_position;

    public float playerHeightOffset = 1.2f;

    private double lastNoteHitTime;

    public InputActionReference bottom, low, top, high, interact;

    public float fallSpeed = 1f;
    private float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startX = rb.transform.position.x;
        reset = true;
        curHealth = maxHealth;
        hb.setMaxHealth (maxHealth);
    }

    private void Update()
    {
        transform.position = new Vector3(drumXVal, transform.position.y, transform.position.z);
        pianist.transform.position = new Vector3(pianoXval, -0.45f, 0);
        guitarist.transform.position = new Vector3(guitarXval, guitarist.transform.position.y, guitarist.transform.position.z);
        switch (spine.state)
        {
            case InterfaceState.GAMEPLAY:
                {
                    anim.SetBool("isRun", true);
                    Attack();
                    Jump();
                    Run();
                    if (Input.GetKeyDown(KeyCode.P)) spine.DialogueStart();
                }
                break;
            case InterfaceState.DIALOGUE:
                {
                    anim.SetBool("isRun", false);
                    if (interact.action.WasPressedThisFrame())
                    {
                        int result = dialogue.NextLine();
                        if (result == -1) spine.TutorialStart();
                    }
                }
                break;
            case InterfaceState.TUTORIAL:
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Joystick1Button5))
                    {
                        tutorial.NextVideo();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Joystick1Button4))
                    {
                        tutorial.PrevVideo();
                    }
                    else if (interact.action.WasPressedThisFrame())
                    {
                        spine.GameplayStart();
                    }
                }
                break;
            case InterfaceState.GAME_OVER:
                {
                }
                break;
        }
    }

    public void Die()
    {
        Debug.Log("YOU DIED!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        anim.SetBool("isJump", false);
    }

    void Run()
    {
        Vector3 moveVelocity = Vector3.right;
        if (!anim.GetBool("isJump")) anim.SetBool("isRun", true);
    }

    void Jump()
    {
        if (
            (Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0) &&
            !anim.GetBool("isJump")
        )
        {
            isJumping = true;
            anim.SetBool("isJump", true);
        }
        if (!isJumping)
        {
            return;
        }

        rb.velocity = Vector2.zero;

        //Vector2 jumpVelocity = new Vector2(0, jumpPower);
        //rb.AddForce(jumpVelocity, ForceMode2D.Impulse);
        isJumping = false;
    }

    void Attack()
    {
        uint prev_track_pos = current_track_position;
        if (top.action.WasPressedThisFrame())
        {
            transform.position = new Vector3(drumXVal, tracksController.Track1.transform.position.y - playerHeightOffset, 0);
            guitarist.transform.position = new Vector3(guitarXval, tracksController.Track1.transform.position.y - playerHeightOffset, 0);
            collector.transform.position = new Vector3(collector.transform.position.x, tracksController.Track1.transform.position.y, collector.transform.position.z);
            current_track_position = 3;
            lastNoteHitTime = conductor.GetSongPosition();

            if (prev_track_pos == current_track_position)
            {
                anim.SetTrigger("kickAttack");
                anim2.SetTrigger("attack2");
                anim3.SetTrigger("attack");
            }
            else if (prev_track_pos < current_track_position)
            {
                anim.SetTrigger("attack");
                anim2.SetTrigger("attack1");
                anim3.SetTrigger("attack");
            }
            else if (prev_track_pos > current_track_position)
            {
                anim.SetTrigger("downAttack");
                anim2.SetTrigger("attack1");
                anim3.SetTrigger("attack");
            }
        }
        if (high.action.WasPressedThisFrame())
        {
            transform.position = new Vector3(drumXVal, tracksController.Track2.transform.position.y - playerHeightOffset, 0);
            guitarist.transform.position = new Vector3(guitarXval, tracksController.Track2.transform.position.y - playerHeightOffset, 0);
            collector.transform.position = new Vector3(collector.transform.position.x, tracksController.Track2.transform.position.y, collector.transform.position.z);
            current_track_position = 2;
            lastNoteHitTime = conductor.GetSongPosition();

            if (prev_track_pos == current_track_position)
            {
                anim.SetTrigger("kickAttack");
                anim2.SetTrigger("attack2");
                anim3.SetTrigger("attack");
            }
            else if (prev_track_pos < current_track_position)
            {
                anim.SetTrigger("attack");
                anim2.SetTrigger("attack1");
                anim3.SetTrigger("attack");
            }
            else if (prev_track_pos > current_track_position)
            {
                anim.SetTrigger("downAttack");
                anim2.SetTrigger("attack1");
                anim3.SetTrigger("attack");
            }
        }
        if (low.action.WasPressedThisFrame())
        {
            transform.position = new Vector3(drumXVal, tracksController.Track3.transform.position.y - playerHeightOffset, 0);
            guitarist.transform.position = new Vector3(guitarXval, tracksController.Track3.transform.position.y - playerHeightOffset, 0);
            collector.transform.position =
                new Vector3(collector.transform.position.x,
                    tracksController.Track3.transform.position.y,
                    collector.transform.position.z);
            current_track_position = 1;
            lastNoteHitTime = conductor.GetSongPosition();

            if (prev_track_pos == current_track_position)
            {
                anim.SetTrigger("kickAttack");
                anim2.SetTrigger("attack2");
                anim3.SetTrigger("attack");
            }
            else if (prev_track_pos < current_track_position)
            {
                anim.SetTrigger("attack");
                anim2.SetTrigger("attack1");
                anim3.SetTrigger("attack");
            }
            else if (prev_track_pos > current_track_position)
            {
                anim.SetTrigger("downAttack");
                anim2.SetTrigger("attack1");
                anim3.SetTrigger("attack");
            }
        }
        if (bottom.action.WasPressedThisFrame())
        {
            transform.position = new Vector3(drumXVal, tracksController.Track4.transform.position.y - playerHeightOffset, 0);
            guitarist.transform.position = new Vector3(guitarXval, tracksController.Track4.transform.position.y - playerHeightOffset, 0);
            collector.transform.position =
                new Vector3(collector.transform.position.x,
                    tracksController.Track4.transform.position.y,
                    collector.transform.position.z);
            current_track_position = 0;
            lastNoteHitTime = conductor.GetSongPosition();

            if (prev_track_pos == current_track_position)
            {
                anim.SetTrigger("kickAttack");
                anim2.SetTrigger("attack2");
                anim3.SetTrigger("attack");
            }
            else if (prev_track_pos < current_track_position)
            {
                anim.SetTrigger("attack");
                anim2.SetTrigger("attack1");
                anim3.SetTrigger("attack");
            }
            else if (prev_track_pos > current_track_position)
            {
                anim.SetTrigger("downAttack");
                anim2.SetTrigger("attack1");
                anim3.SetTrigger("attack");
            }
        }

        if (
            conductor.GetSongPosition() - (conductor.spotLength * 4) >=
            lastNoteHitTime &&
            !top.action.WasPressedThisFrame() &&
            !high.action.WasPressedThisFrame() &&
            !low.action.WasPressedThisFrame() &&
            !bottom.action.WasPressedThisFrame()
        )
        {
            t = 0;
            t += fallSpeed * Time.deltaTime;
            t = Mathf.Clamp01(t);
            //float curveT = Mathf.SmoothStep(0f, 1f, t);
            //transform.position = new Vector3(0, -3.2f, 0);
            transform.position = Vector3.Lerp(transform.position, new Vector3(drumXVal, -3.2f, 0), t);
            guitarist.transform.position = Vector3.Lerp(guitarist.transform.position, new Vector3(guitarXval, -3f, 0), t);
            collector.transform.position =
                new Vector3(collector.transform.position.x,
                    tracksController.Track4.transform.position.y,
                    collector.transform.position.z);
            current_track_position = 0;
        }
    }
}
