using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemyArcherControl : MonoBehaviour
{
    public float moveSpeed = 0.5f; // 몬스터의 이동속도
    public float attackLongRange = 4.0f; // 곡사 공격 사거리
    public float attackDirectRange = 2.0f; // 직사 공격 사거리
    public Transform player;
    private Animator animator;

    public GameObject arrowPrefab;
    public float arrowSpeed = 5.0f;

    private MonsterState currentState;
    private bool isShooting = false;
    private bool isAttackCooldown = false;

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
        
        if (player != null)
        {
            Vector2 direction = player.position - transform.position;

            if (isShooting) //활 쏠때 정지
            {
                moveSpeed = 0;
                UpdateAnimationState();
            }

            else if (!isShooting && !isAttackCooldown)
            {
                if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) < 10.0f && Mathf.Abs(direction.x) > 4.0f) //플레이어 인식
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

                else if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) >= 2.0f && Mathf.Abs(direction.x) <= 4.0f) //곡사 공격
                {
                    if (!isAttackCooldown) //쿨다운이 돌면 다시 공격가능
                    {
                        currentState = MonsterState.AttackLongRange;
                        Vector2 moveDirection = direction.normalized;

                        if (moveDirection.x < 0) //방향 전환 기능
                        {
                            transform.localScale = new Vector3(-1, 1, 1);
                        }
                        else
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                        }
                        isShooting = true;

                        Invoke("FireArrow",2.0f);
                    }

                }
             
                else if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) < 2.0f) //직사 공격
                {
                    if (!isAttackCooldown) //쿨다운이 돌면 다시 공격가능
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
                        UpdateAnimationState();
                        isShooting = true;

                        Invoke("FireArrow", 2.0f);
                    }

                    else
                    {
                        currentState = MonsterState.Idle;
                    }
                }
                UpdateAnimationState();
            }            
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

    public void FireArrow()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

        Vector2 direction = player.position - transform.position;

        if (Mathf.Abs(direction.x) >= 4.0f)
        {
            // 화살을 포물선으로 발사합니다
            // 중력 및 초기 속도 및 각도를 조정합니다

            float gravity = 9.81f; // 중력 가속도 (9.81 m/s^2)
            float timeToReachPlayer = Mathf.Sqrt((2 * direction.magnitude) / gravity);

            // 초기 속도를 계산하여 각도와 방향으로 분리합니다
            float initialVelocityX = direction.x / timeToReachPlayer;
            float initialVelocityY = direction.y / timeToReachPlayer - 0.5f * gravity * timeToReachPlayer;

            // 둥글게 그리도록 중력의 영향을 약하게 합니다
            initialVelocityY *= 0.1f;

            // 화살의 Rigidbody2D 속도를 설정합니다
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(initialVelocityX, initialVelocityY);
        }
        else
        {
            // 화살을 직선으로 발사합니다
            // 화살의 Rigidbody2D 속도를 플레이어를 향해 직접 설정합니다
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            rb.velocity = direction.normalized * arrowSpeed;
        }
    }
}