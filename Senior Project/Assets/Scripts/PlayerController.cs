using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movePower = 2f;
    public float jumpPower = 5f; //Set Gravity Scale in Rigidbody2D Component to 5

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

    public CamerController cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startX = rb.transform.position.x;
        reset = true;
        curSongPosition = conductor.songPosition;
    }

    private void Update()
    {
        Restart();
        if (alive)
        {
            Hurt();
            Die();
            Attack();
            Jump();
            Run();
<<<<<<< HEAD

        }
    }
=======
        }
    }

>>>>>>> e405f8c7f9636327278a6ee3e9da4cea418ea372
    private void OnTriggerEnter2D(Collider2D other)
    {
        anim.SetBool("isJump", false);
    }

<<<<<<< HEAD

    void Run()
    {
        Vector3 moveVelocity;
        if (cam.isMoving) {
            direction = 1;
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(direction, 1, 1) * 0.5f;
            if (!anim.GetBool("isJump"))
                anim.SetBool("isRun", true);
        }
        else {
=======
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
>>>>>>> e405f8c7f9636327278a6ee3e9da4cea418ea372
            moveVelocity = Vector3.zero;
            anim.SetBool("isRun", false);
        }

        // transform.position += moveVelocity * movePower * Time.deltaTime;
    }
<<<<<<< HEAD
    void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0)
        && !anim.GetBool("isJump"))
=======

    void Jump()
    {
        if (
            (Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0) &&
            !anim.GetBool("isJump")
        )
>>>>>>> e405f8c7f9636327278a6ee3e9da4cea418ea372
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
<<<<<<< HEAD

        isJumping = false;
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.H))
=======
        isJumping = false;
    }

    void Attack()
    {
        if (
            Input.GetKeyDown(KeyCode.Joystick1Button3) ||
            Input.GetKeyDown(KeyCode.H)
        )
>>>>>>> e405f8c7f9636327278a6ee3e9da4cea418ea372
        {
            transform.position = new Vector3(0, 1, 0);
            playerState = 2;

            curSongPosition = conductor.songPosition;
            anim.SetTrigger("attack");
        }
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.J))
=======
        if (
            Input.GetKeyDown(KeyCode.Joystick1Button2) ||
            Input.GetKeyDown(KeyCode.J)
        )
>>>>>>> e405f8c7f9636327278a6ee3e9da4cea418ea372
        {
            transform.position = new Vector3(0, -0.2f, 0);
            playerState = 1;
            curSongPosition = conductor.songPosition;
            anim.SetTrigger("attack");
        }
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.K))
=======
        if (
            Input.GetKeyDown(KeyCode.Joystick1Button0) ||
            Input.GetKeyDown(KeyCode.K)
        )
>>>>>>> e405f8c7f9636327278a6ee3e9da4cea418ea372
        {
            transform.position = new Vector3(0, -2, 0);
            playerState = 0;
            curSongPosition = conductor.songPosition;
            anim.SetTrigger("attack");
        }

<<<<<<< HEAD
        if(curSongPosition+(conductor.spotLength*8) <= conductor.songPosition &&
            !Input.GetKey(KeyCode.Joystick1Button2) && 
            !Input.GetKey(KeyCode.Joystick1Button3) && 
            !Input.GetKey(KeyCode.Joystick1Button0))
=======
        if (
            curSongPosition + (conductor.spotLength * 8) <=
            conductor.songPosition &&
            !Input.GetKey(KeyCode.Joystick1Button2) &&
            !Input.GetKey(KeyCode.Joystick1Button3) &&
            !Input.GetKey(KeyCode.Joystick1Button0)
        )
>>>>>>> e405f8c7f9636327278a6ee3e9da4cea418ea372
        {
            transform.position = new Vector3(0, -2, 0);
            playerState = 0;
        }
    }
<<<<<<< HEAD
    void Hurt()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("hurt");
            //if (direction == 1)
                //rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
            //else
                //rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
        }
    }
    void Die()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetTrigger("die");
            alive = false;
        }
    }
    void Restart()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            anim.SetTrigger("idle");
            alive = true;
        }
    }
    
=======

    void Hurt()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     anim.SetTrigger("hurt");
        //     //if (direction == 1)
        //         //rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
        //     //else
        //         //rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
        // }
    }

    void Die()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha4))
        // {
        //     anim.SetTrigger("die");
        //     alive = false;
        // }
    }

    void Restart()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     anim.SetTrigger("idle");
        //     alive = true;
        // }
    }
>>>>>>> e405f8c7f9636327278a6ee3e9da4cea418ea372
}
