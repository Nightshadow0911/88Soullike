using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyArcherControl : MonoBehaviour
{
    public float moveSpeed = 0.5f; // 몬스터의 이동속도
    public float attackLongRange = 4.0f; // 곡사 공격 사거리
    public float attackDirectRange = 2.0f; // 직사 공격 사거리
    public Transform player;
    private Animator animator;
    private enum MonsterState { Idle, Run, AttackLongRange, AttackDirect };
    private MonsterState currentState = MonsterState.Idle;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = player.position - transform.position;

            if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) < 10.0f && Mathf.Abs(direction.x) > 4.0f) //y축으로 0.5이하, x축으로 4이상 8이하 만큼 떨어져있을때 몬스터는 플레이어를 인식한다.
            {
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
            else if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) >= 2.0f && Mathf.Abs(direction.x) <= 4.0f) //y축으로 0.5이하, x축으로 2이상 4미만 만큼 떨어져있을때 몬스터는 플레이어를 곡사로 공격한다.
            {
                currentState = MonsterState.AttackLongRange;
            }
            else if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) < 2.0f) //y축으로 0.5이하, x축으로 2미만으로 떨어져있을때 몬스터는 플레이어를 직사로 공격한다.
            {
                currentState = MonsterState.AttackDirect;
            }
            else
            {
                currentState = MonsterState.Idle;
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