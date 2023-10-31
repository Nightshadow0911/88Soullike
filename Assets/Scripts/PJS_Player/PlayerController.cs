using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections))]

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 10f;
    private float jumpImpulse = 4f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;



    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    return walkSpeed;
                }
                else
                {
                    //idle speed 0 
                    return 0;
                }
            }
            else
            {
                //움직임 잠금 
                return 0;
            }
        }
    }
    [SerializeField]
    private bool _isJumping = false;

    public bool IsJumping
    {
        get
        {
            return _isJumping;
        }
        private set
        {
            _isJumping = value;
        }
    }

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving {
        get
        {
                return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }


    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 40f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    //Dash
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed * Time.fixedDeltaTime, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        //if (touchingDirections.IsGrounded)
        //{
        //    IsJumping = false; // 점프가 끝난 경우 점프 중인 상태 해제
        //    animator.SetBool(AnimationStrings.roll, false);
        //    Debug.Log("1");
        //}
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }
    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x>0&& !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x<0&& IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            //animator.SetBool(AnimationStrings.roll,true);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            Debug.Log("3");
            IsJumping = true;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
