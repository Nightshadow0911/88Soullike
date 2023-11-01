using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeathBringerEnemy : MonoBehaviour
{
    public Transform player;
    private Animator animator;

    public GameObject meleeAttack;
    public GameObject spellAttack;
    public GameObject spellEffect;

    private float moveSpeed = 0.2f;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        Animator animator = GetComponent<Animator>();
        Vector2 direction = player.position - transform.position;

        if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) < 10.0f && Mathf.Abs(direction.x) > 0.4f) //같은 높이에 어느정도 가까운 거리면 걸어서 이동
        {
            if (isAttacking)
            {
                moveSpeed = 0;
            }
            if (!isAttacking)
            {
                moveSpeed = 0.5f;

                Vector2 moveDirection = direction.normalized;

                if (moveDirection.x < 0) // 방향 전환 기능
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                animator.Play("running");

                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }

        }

        else if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) <= 0.4f) //플레이어 공격 로직
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }

        else
        {
            if (!isAttacking)
            {
                animator.Play("idle");
            }
        }

        IEnumerator AttackPlayer() //몬스터가 공격하면 공격범위에 프리팹을 소환하고, 그 프리팹에 닿으면 플레이어에게 피해를 주도록
        {
            animator.Play("attack");
            isAttacking = true;
            yield return new WaitForSeconds(1.5f);
            Vector2 direction = player.position - transform.position;
            Vector2 moveDirection = direction.normalized;

            if (moveDirection.x < 0) // 방향 전환 기능
            {
                Vector2 spawnPosition = transform.position + new Vector3(0f, -0.2f);
                GameObject meleeAttackRange = Instantiate(meleeAttack, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(0.7f);
                Destroy(meleeAttackRange);

                animator.Play("idle");
                yield return new WaitForSeconds(1.5f);
                isAttacking = false;
            }
            else
            {
                Vector2 spawnPosition = transform.position + new Vector3(0f, -0.2f);
                GameObject meleeAttackRange = Instantiate(meleeAttack, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(0.7f);

                Destroy(meleeAttackRange);

                animator.Play("idle");
                yield return new WaitForSeconds(1.5f);
                isAttacking = false;
            }
        }
    }
}