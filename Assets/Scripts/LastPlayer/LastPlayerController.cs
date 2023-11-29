using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private PlayerAttack playerAttack;

    public FadeOut fadeOut;
    public CharacterStats characterStats;
    public LedgeCheck ledgeCheck;

    public PlayerUI playerUI;
    [SerializeField] private float jumpForce = 10;

    private bool canMove = true;

    private bool canWallSlide;
    private bool isWallSliding;

    public bool facingRight = true;
    private float movingInput;
    public int facingDirection = 1;
    [SerializeField] private Vector2 wallJumpDirection;

    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private LayerMask whatIsLadder;
    [SerializeField] private LayerMask whatIsLedge;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float ladderCheckdistance;
    [SerializeField] private float ceilCheckDistance;
    [SerializeField] private LayerMask whatIsCeil;

    public bool isGrounded;
    private bool isWallDetected;
    private bool isLadderDetected;
    public bool isCeilDetected = true;

    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    public bool isDashing = false;
    private float dashStartTime;
    private float lastDashTime;

    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaRegenRate = 10f;
    [SerializeField] private float dashStaminaCost = 20f;
    [SerializeField] public float attackStaminaCost = 5f;
    [SerializeField] public float comboStaminaCost = 20f;
    [HideInInspector] public bool ledgeDetected;

    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    public bool canGrabLedge = true;
    private bool isClimbing;

    public bool isSitting;
    public bool canDash = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        CheckInput();
        UseMana();
        CollisionCheck();
        FlipController();
        AnimatorController();
        ClimbLadder();
        if (isGrounded)
        {
            canMove = true;
            RegenStamina();
        }
        if (canWallSlide)
        {
            IsWallSliding();
        }
        else
        {
            Move();
            Dash();
            CheckForLedge();
        }
        Death();
    }

    private void UseMana()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (characterStats.characterMana >= 1)
            {
                characterStats.characterMana -= 1;
            }
        }
    }

    private void IsWallSliding()
    {
        isWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
    }

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            Vector2 ledgePosition = ledgeCheck.transform.position;
            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;
            isClimbing = true;
            anim.SetBool("isClimbing", true);
        }
        if (isClimbing)
        {
            transform.position = climbBegunPosition;
        }
    }

    private void LedgeClimbOver()
    {
        isClimbing = false;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", 1f);
    }

    private IEnumerator LedgeClimbOverCoroutine()
    {
        float elapsedTime = 0f;
        float lerpDuration = 1f;
        while (elapsedTime < lerpDuration)
        {
            transform.position = Vector3.Lerp(climbBegunPosition, climbOverPosition, elapsedTime / lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void AllowLedgeGrab()
    {
        canGrabLedge = true;
    }

    public void CheckInput()
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
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"));
            rb.velocity = new Vector2((float)(movingInput * gameManager.playerStats.CharacterSpeed), rb.velocity.y);
            if (isSitting)
            {
                rb.velocity = new Vector2((float)(movingInput * gameManager.playerStats.ExtraCharacterSpeed), rb.velocity.y);
            }
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown)
        {
            if (canDash)
            {
                if (characterStats.characterStamina >= dashStaminaCost)
                {
                    characterStats.characterStamina -= dashStaminaCost;
                    fadeOut.makeFadeOut = true;
                    isDashing = true;
                    dashStartTime = Time.time;
                    lastDashTime = Time.time;
                }
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
        characterStats.characterStamina = Mathf.Clamp(characterStats.characterStamina, 0f, 100f);
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

    private void Flip()
    {
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }

    private void ClimbLadder()
    {
        if (isLadderDetected)
        {
            float verticalInput = Input.GetAxis("Vertical");
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, (float)(verticalInput * gameManager.playerStats.CharacterSpeed));
            isGrounded = false;
            canWallSlide = false;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadderDetected = true;
            isGrounded = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadderDetected = false;
        }
    }

    public void AnimatorController()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallDetected", isWallDetected);
        anim.SetBool("isClimbing", isClimbing);
        anim.SetBool("isSitting", isSitting);
    }

    private void CollisionCheck()
    {
        Vector3 offset = new Vector3(0, 1f, 0);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround | whatIsLedge);

        isWallDetected = Physics2D.Raycast(transform.position + offset, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        isLadderDetected = Physics2D.Raycast(transform.position, Vector2.up, ladderCheckdistance, whatIsLadder);
        isCeilDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilCheckDistance, whatIsCeil);

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
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + ceilCheckDistance));
    }
}
