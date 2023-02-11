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

            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            anim.SetBool("isJump", false);
        }


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
                moveVelocity = Vector3.zero;
                anim.SetBool("isRun", false);
            }

            // transform.position += moveVelocity * movePower * Time.deltaTime;
        }
        void Jump()
        {
            if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0)
            && !anim.GetBool("isJump"))
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
            if (Input.GetKeyDown(KeyCode.Joystick1Button3) || Input.GetKeyDown(KeyCode.H))
            {
                transform.position = new Vector3(0, 1, 0);
                playerState = 2;

                curSongPosition = conductor.songPosition;
                anim.SetTrigger("attack");
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.J))
            {
                transform.position = new Vector3(0, -0.2f, 0);
                playerState = 1;
                curSongPosition = conductor.songPosition;
                anim.SetTrigger("attack");
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.K))
            {
                transform.position = new Vector3(0, -2, 0);
                playerState = 0;
                curSongPosition = conductor.songPosition;
                anim.SetTrigger("attack");
            }

            if(curSongPosition+(conductor.spotLength*8) <= conductor.songPosition &&
                !Input.GetKey(KeyCode.Joystick1Button2) && 
                !Input.GetKey(KeyCode.Joystick1Button3) && 
                !Input.GetKey(KeyCode.Joystick1Button0))
            {
                transform.position = new Vector3(0, -2, 0);
                playerState = 0;
            }
        }
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
        
    }