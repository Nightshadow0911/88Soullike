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
    public GameObject soulDrop;
    

    private Animator animator;

    public GameObject meleeAttack;
    public GameObject spellAttack;
    public GameObject spellEffect;
    public GameObject magicMissile;

    private GameObject spellEffectObject;
    private GameObject spellAttackObject;
    private GameObject meleeAttackRange;
    private GameObject MagicMissile;

    private float moveSpeed = 0.5f;
    public float missileSpeed = 5f;
    private bool isAttacking = false;
    private bool isTeleport = false;

    public GameObject MiddleBoss;
    private int spellCount; //스펠 사용 횟수
    private int maxSpellCount = 3; //최대 스펠 사용 횟수

    public int maxHealth;
    [SerializeField]
    private int currentHealth;

    public CharacterStats characterStats;

    void Start()
    {
        animator = GetComponent<Animator>();
        //player = GameManager.Instance.player.transform;
        currentHealth = maxHealth;
    }

    void Update()
    {
        Vector2 direction = player.position - selfPosition.position;
        Vector2 moveDirection = direction.normalized;
        // Debug.Log(direction.x);

        

        if (Mathf.Abs(direction.x) <= 4f) //근접 평타 액션
        {
            if (!isAttacking && !isTeleport)
            {
                StartCoroutine(AttackPlayer());
            }
        }

        else if (Mathf.Abs(direction.x) >= 10f && Mathf.Abs(direction.x) <= 20f) //원거리 주문 공격
        {
            if (isAttacking)
            {
                moveSpeed = 0;
            }
            if (!isAttacking && !isTeleport)
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

        else if (Mathf.Abs(direction.x) <= 30f) //이동
        {
            if (isAttacking) //공격중일 때 이동 불가
            {
                moveSpeed = 0;
            }
            else if (!isTeleport) //공격, 텔레포트를 사용중이지 않을 때 이동
            {
                moveSpeed = 0.5f;

                MonsterFaceWay();

                animator.Play("running");

                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
        }

        IEnumerator AttackPlayer() //몬스터가 공격하면 공격범위에 프리팹을 소환하고, 그 프리팹에 닿으면 플레이어에게 피해를 주도록
        {
            animator.Play("attack");
            isAttacking = true;
            Vector2 spawnPosition;
            
            if (direction.x < 0)
            {
                spawnPosition = selfPosition.position + new Vector3(-2.6f, 3.5f);
                meleeAttackRange = Instantiate(meleeAttack, spawnPosition, Quaternion.identity);
                meleeAttack.transform.localScale = new Vector3(5, 5, 1);
            }
            else
            {
                spawnPosition = selfPosition.position + new Vector3(2.6f, 3.5f);
                meleeAttackRange = Instantiate(meleeAttack, spawnPosition, Quaternion.identity);
                meleeAttack.transform.localScale = new Vector3(-5, 5, 1);
            }
            meleeAttackRange.SetActive(false); //생성된 공격 범위를 비활성화
            yield return YieldCache.WaitForSeconds(1.5f);
            meleeAttackRange.SetActive(true); //생성된 공격 범위를 활성화
            yield return YieldCache.WaitForSeconds(0.3f);
            Destroy(meleeAttackRange);
            animator.Play("idle");
            yield return YieldCache.WaitForSeconds(1.5f);
            isAttacking = false;
        }

        IEnumerator UseSpell()
        {
            int rate = Random.Range(1, 10);
            if (rate <= 6)
            {
                Vector2 spellPoint = player.position + new Vector3(0f, 3.5f);
                animator.Play("cast");
                isAttacking = true;
                yield return YieldCache.WaitForSeconds(1.0f);
                animator.Play("idle");
                spellEffectObject = Instantiate(spellEffect, spellPoint, Quaternion.identity); //플레이어 위치에 스펠 이펙트 생성
                spellAttackObject = Instantiate(spellAttack, spellPoint, Quaternion.identity); //플레이어 위치에 스펠 범위 생성
                spellAttackObject.SetActive(false); //생성된 공격 범위를 비활성화
                yield return YieldCache.WaitForSeconds(1.4f);
                spellAttackObject.SetActive(true); //시전시간 이후 활성화
                yield return YieldCache.WaitForSeconds(0.6f);
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
            isTeleport = true;
            moveSpeed = 0;
            animator.Play("disappear");
            yield return YieldCache.WaitForSeconds(1f);
            if (direction.x < 0) // 방향 전환 기능
            {
                Vector2 playernear = player.position + new Vector3(2f, 0f);
                transform.position = playernear;
            }
            else
            {  
                Vector2 playernear = player.position + new Vector3(-2f, 0f);
                transform.position = playernear;
            }
            animator.Play("appear");
            spellCount = 0;
            moveSpeed = 0.5f;
            yield return YieldCache.WaitForSeconds(1f);
            isTeleport = false;
        }

        IEnumerator WideAreaAttack()
        {            
            animator.Play("cast");
            
            isAttacking = true;
            yield return YieldCache.WaitForSeconds(1.0f);
            animator.Play("idle");

            List<GameObject> spellEffectObjects = new List<GameObject>();
            List<GameObject> spellAttackObjects = new List<GameObject>();

            for (int i = -5; i < 6; i++)
            {
                Vector2 spellPoint = mapSpellMarker.position + new Vector3(4.5f * i, -3f);
                spellEffectObject = Instantiate(spellEffect, spellPoint, Quaternion.identity); //광역으로 생성되도록 수정해야함
                spellAttackObject = Instantiate(spellAttack, spellPoint, Quaternion.identity); //플레이어 위치에 스펠 범위 생성
                spellAttackObject.SetActive(false); //생성된 공격 범위를 비활성화

                spellEffectObjects.Add(spellEffectObject);
                spellAttackObjects.Add(spellAttackObject);
            }
            yield return YieldCache.WaitForSeconds(1.4f);
            foreach (var spellAttackObject in spellAttackObjects)
            {
                spellAttackObject.SetActive(true);
            }
            yield return YieldCache.WaitForSeconds(0.6f);
            foreach (var spellEffectObject in spellEffectObjects)
            {
                Destroy(spellEffectObject);
            }
            foreach (var spellAttackObject in spellAttackObjects)
            {
                Destroy(spellAttackObject);
            }
            isAttacking = false;
            spellCount++;
        }

        void MissleAttack()
        {
            int skillNumber = Random.Range(0, 2);
            if(skillNumber == 0)
            {
                StartCoroutine(MissileAttacks(2));
            }
            if(skillNumber == 1)
            {
                StartCoroutine(MissileAttacks(0));
            }
            if(skillNumber == 2)
            {
                StartCoroutine(MissileAttacks(4));
            }
        }

        IEnumerator MissileAttacks(int firePointHeight) //미완성.
        {
            isAttacking = true;
            Vector2 firePoint = selfPosition.position;
            float moveDuration = 1f;
            float elapsedTime = 0f;
            GameObject MagicMissile = Instantiate(magicMissile, selfPosition.position, Quaternion.identity);
            Vector2 targetPosition = MagicMissile.transform.position + new Vector3(0, firePointHeight, 0);
            Vector2 initialPosition = MagicMissile.transform.position;
            while (elapsedTime < moveDuration)
            {
                MagicMissile.transform.position = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            MagicMissile.transform.position = targetPosition;
            isAttacking = false;
        }

        void MonsterFaceWay()
        {

            if (direction.x < 0) // 방향 전환 기능
            {
                transform.localScale = new Vector3(8, 8, 1);
            }
            else
            {
                transform.localScale = new Vector3(-8, 8, 1);
            }
        }
    }

    public void TakeDamage(int attackDamage) // playerToMonster NO Stats
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
        animator.Play("disappear");
        yield return YieldCache.WaitForSeconds(1f);

        Vector2 SelfPosition = selfPosition.position + new Vector3(0,1);
        SoulObjectPool objectPool = FindObjectOfType<SoulObjectPool>();
        foreach (var pool in objectPool.pools)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = objectPool.SpawnFromPool(pool.tag);

                if (obj != null)
                {
                    obj.transform.position = SelfPosition;
                    obj.SetActive(true);
                }
            }
        }
        gameObject.SetActive(false);        
    }
}