using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeathBringerEnemy : MonoBehaviour
{
    public Transform player;
    private Animator animator;

    public GameObject meleeAttack;
    public GameObject spellAttack;
    public GameObject spellEffect;

    private float moveSpeed = 0.5f;
    private bool isAttacking = false;

    public GameObject MiddleBoss;
    private int spellCount; //스펠 사용 횟수
    private int maxSpellCount = 1; //최대 스펠 사용 횟수


    void Start()
    {
        animator = GetComponent<Animator>();

        player = GameManager.Instance.player.transform;
    }

    
    void Update()
    {
        Animator animator = GetComponent<Animator>();
        Vector2 direction = player.position - transform.position;

        if (Mathf.Abs(direction.y) < 4f && Mathf.Abs(direction.x) > 1.5f) //비슷한 높이에 어느정도 가까운 거리면 걸어서 이동, y축값 수정 필요.
        {
            if (isAttacking)
            {
                moveSpeed = 0;
            }
            if (!isAttacking)
            {
                Vector2 moveDirection = direction.normalized;

                moveSpeed = 0.5f;

                MonsterFaceWay();

                animator.Play("running");

                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
        }

        else if (Mathf.Abs(direction.y) < 4f && Mathf.Abs(direction.x) <= 2f) //근접 평타 액션
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }

        else if (Mathf.Abs(direction.y) >= 5f) //원거리 공격 이후 순간이동 모션
        {

            if (isAttacking)
            {
                moveSpeed = 0;
            }
            if (!isAttacking)
            { 
                if(spellCount < maxSpellCount) //3번까지 플레이어에게 원거리 주문 공격
                {
                    StartCoroutine(UseSpell());
                }
                else //이후 플레이어에게 순간 이동
                {
                    StartCoroutine(MoveToPlayerDisappear());
                }

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

        IEnumerator UseSpell()
        {
            Vector2 spellPoint = player.position + new Vector3(0f, 3.5f);
            animator.Play("cast");
            isAttacking = true;
            yield return new WaitForSeconds(1.0f);
            animator.Play("idle");
            Instantiate(spellEffect, spellPoint, Quaternion.identity); //플레이어 위치에 스펠 이펙트 생성
            GameObject spellAttackObject = Instantiate(spellAttack, spellPoint, Quaternion.identity); //플레이어 위치에 스펠 범위 생성
            spellAttackObject.SetActive(false); //생성된 공격 범위를 비활성화
            yield return new WaitForSeconds(1.2f);
            spellAttackObject.SetActive(true); //시전시간 이후 활성화
            yield return new WaitForSeconds(0.8f);
            spellCount++;
            Destroy(spellEffect);

            isAttacking = false;
        }

        IEnumerator MoveToPlayerDisappear() //몬스터를 플레이어 근처로 순간이동
        {
            Vector2 moveDirection = direction.normalized;


            if (moveDirection.x < 0) // 방향 전환 기능
            {


                moveSpeed = 0;
                
                animator.Play("disappear");
                yield return new WaitForSeconds(1f);
                animator.Play("appear");

                Vector2 playernear = player.position + new Vector3(2f, 4.5f);
                transform.position = playernear;

                spellCount = 0;
                moveSpeed = 0.5f;
            }
            else
            {
                moveSpeed = 0;
                animator.Play("disappear");
                yield return new WaitForSeconds(1f);
                animator.Play("appear");

                Vector2 playernear = player.position + new Vector3(-2f, 4.5f);
                transform.position = playernear;

                spellCount = 0;
                moveSpeed = 0.5f;
            }       
        }
        

        void MonsterFaceWay()
        {
            Vector2 moveDirection = direction.normalized;

            if (moveDirection.x < 0) // 방향 전환 기능
            {
                transform.localScale = new Vector3(8, 8, 1);
            }
            else
            {
                transform.localScale = new Vector3(-8, 8, 1);
            }
        }
    }
}