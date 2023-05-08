using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movePower = 2f;
    public float jumpPower = 5f; //Set Gravity Scale in Rigidbody2D Component to 5

    public int maxHealth;

    public int curHealth;

    public HealthBar hb;

    private Rigidbody2D rb;

    public Animator anim;

    bool isJumping = false;

    public InterfaceState state = InterfaceState.GAMEPLAY;

    public float startX;

    public bool reset = false;

    public uint current_track_position;

    public Conductor conductor;

    public MIDIReader midiReader;

    public CameraController cam;

    public TracksController tracksController;

    public Dialogue dialogue;

    public GameObject collector;

    public NoteTrigger noteTrigger;

    private float playerHeightOffset = 1.2f;

    private double lastNoteHitTime;

    private string[] MIDIOrder = {"drum", "piano", "guitar"};

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startX = rb.transform.position.x;
        reset = true;
        curHealth = maxHealth;
        hb.setMaxHealth(maxHealth);
    }

    private void Update()
    {
        switch (state)
        {
            case InterfaceState.GAMEPLAY:
            {
                Attack();
                Jump();
                Run();

                // Switch State (DEBUG)
                if (noteTrigger.finished || reset)
                {
                    state = InterfaceState.DIALOGUE;
                    cam.isMoving = false;
                    reset = false;
                    noteTrigger.finished = false;
                    dialogue.Enable();
                }
            }
            break;
            case InterfaceState.DIALOGUE:
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    int result = dialogue.NextLine();
                    Debug.Log("result: " + result);
                    Debug.Log("current break: " + dialogue.sectionBreak[dialogue.currentSection]);
                    if (result == -1 || result == dialogue.sectionBreak[dialogue.currentSection])
                    {
                        // Dialogue over.
                        state = InterfaceState.GAMEPLAY;
                        cam.isMoving = true;
                        if (MIDIOrder[dialogue.currentSection] == "drum"){
                            conductor.playDrumsNextBar = true;
                            midiReader.ended = true;
                            dialogue.currentSection++;
                        }
                        else if (MIDIOrder[dialogue.currentSection] == "piano"){
                            conductor.playPianoNextBar = true;
                            midiReader.ended = true;
                            dialogue.currentSection++;
                        }
                        else if (MIDIOrder[dialogue.currentSection] == "guitar"){
                            conductor.playGuitarNextBar = true;
                        }
                        
                        dialogue.Disable();
                    }
                    
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
        Vector3 moveVelocity;
        if (cam.isMoving)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1) * 0.5f;
            if (!anim.GetBool("isJump")) anim.SetBool("isRun", true);
        }
        else
        {
            moveVelocity = Vector3.zero;
            anim.SetBool("isRun", false);
        }

        // transform.position += moveVelocity * movePower * Time.deltaTime;
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
        if (
            Input.GetKeyDown(KeyCode.Joystick1Button3) ||
            Input.GetKeyDown(KeyCode.H)
        )
        {
            transform.position = new Vector3(0, tracksController.Track1.transform.position.y - playerHeightOffset, 0);
            collector.transform.position = new Vector3(collector.transform.position.x, tracksController.Track1.transform.position.y, collector.transform.position.z);
            current_track_position = 3;
            lastNoteHitTime = conductor.GetSongPosition();

            anim.SetTrigger("attack");
        }
        if (
            Input.GetKeyDown(KeyCode.Joystick1Button2) ||
            Input.GetKeyDown(KeyCode.J)
        )
        {
            transform.position = new Vector3(0, tracksController.Track2.transform.position.y - playerHeightOffset, 0);
            collector.transform.position = new Vector3(collector.transform.position.x, tracksController.Track2.transform.position.y, collector.transform.position.z);
            current_track_position = 2;
            lastNoteHitTime = conductor.GetSongPosition();

            anim.SetTrigger("attack");
        }
        if (
            Input.GetKeyDown(KeyCode.Joystick1Button0) ||
            Input.GetKeyDown(KeyCode.K)
        )
        {
            transform.position = new Vector3(0, tracksController.Track3.transform.position.y - playerHeightOffset, 0);
            collector.transform.position = new Vector3(collector.transform.position.x, tracksController.Track3.transform.position.y, collector.transform.position.z);
            current_track_position = 1;
            lastNoteHitTime = conductor.GetSongPosition();

            anim.SetTrigger("attack");
        }
        if (
           Input.GetKeyDown(KeyCode.L)
        )
        {
            transform.position = new Vector3(0, tracksController.Track4.transform.position.y - playerHeightOffset, 0);
            collector.transform.position = new Vector3(collector.transform.position.x, tracksController.Track4.transform.position.y, collector.transform.position.z);
            current_track_position = 0;
            lastNoteHitTime = conductor.GetSongPosition();

            anim.SetTrigger("attack");
        }

        if (
            conductor.GetSongPosition() + (conductor.spotLength * 8) <= lastNoteHitTime &&
            !Input.GetKey(KeyCode.Joystick1Button2) &&
            !Input.GetKey(KeyCode.Joystick1Button3) &&
            !Input.GetKey(KeyCode.Joystick1Button0)
        )
        {
            transform.position = new Vector3(0, tracksController.Track4.transform.position.y - playerHeightOffset, 0);
            current_track_position = 0;
        }
    }
}
