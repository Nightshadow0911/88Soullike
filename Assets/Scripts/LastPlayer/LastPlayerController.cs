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
    [SerializeField] private float jumpForce = 10;

    private bool canMove = true;

    private bool canWallSlide;
    private bool isWallSliding;

    private bool facingRight = true;
    private float movingInput;
    private int facingDirection = 1;
    [SerializeField] private Vector2 wallJumpDirection;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask WhatIsLadder;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float ladderCheckdistance;
    private bool isGrounded;
    private bool isWallDetected;
    private bool isLadderDetected;

    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashStartTime;
    private float lastDashTime;


    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float dashStaminaCost = 20f;
    [SerializeField] private float attackStaminaCost = 5f;


    public Transform attackPoint;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer;
    public int attackDamage = 10;




    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
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
            RegenStamina();
        }

        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }

        if (isLadderDetected)
        {
            ClimbLadder();
        }
        else
        {
            ReleaseLadder();
            Move();
        Dash();
        Attack();
        }
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

    private void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(movingInput * speed, rb.velocity.y);
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown)
        {
            if (currentStamina >= dashStaminaCost)
            {
                currentStamina -= dashStaminaCost;
                fadeOut.makeFadeOut = true;
                isDashing = true;
                dashStartTime = Time.time;
                lastDashTime = Time.time;
                canMove = false;
            }
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

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentStamina>= attackStaminaCost)
            {
                currentStamina -= attackStaminaCost;
                anim.SetTrigger("attack");

                ApplyDamage();
            }
        }
    }

    private void ApplyDamage() //몬스터 추가될 때 
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Boss_DB"))
            {
                DeathBringerEnemy deathBringer = enemyCollider.GetComponent<DeathBringerEnemy>();
                if (deathBringer != null)
                {
                    Debug.Log("플레이어가 중보에게 " + attackDamage + "만큼 피해를 입혔습니다.");
                    deathBringer.TakeDamage(attackDamage);
                }
            }
            else if (enemyCollider.CompareTag("Boss_Archer"))
            {
                Boss_Archer boss_archer = enemyCollider.GetComponent<Boss_Archer>();
                if (boss_archer != null)
                {
                    Debug.Log("플레이어가 보스몹에게 " + attackDamage + "만큼 피해를 입혔습니다.");
                    boss_archer.TakeDamage(attackDamage);
                }
            }
            else if (enemyCollider.CompareTag("skeleton"))
            {
                skeletonEnemy skeleton = enemyCollider.GetComponent<skeletonEnemy>();
                if (skeleton != null)
                {
                    Debug.Log("플레이어가 해골몹에게 " + attackDamage + "만큼 피해를 입혔습니다.");
                    skeleton.TakeDamage(attackDamage);
                }
            }
            else if (enemyCollider.CompareTag("archer"))
            {
                archerEnemy archer = enemyCollider.GetComponent<archerEnemy>();
                if (archer != null)
                {
                    Debug.Log("플레이어가 궁수몹에게 " + attackDamage + "만큼 피해를 입혔습니다.");
                    archer.TakeDamage(attackDamage);
                }
            }
        }
    }


    private void RegenStamina()
    {
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
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

    private void ClimbLadder()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0)
        {
            rb.velocity = new Vector2(0, speed);
        }
        else if(verticalInput < 0)
        {
            rb.velocity = new Vector2(0, -speed);
        }

    }

    private void ReleaseLadder()
    {
        rb.gravityScale = 1;
    }


    private void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        isLadderDetected = Physics2D.Raycast(transform.position, Vector2.up, ladderCheckdistance, WhatIsLadder);

        if (isWallDetected && rb.velocity.y < 0)
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
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance * facingDirection, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));

        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
