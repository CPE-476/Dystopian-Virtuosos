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
            transform.position =
                new Vector3(0,
                    tracksController.Track1.transform.position.y -
                    playerHeightOffset,
                    0);
            collector.transform.position =
                new Vector3(collector.transform.position.x,
                    tracksController.Track1.transform.position.y,
                    collector.transform.position.z);
            current_track_position = 3;
            lastNoteHitTime = conductor.GetSongPosition();

            if (prev_track_pos == current_track_position)
                anim.SetTrigger("kickAttack");
            else if (prev_track_pos < current_track_position)
                anim.SetTrigger("attack");
            else if (prev_track_pos > current_track_position)
                anim.SetTrigger("downAttack");
        }
        if (high.action.WasPressedThisFrame())
        {
            transform.position =
                new Vector3(0,
                    tracksController.Track2.transform.position.y -
                    playerHeightOffset,
                    0);
            collector.transform.position =
                new Vector3(collector.transform.position.x,
                    tracksController.Track2.transform.position.y,
                    collector.transform.position.z);
            current_track_position = 2;
            lastNoteHitTime = conductor.GetSongPosition();

            if (prev_track_pos == current_track_position)
                anim.SetTrigger("kickAttack");
            else if (prev_track_pos < current_track_position)
                anim.SetTrigger("attack");
            else if (prev_track_pos > current_track_position)
                anim.SetTrigger("downAttack");
        }
        if (low.action.WasPressedThisFrame())
        {
            transform.position =
                new Vector3(0,
                    tracksController.Track3.transform.position.y -
                    playerHeightOffset,
                    0);
            collector.transform.position =
                new Vector3(collector.transform.position.x,
                    tracksController.Track3.transform.position.y,
                    collector.transform.position.z);
            current_track_position = 1;
            lastNoteHitTime = conductor.GetSongPosition();

            if (prev_track_pos == current_track_position)
                anim.SetTrigger("kickAttack");
            else if (prev_track_pos < current_track_position)
                anim.SetTrigger("attack");
            else if (prev_track_pos > current_track_position)
                anim.SetTrigger("downAttack");
        }
        if (bottom.action.WasPressedThisFrame())
        {
            transform.position =
                new Vector3(0,
                    tracksController.Track4.transform.position.y -
                    playerHeightOffset,
                    0);
            collector.transform.position =
                new Vector3(collector.transform.position.x,
                    tracksController.Track4.transform.position.y,
                    collector.transform.position.z);
            current_track_position = 0;
            lastNoteHitTime = conductor.GetSongPosition();

            if (prev_track_pos == current_track_position)
                anim.SetTrigger("kickAttack");
            else if (prev_track_pos < current_track_position)
                anim.SetTrigger("attack");
            else if (prev_track_pos > current_track_position)
                anim.SetTrigger("downAttack");
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
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, -3.2f, 0), t);
            collector.transform.position =
                new Vector3(collector.transform.position.x,
                    tracksController.Track4.transform.position.y,
                    collector.transform.position.z);
            current_track_position = 0;
        }
    }
}
