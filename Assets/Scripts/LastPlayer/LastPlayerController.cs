using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private GameManager gameManager;

    public FadeOut fadeOut;
    public CharacterStats characterStats;

    public PlayerUI playerUI;

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

    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float dashStaminaCost = 20f;
    [SerializeField] private float attackStaminaCost = 5f;
    [SerializeField] private float comboStaminaCost = 20f; 

    public Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;

    private float lastAttackTime = 0f;
    public float attackRate = 1f;
    float nextAttackTime = 0f;
    private int attackClickCount = 1;

    public bool canTakeDamage = true;
    private int damage=10;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //characterStats.characterStamina = maxStamina;
        gameManager = GameManager.Instance;
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
        if (Time.time > lastAttackTime + 5f)
          {
               attackClickCount = 1;
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
            CheckAttackTime();
        }
        Death();
    }

    private void CheckAttackTime()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && isGrounded && PopupUIManager.instance.activePopupLList.Count <= 0)
            {
                nextAttackTime = Time.time + 0.5f / attackRate;
                Attack();
            }
        }
    }
    public void CheckInput()
    {
        movingInput = Input.GetAxis("Horizontal");

        if (Input.GetAxis("Vertical") < 0)
        {
            canWallSlide = false;
        }
        if (Input.GetMouseButtonDown(1))
        {
            canTakeDamage = false;
            Debug.Log("누름");
        }
        else if (Input.GetMouseButtonUp(1))
        {
            canTakeDamage = true;
            Debug.Log("땜");
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
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
            rb.velocity = new Vector2(movingInput * speed, rb.velocity.y);
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown)
        {
            if (characterStats.characterStamina >= dashStaminaCost)
            {
                characterStats.characterStamina -= dashStaminaCost;
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

    private void RegenStamina()
    {
        characterStats.characterStamina += staminaRegenRate * Time.deltaTime;
        // Fix
        characterStats.characterStamina = Mathf.Clamp(characterStats.characterStamina, 0f, 100f);
    }

    //private void Attack()
    //{
    //        if (characterStats.characterStamina >= attackStaminaCost)
    //        {
    //            characterStats.characterStamina -= attackStaminaCost;
    //            anim.SetTrigger("attack");
    //            Debug.Log(attackClickCount);
    //            int modifiedAttackDamage = characterStats.characterNomallAttackDamage;

    //            if (attackClickCount !=0 && attackClickCount % 3 == 0)
    //            {
    //                characterStats.characterStamina -= comboStaminaCost;
    //                anim.SetTrigger("combo");
    //                modifiedAttackDamage += 10;
    //                attackClickCount = 0;
    //            }
    //            ApplyDamage(modifiedAttackDamage);
    //        }
    //}
    private void Attack()
    {
        if (gameManager.playerStats.characterStamina >= attackStaminaCost)
        {
            gameManager.playerStats.characterStamina -= attackStaminaCost;
            anim.SetTrigger("attack");
            gameManager.playerStats.AttackDamage(damage);
            int modifiedAttackDamage =damage;
            if (attackClickCount != 0 && attackClickCount % 3 == 0)
            {
                gameManager.playerStats.characterStamina -= comboStaminaCost;
                anim.SetTrigger("combo");
                modifiedAttackDamage += 10;
                attackClickCount = 0;
            }
            ApplyDamage(modifiedAttackDamage);
        }
    }

    //Debug.Log(canTakeDamage);//PlayerToMonster

    private void ApplyDamage(int damage) // Add damage To Monster
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Boss_DB"))
            {
                lastAttackTime = Time.time;
                attackClickCount++;
                DeathBringerEnemy deathBringer = enemyCollider.GetComponent<DeathBringerEnemy>();
                if (deathBringer != null)
                {
                    deathBringer.TakeDamage(damage);
                    PlayerEvents.playerDamaged.Invoke(gameObject, damage);
                }
            }
            else if (enemyCollider.CompareTag("Boss_Archer"))
            {
                Boss_Archer boss_archer = enemyCollider.GetComponent<Boss_Archer>();
                if (boss_archer != null)
                {
                    Debug.Log("Deal" + characterStats.characterNomallAttackDamage + " damage to Boss Archer.");
                    //boss_archer.TakeDamage(attackDamage);
                    boss_archer.TakeDamage(characterStats.characterNomallAttackDamage);
                }
            }
            else if (enemyCollider.CompareTag("skeleton"))
            {
                skeletonEnemy skeleton = enemyCollider.GetComponent<skeletonEnemy>();
                if (skeleton != null)
                {
                    Debug.Log("Deal" + characterStats.characterNomallAttackDamage + " damage to Skeleton.");
                    skeleton.TakeDamage(characterStats.characterNomallAttackDamage);
                }
            }
            else if (enemyCollider.CompareTag("archer"))
            {
                archerEnemy archer = enemyCollider.GetComponent<archerEnemy>();
                if (archer != null)
                {
                    Debug.Log("Deal " + characterStats.characterNomallAttackDamage + " damage to Archer.");
                    archer.TakeDamage(characterStats.characterNomallAttackDamage);
                }
            }
        }
    }


    private void Death()
    {
        if (characterStats.characterHp <= 0)
        {
            Debug.Log("DeathAnim");
            anim.SetBool("isDeath", true);
            canMove = false;
            rb.velocity = Vector2.zero;
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
        else if (verticalInput < 0)
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