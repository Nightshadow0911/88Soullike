using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class skeletonEnemy : MonoBehaviour
{
    public Transform player;
    private Animator animator;
    public GameObject skeletonWeapon;
    public int maxHealth = 100;
    private int currentHealth;

    private float moveSpeed = 0.2f;
    private bool isAttacking = false;

    public Transform selfPosition;
    public GameObject soulDrop;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.Instance.player.transform;
        currentHealth = maxHealth;
    }

    void Update()
    {
        Animator animator = GetComponent<Animator>();
        Vector2 direction = player.position - transform.position;

        if (Mathf.Abs(direction.y) < 3f && Mathf.Abs(direction.x) < 15.0f && Mathf.Abs(direction.x) > 3f) //움직이는 로직
        {
            if (isAttacking)
            {
                moveSpeed = 0;
            }
            if (!isAttacking)
            {
                moveSpeed = 0.5f;

                Vector2 moveDirection = direction.normalized;

                MonsterFaceWay();

                animator.Play("running");

                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }

        }
        else if (Mathf.Abs(direction.y) < 3f && Mathf.Abs(direction.x) <= 3f)
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
    }

    IEnumerator AttackPlayer() //몬스터가 공격하면 공격범위에 프리팹을 소환하고, 그 프리팹에 닿으면 플레이어에게 피해를 주도록
    {
        
        Vector2 direction = player.position - transform.position;
        Vector2 moveDirection = direction.normalized;
        
        if (moveDirection.x < 0) // 방향 전환 기능
        {
            MonsterFaceWay();
            animator.Play("Attack");
            isAttacking = true;
            

            Vector2 spawnPosition = transform.position + new Vector3(-1.7f, 2f);
            GameObject skeletonSword = Instantiate(skeletonWeapon, spawnPosition, Quaternion.identity);
           skeletonSword.SetActive(false); //생성된 공격 범위를 비활성화
            yield return YieldCache.WaitForSeconds(0.8f);
            skeletonSword.SetActive(true);
            yield return YieldCache.WaitForSeconds(0.2f);
            Destroy(skeletonSword);
            if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) <= 0.5f) //여전히 플레이어가 공격 범위 내이면 2차 공격
            {
                StartCoroutine(SecondAttackPlayer());
                
            }
            else
            {
                animator.Play("idle");
                yield return YieldCache.WaitForSeconds(1.5f);
                isAttacking = false;
            }
        }
        else
        {
            MonsterFaceWay();
            animator.Play("Attack");
            isAttacking = true;
            
            Vector2 spawnPosition = transform.position + new Vector3(1.7f, 2f);
            GameObject skeletonSword = Instantiate(skeletonWeapon, spawnPosition, Quaternion.identity);
            skeletonSword.SetActive(false); //생성된 공격 범위를 비활성화
            yield return YieldCache.WaitForSeconds(0.8f);
            skeletonSword.SetActive(true);
            yield return YieldCache.WaitForSeconds(0.2f);
            Destroy(skeletonSword);
            if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) <= 0.5f) //여전히 플레이어가 공격 범위 내이면 2차 공격
            {
                StartCoroutine(SecondAttackPlayer());
                
            }
            else
            {
                animator.Play("idle");
                yield return new WaitForSeconds(1.5f);
                isAttacking = false;

            }
        }
    }

    IEnumerator SecondAttackPlayer() //연계 공격
    {
        animator.Play("SecondAttack");
        isAttacking = true;
        yield return new WaitForSeconds(0.8f);
        Vector2 direction = player.position - transform.position;
        Vector2 moveDirection = direction.normalized;

        if (moveDirection.x < 0) // 방향 전환 기능
        {
            Vector2 spawnPosition = transform.position + new Vector3(-0.14f, 0f);
            GameObject skeletonSword = Instantiate(skeletonWeapon, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Destroy(skeletonSword);
            animator.Play("idle");
            yield return new WaitForSeconds(1.5f);
            isAttacking = false;
        }
        else
        {
            Vector2 spawnPosition = transform.position + new Vector3(0.14f, 0f);
            GameObject skeletonSword = Instantiate(skeletonWeapon, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Destroy(skeletonSword);
            animator.Play("idle");
            yield return new WaitForSeconds(1.5f);
            isAttacking = false;
        }
    }

    void MonsterFaceWay()
    {
        Vector2 direction = player.position - transform.position;
        Vector2 moveDirection = direction.normalized;

        if (moveDirection.x < 0) // 방향 전환 기능
        {
            transform.localScale = new Vector3(-1.3f, 1.3f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1);
        }
    }

    public void TakeDamage(int attackDamage)
    {
        currentHealth -= attackDamage;

        if (currentHealth <= 0)
        {
            StartCoroutine(Death());
        }
    }
    IEnumerator Death()
    {
        isAttacking = true;
        animator.Play("death");
        yield return new WaitForSeconds(1f);
        Vector2 SelfPosition = selfPosition.position + new Vector3(0, 1);
        Instantiate(soulDrop, SelfPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}
