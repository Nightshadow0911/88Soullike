using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Transform player;
    private Animator animator;
    public GameObject arrowPrefab;
    public float fireInterval = 2.0f;

    private bool isShooting = false;
    private float distanceToPlayer;

    private float moveSpeed;
    private MonsterState currentState;

    private enum MonsterState
    {
        Idle,
        Run,
        AttackLongRange,
        AttackDirect
    }

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        Vector2 direction = player.position - transform.position;

        if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) < 10.0f && Mathf.Abs(direction.x) > 4.0f)
        {

            moveSpeed = 0.5f;
            currentState = MonsterState.Run;

            Vector2 moveDirection = direction.normalized;

            if (moveDirection.x < 0) //방향 전환 기능
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        else if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) >= 2.0f && Mathf.Abs(direction.x) <= 4.0f)
        {
            currentState = MonsterState.AttackLongRange;

            if (!isShooting)
            {
               
                StartCoroutine(ShootArrowInArc());

            }
        }
        else if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) < 2.0f)
        {
            currentState = MonsterState.AttackDirect;
            Vector2 moveDirection = direction.normalized;

            if (moveDirection.x < 0) //방향 전환 기능
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            if (!isShooting)
            {
               
                StartCoroutine(ShootStraightArrow());


            }
        }
        else
        {
            currentState = MonsterState.Idle;
        }
        UpdateAnimationState();
    }

    IEnumerator ShootArrowInArc()
    {
        isShooting = true;
        yield return new WaitForSeconds(fireInterval);
        Vector2 direction = player.position - transform.position;

        Vector2 spawnPosition = transform.position + new Vector3(0, 0.5f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        // 화살의 Rigidbody2D 속도를 설정합니다
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * Mathf.Abs(direction.x) * 3f;

        isShooting = false;
    }

    IEnumerator ShootStraightArrow()
    {
        isShooting = true;
        yield return new WaitForSeconds(fireInterval);
        Vector2 direction = player.position - transform.position;

        Vector2 spawnPosition = transform.position + new Vector3(0, 0.5f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * Mathf.Abs(direction.x)*3f;

        isShooting = false;
    }

    void UpdateAnimationState()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                animator.SetBool("Idle", true);
                animator.SetBool("Run", false);
                animator.SetBool("AttackLongRange", false);
                animator.SetBool("AttackDirect", false);
                break;

            case MonsterState.Run:
                animator.SetBool("Idle", false);
                animator.SetBool("Run", true);
                animator.SetBool("AttackLongRange", false);
                animator.SetBool("AttackDirect", false);
                break;

            case MonsterState.AttackLongRange:
                animator.SetBool("Idle", false);
                animator.SetBool("Run", false);
                animator.SetBool("AttackLongRange", true);
                animator.SetBool("AttackDirect", false);
                break;

            case MonsterState.AttackDirect:
                animator.SetBool("Idle", false);
                animator.SetBool("Run", false);
                animator.SetBool("AttackLongRange", false);
                animator.SetBool("AttackDirect", true);
                break;
        }
    }
}
