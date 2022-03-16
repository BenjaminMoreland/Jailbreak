using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;

    private Animator friendAnim;
    private bool followTarget;
    private bool playerFaceR;
    [HideInInspector]
    public Rigidbody2D rb;
    public SpriteRenderer render;
    private Vector2 movement;
    private float moveInput;
    [HideInInspector]
    public bool free = false;

    // Start is called before the first frame update
    void Start()
    {
        playerFaceR = FindObjectOfType<MainPlayerController>();
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        friendAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        friendAnim.SetBool("Follow", followTarget);
        moveInput = FindObjectOfType<MainPlayerController>().moveInputH;
        if (player != null && free)
        {
            // Code by: Logan Laurance
            // Checks if either the player is moving right or left, then updates the boolean variable accordingly
            if (moveInput > 0)
            {
                followTarget = true;
                playerFaceR = true;
            }
            else if (moveInput < 0)
            {
                followTarget = true;
                playerFaceR = false;
            }
            else if(moveInput == 0)
            {
                followTarget = false;
            }
            if (playerFaceR)
            {
                Vector3 direction = player.position - new Vector3(2, 0, 0) - transform.position;
                direction.Normalize();
                movement = direction;
            }
            else if (!playerFaceR)
            {
                Vector3 direction = player.position + new Vector3(2, 0, 0) - transform.position;
                direction.Normalize();
                movement = direction;
            }
        }
    }
    private void FixedUpdate()
    {
        float yposf = Mathf.Round(rb.position.y);
        float yposp = Mathf.Round(player.position.y);
        if (yposf > yposp && free)
        {
            rb.transform.position = player.position;
        }
        if (followTarget && free)
        {
            moveCharacter(movement);
        }
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }
}
