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
    private bool facingRight = true;

    private float movingInput;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadious;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        movingInput = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
        CollisionCheck();
        FlipController();
        AnimatorController();
    }



    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movingInput * speed, rb.velocity.y);

    }

    private void JumpButton()
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, -180, 0);
    }
    private void FlipController()
    {
        if (rb.velocity.x>0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x<0 && facingRight)
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
    }

    private void CollisionCheck()
    {
        isGrounded= Physics2D.OverlapCircle(groundCheck.position, groundCheckRadious, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadious);
    }
}
