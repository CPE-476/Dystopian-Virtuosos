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

    private int direction = 1;

    bool isJumping = false;

    private bool alive = true;

    public float startX;

    public bool reset = false;

    public uint playerState;

    public Conductor conductor;

    private double curSongPosition;

    public CameraController cam;

    public TracksController tracksController;

    public GameObject collector;

    private float playerHeightOffset = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startX = rb.transform.position.x;
        reset = true;
        curHealth = maxHealth;
        hb.setMaxHealth(maxHealth);
        curSongPosition = conductor.songPosition;
    }

    private void Update()
    {
        if (alive)
        {
            Attack();
            Jump();
            Run();
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
            direction = 1;
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(direction, 1, 1) * 0.5f;
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
            playerState = 3;

            curSongPosition = conductor.songPosition;
            anim.SetTrigger("attack");
        }
        if (
            Input.GetKeyDown(KeyCode.Joystick1Button2) ||
            Input.GetKeyDown(KeyCode.J)
        )
        {
            transform.position = new Vector3(0, tracksController.Track2.transform.position.y - playerHeightOffset, 0);
            collector.transform.position = new Vector3(collector.transform.position.x, tracksController.Track2.transform.position.y, collector.transform.position.z);
            playerState = 2;
            curSongPosition = conductor.songPosition;
            anim.SetTrigger("attack");
        }
        if (
            Input.GetKeyDown(KeyCode.Joystick1Button0) ||
            Input.GetKeyDown(KeyCode.K)
        )
        {
            transform.position = new Vector3(0, tracksController.Track3.transform.position.y - playerHeightOffset, 0);
            collector.transform.position = new Vector3(collector.transform.position.x, tracksController.Track3.transform.position.y, collector.transform.position.z);
            playerState = 1;
            curSongPosition = conductor.songPosition;
            anim.SetTrigger("attack");
        }
        if (
           Input.GetKeyDown(KeyCode.L)
        )
        {
            transform.position = new Vector3(0, tracksController.Track4.transform.position.y - playerHeightOffset, 0);
            collector.transform.position = new Vector3(collector.transform.position.x, tracksController.Track4.transform.position.y, collector.transform.position.z);
            playerState = 0;
            curSongPosition = conductor.songPosition;
            anim.SetTrigger("attack");
        }

        if (
            curSongPosition + (conductor.spotLength * 8) <=
            conductor.songPosition &&
            !Input.GetKey(KeyCode.Joystick1Button2) &&
            !Input.GetKey(KeyCode.Joystick1Button3) &&
            !Input.GetKey(KeyCode.Joystick1Button0)
        )
        {
            transform.position = new Vector3(0, tracksController.Track4.transform.position.y - playerHeightOffset, 0);
            playerState = 0;
        }
    }
}
