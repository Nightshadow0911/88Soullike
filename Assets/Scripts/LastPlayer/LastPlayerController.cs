using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    public FadeOut fadeOut;


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


    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashStartTime;
    private float lastDashTime;

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

        if (isGrounded)
        {
            canMove = true;
        }

        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }
        Move();
        Dash();
    }

    private void CheckInput()
    {
            movingInput = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Vertical") < 0)
        {
            canWallSlide = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }
    //Move
    private void Move()
    {
        if (canMove)
        {
         rb.velocity = new Vector2(movingInput * speed, rb.velocity.y);
         //fadeOut.makeFadeOut = true;
        }
    }
    //Dash
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown)
        {
            fadeOut.makeFadeOut = true;
            isDashing = true;
            dashStartTime = Time.time;
            lastDashTime = Time.time;
            canMove = false; // 대시 중에 움직임 비활성화 (선택 사항)
        }

        if (isDashing && Time.time < dashStartTime + dashDuration)
        {
            rb.velocity = new Vector2(facingDirection * dashDistance / dashDuration, rb.velocity.y);
        }
        else
        {
            isDashing = false;
            fadeOut.makeFadeOut = false;
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
        canWallSlide = false;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void wallJump()
    {
        //WallJumpDirections flozen
        //canMove = false;
        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
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
        anim.SetBool("isWallDectected", isWallDetected);
    }

    private void CollisionCheck()
    {
        isGrounded= Physics2D.Raycast(transform.position,Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right*facingDirection, wallCheckDistance, whatIsGround);
        if (isWallDetected && rb.velocity.y<0)
        {
            canWallSlide = true;
        }
        if (!isWallDetected)
        {
            canWallSlide = false;
            isWallSliding = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance*facingDirection, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y-groundCheckDistance));

    }
}
