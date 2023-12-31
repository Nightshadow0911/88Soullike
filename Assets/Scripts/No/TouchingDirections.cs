using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;
    public float ladderDistance = 0.2f;

    CapsuleCollider2D touchingCol;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    RaycastHit2D[] ladderHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField]
    private bool _isOnWall;

    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling;

    private Vector2 wallcheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    [SerializeField]
    private bool _isOnLadder;

    public bool IsOnLadder
    {
        get
        {
            return _isOnLadder;
        }
        private set
        {
            _isOnLadder = value;
            animator.SetBool(AnimationStrings.isOnLadder, value);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
     }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded =  touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance)>0;
        //Debug.Log(touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance));
        IsOnWall = touchingCol.Cast(wallcheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
        IsOnLadder = touchingCol.Cast(Vector2.up, castFilter, ladderHits, ladderDistance) > 0;

    }
}
