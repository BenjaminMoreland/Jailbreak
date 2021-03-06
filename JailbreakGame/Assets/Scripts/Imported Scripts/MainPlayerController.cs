using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Literally all of this is made by Ryan

public class MainPlayerController : MonoBehaviour
{
    //speed and movement variables
    public float speed;
    public float airSpeed;
    [HideInInspector]
    public float moveInputH;

    //screen shake things
    public FollowCamera FC;

    //grab this to adujust physics
    private Rigidbody2D myRb;
    [HideInInspector]
    public Collider2D myCollider;

    //used for checking what direction to be flipped, made public for the FollowCamera script to grab
    [HideInInspector]
    public bool facingRight = true;

    //things for ground checking
    private bool isGrounded = false;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //jump things
    public int extraJumps = 1;
    private int jumps;
    public float jumpForce;
    private bool jumpPressed = true;
    private bool havePressedJump = false;

    private float jumpTimer = 0;
    public float jumpTime = 0.2f;

    public float gravityScale = 5;

    public float groundDrag = 5;
    public float airDrag = 1;

    private AudioSource myAud;
    public AudioClip jumpNoise;
    public AudioClip landNoise;
    public AudioClip footSteps;
    public AudioClip deathNoise;

    //ladder things
    private bool isClimbing;
    public LayerMask whatIsLadder;
    public float ladderDist;
    private float moveInputV;
    public float climbSpeed;

    //Respawn info
    [HideInInspector]
    public Vector3 RespawnPoint = new Vector3();

    //animation
    private Animator myAnim;

    //use this to turn on and off player controls
    private bool controlOn = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Zapper")) 
            {
            StartCoroutine(OnDeath());
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myAud = GetComponent<AudioSource>();
        myAnim = GetComponent<Animator>();
        FC = FindObjectOfType<FollowCamera>();

        jumps = extraJumps;

        RespawnPoint = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (controlOn)
        {
            // Sets various animator variables to make movement proper, climbing work, crawling crawl, etc etc.
            myAnim.SetFloat("Speed", Mathf.Abs(moveInputH));
            myAnim.SetBool("isClimbing", isClimbing);
            myAnim.SetFloat("IsMovingUp", Mathf.Abs(moveInputV));
            myAnim.SetBool("hasJumped", jumpPressed);

            //check for ground
            moveInputH = Input.GetAxisRaw("Horizontal");
            if (isGrounded == true)
            {
                // Code by: Logan Laurance
                // Plays the landing noise once the player has landed, then resets it to false
                if(jumpPressed == false && havePressedJump == true && isClimbing == false)
                {
                    myAud.pitch = 1;
                    myAud.PlayOneShot(landNoise);
                    havePressedJump = false;
                }
                jumps = extraJumps;
            }
            //check if jump can be triggered
            if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && isGrounded == true && isClimbing == false)
            {
                myAud.pitch = 1;
                myAud.PlayOneShot(jumpNoise);
                myRb.drag = airDrag;
                if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
                {
                    myRb.velocity = (Vector2.up * jumpForce);
                }
                else
                {
                    myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
                }
                jumpPressed = true;
                havePressedJump = true;
            }
            else if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && jumps > 0 && isClimbing == false)
            {
                myAud.pitch = 1.2f;
                myAud.PlayOneShot(jumpNoise);
                myRb.drag = airDrag;
                if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
                {
                    myRb.velocity = (Vector2.up * jumpForce);
                }
                else
                {
                    myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
                }
                jumpPressed = true;
                havePressedJump = true;
                jumps--;
            }
            else if (Input.GetAxisRaw("Jump") == 0)
            {
                jumpPressed = false;
                jumpTimer = 0;
            }
            else if (jumpPressed == true && jumpTimer < jumpTime)
            {
                jumpTimer += Time.deltaTime;
                myRb.drag = airDrag;
                myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
                jumpPressed = true;
                havePressedJump = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (controlOn)
        {
            //check for ground
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

            //set animators on ground
            myAnim.SetBool("OnGround", isGrounded);

            //ladder things


            moveInputV = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Jump");
            //check for the ladder if around the player
            RaycastHit2D hitInfo = Physics2D.Raycast(groundCheck.position, Vector2.up, ladderDist, whatIsLadder);

            //if ladder was found see if we are climbing, stop falling
            if (hitInfo.collider != null)
            {
                myRb.gravityScale = 0;
                isClimbing = true;
                if (moveInputV > 0)
                {
                    myRb.AddForce(new Vector2(0, climbSpeed));
                }
                else if (moveInputV < 0)
                {
                    myRb.AddForce(new Vector2(0, -climbSpeed));
                }
                else
                {
                    myRb.velocity = new Vector2(myRb.velocity.x, 0);
                }
            }
            else
            {
                myRb.gravityScale = gravityScale;
                isClimbing = false;
            }

           
            //horizontal movement
            moveInputH = Input.GetAxisRaw("Horizontal");
            //animator settings
            if (moveInputH == 0)
            {
                myAnim.SetBool("Moving", false);
            }
            else
            {
                myAnim.SetBool("Moving", true);
            }

            if (isGrounded && !jumpPressed || isClimbing)
            {
                myRb.drag = groundDrag;
                myRb.AddForce(new Vector2(moveInputH * speed, 0));
            }
            else
            {
                myRb.drag = airDrag;
                myRb.AddForce(new Vector2(moveInputH * airSpeed, 0));
            }
            //check if we need to flip the player direction
            if (facingRight == false && moveInputH > 0)
                Flip();
            else if (facingRight == true && moveInputH < 0)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Zapper"))
        {
            StartCoroutine(OnDeath());
        }
    }

    private void Footsteps()
    {
        myAud.PlayOneShot(footSteps);
    }

    private IEnumerator OnDeath()
    {
        //stop moving
        myRb.velocity = Vector2.zero;

        //start death animation
        //make this the name of whatever death animation is on your player.
        //myAnim.Play("");

        //disable gravity
        myRb.gravityScale = 0;

        //disable player controls
        controlOn = false;

        //disable player collision
        myCollider.enabled = false;

        //stop other sound effects and play the death sound effect
        myAud.Stop();
        myAud.PlayOneShot(deathNoise);

        //time it takes for animation to complete
        yield return new WaitForSeconds(1.15f);

        //re enable everything disabled
        myCollider.enabled = true;
        controlOn = true;
        myRb.gravityScale = gravityScale;

        //respawn at designated location
        transform.position = RespawnPoint;
    }
}