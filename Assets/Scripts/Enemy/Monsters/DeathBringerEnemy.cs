using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeathBringerEnemy : MonoBehaviour
{
    public Transform player;
    public Transform mapSpellMarker; //중간보스방의 중앙을 인식할 좌표 오브젝트
    public Transform selfPosition;
    private Animator animator;

    public GameObject meleeAttack;
    public GameObject spellAttack;
    public GameObject spellEffect;
    public GameObject soulDrop;
    private GameObject spellEffectObject;
    private GameObject spellAttackObject;
    
    private float moveSpeed = 0.5f;
    private bool isAttacking = false;

    public GameObject MiddleBoss;
    private int spellCount; //스펠 사용 횟수
    private int maxSpellCount = 3; //최대 스펠 사용 횟수

    public int maxHealth = 800;
    private int currentHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        //player = GameManager.Instance.player.transform;
        currentHealth = maxHealth;
        Debug.Log(currentHealth);
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
                    StartCoroutine(MoveToPlayer());
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
            int rate = Random.Range(1, 10);
            if (rate <= 8)
            {
                
                Vector2 spellPoint = player.position + new Vector3(0f, 3.5f);
                animator.Play("cast");
                isAttacking = true;
                yield return new WaitForSeconds(1.0f);
                animator.Play("idle");
                spellEffectObject = Instantiate(spellEffect, spellPoint, Quaternion.identity); //플레이어 위치에 스펠 이펙트 생성
                spellAttackObject = Instantiate(spellAttack, spellPoint, Quaternion.identity); //플레이어 위치에 스펠 범위 생성
                spellAttackObject.SetActive(false); //생성된 공격 범위를 비활성화
                yield return new WaitForSeconds(1.2f);
                spellAttackObject.SetActive(true); //시전시간 이후 활성화
                yield return new WaitForSeconds(0.8f);
                spellCount++;
                Destroy(spellEffectObject);
                Destroy(spellAttackObject);
                
                isAttacking = false;
            }
            
            else //20퍼센트 확률로 광역기 시전
            {
                StartCoroutine(WideAreaAttack());
            }
        }

        IEnumerator MoveToPlayer() //몬스터를 플레이어 근처로 순간이동
        {
            Vector2 moveDirection = direction.normalized;

            if (moveDirection.x < 0) // 방향 전환 기능
            {
                moveSpeed = 0;                
                animator.Play("disappear");
                yield return new WaitForSeconds(1f);
                
                Vector2 playernear = player.position + new Vector3(2f, 4.5f);
                transform.position = playernear;

                animator.Play("appear");
                spellCount = 0;
                moveSpeed = 0.5f;
                
            }

            else
            {
                moveSpeed = 0;
                animator.Play("disappear");
                yield return new WaitForSeconds(1f);
                
                Vector2 playernear = player.position + new Vector3(-2f, 4.5f);
                transform.position = playernear;

                animator.Play("appear");
                spellCount = 0;
                moveSpeed = 0.5f;
            }       
        }

        IEnumerator WideAreaAttack()
        {            
            animator.Play("cast");
            
            isAttacking = true;
            yield return new WaitForSeconds(1.0f);
            animator.Play("idle");

            List<GameObject> spellEffectObjects = new List<GameObject>();
            List<GameObject> spellAttackObjects = new List<GameObject>();

            for (int i = -5; i < 6; i++)
            {
                Vector2 spellPoint = mapSpellMarker.position + new Vector3(4 * i, 0);
                spellEffectObject = Instantiate(spellEffect, spellPoint, Quaternion.identity); //광역으로 생성되도록 수정해야함
                spellAttackObject = Instantiate(spellAttack, spellPoint, Quaternion.identity); //플레이어 위치에 스펠 범위 생성
                spellAttackObject.SetActive(false); //생성된 공격 범위를 비활성화

                spellEffectObjects.Add(spellEffectObject);
                spellAttackObjects.Add(spellAttackObject);
            }
            yield return new WaitForSeconds(1.2f);
            foreach (var spellAttackObject in spellAttackObjects)
            {
                spellAttackObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.8f);
            foreach (var spellEffectObject in spellEffectObjects)
            {
                Destroy(spellEffectObject);
            }
            foreach (var spellAttackObject in spellAttackObjects)
            {
                Destroy(spellAttackObject);
            }
            isAttacking = false;
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
    public void TakeDamage(int attackDamage)
    {
        currentHealth -= attackDamage;

        if (currentHealth <= 0)
        {
            Death();
        }
    }
    void Death()
    {
        Vector2 SelfPosition = selfPosition.position + new Vector3(0,1);
        Instantiate(soulDrop, SelfPosition, Quaternion.identity);
        Destroy(gameObject);
    }
}