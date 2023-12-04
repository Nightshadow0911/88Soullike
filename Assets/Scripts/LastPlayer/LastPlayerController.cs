using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerController : MonoBehaviour
{
    private PlayerStatusHandler playerStatusHandler;
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerAttack playerAttack;

    public FadeOut fadeOut;
    public CharacterStats characterStats;
    public LedgeCheck ledgeCheck;

    public PlayerUI playerUI;
    [SerializeField] private float jumpForce = 10;

    private bool canMove = true;

    private bool canWallSlide;
    internal bool isWallSliding;

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

    public int skillIndex = 0;

    private bool canPressS = true;
    private float pressCooldown = 2f;
    void Start()
    {

    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerStatusHandler = GetComponent<PlayerStatusHandler>();
    }
    void Update()
    {
        CheckInput();
        FastDown();
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

        if (Input.GetKeyDown(KeyCode.G)) UseSkill();
    }


    private IEnumerator EnablePressAfterCooldown()
    {
        yield return new WaitForSeconds(pressCooldown);
        canPressS = true;
    }

    void UseSkill()
    {

        transform.GetComponent<Equipment>().skillSlotList[skillIndex].Use();


    }
    public void ChangeSkill()
    {
        skillIndex++;
        if (skillIndex >= 3) skillIndex = 0;
        transform.GetComponent<Equipment>().ChageEquipSkill();

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
            rb.velocity = new Vector2(movingInput * playerStatusHandler.GetStat().speed, rb.velocity.y);
            if (isSitting)
            {
                rb.velocity = new Vector2(movingInput * playerStatusHandler.GetStat().speed, rb.velocity.y);
                Debug.Log(playerStatusHandler.GetStat().speed);
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

    private void FastDown()
    {
        if (!isGrounded)
        {
            if (canPressS && Input.GetKeyDown(KeyCode.S))
            {
                if (rb.velocity.y<0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -Mathf.Abs(rb.velocity.y *10f));
                    canPressS = false;
                    StartCoroutine(EnablePressAfterCooldown());
                    Debug.Log("FastDown");
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, -Mathf.Abs(rb.velocity.y * -10f));
                    canPressS = false;
                    StartCoroutine(EnablePressAfterCooldown());
                    Debug.Log("FastDown2");
                }
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            }
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
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * playerStatusHandler.GetStat().speed);
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
