using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections))]

public class PlaterController2 : MonoBehaviour
{
    public float walkSpeed = 400f;
    private float jumpImpulse = 20f;
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
    //질문
    //[SerializeField]
    //private bool _isMoving = false;

    //public bool IsMoving
    //{
    //    get
    //    {if(IsMoving && !touchingDirections.IsOnWall)
    //      {
    //          return _isMoving;
    //      }
    //        else
    //      {
    //          return false;
    //      }
    //    }
    //    private set
    //    {
    //        _isMoving = value;
    //        animator.SetBool(AnimationStrings.isMoving, value);
    //    }
    //}
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

    Rigidbody2D rb;
    Animator animator;


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
        rb.velocity = new Vector2(moveInput.x * walkSpeed * Time.fixedDeltaTime, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
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
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
}
