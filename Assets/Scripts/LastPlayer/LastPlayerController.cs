using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5;
    [SerializeField]private float jumpForce =10;

    private bool canMove = true;

    private bool canWallSlide;
    private bool isWallSliding;

    private bool facingRight = true;
    private float movingInput;
    private int facingDirection = 1;
    [SerializeField] private Vector2 wallJumpDirection;

    //Ground check
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float wallCheckDistance;
    private bool isGrounded;
    private bool isWallDetected;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        CheckInput();
        CollisionCheck();
        FlipController();
        AnimatorController();
    }



    private void FixedUpdate()
    {
        if (isWallDetected && canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y * 0.1f);
        }
        else
        {
            isWallSliding = false;
            Move();
        }
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
        if (canMove)
        {
            movingInput = Input.GetAxis("Horizontal");
        }
    }

    private void Move()
    {
        if (canMove)
        {
         rb.velocity = new Vector2(movingInput * speed, rb.velocity.y);
        }
    }

    private void JumpButton()
    {
        if (isWallSliding)
        {
            wallJump();
        }
        else if (isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void wallJump()
    {

        Vector2 direction = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
        rb.AddForce(direction, ForceMode2D.Impulse);

    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
            //spriteRenderer.flipX = !spriteRenderer.flipX;

    }

    private void FlipController()
    {
        if (isGrounded && isWallDetected)
        {
            if (facingRight && movingInput < 0)
            {
                Flip();
            }
            else if (!facingRight && movingInput > 0)
            {
                Flip();
            }
        }

        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }

    }

    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity",rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void CollisionCheck()
    {
        isGrounded= Physics2D.Raycast(transform.position,Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right*facingDirection, wallCheckDistance, whatIsGround);
        if (!isGrounded && rb.velocity.y<0)
        {
            canWallSlide = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y-groundCheckDistance));

    }
}
