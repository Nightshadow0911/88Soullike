using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerEnemy : MonoBehaviour
{
    public Transform player;
    private Animator animator;
    public GameObject arrowPrefab;
    public float fireInterval = 2.0f;

    private bool isShooting = false;

    private float moveSpeed;

    public int maxHealth = 100;
    private int currentHealth;

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

        if (Mathf.Abs(direction.y) < 3f && Mathf.Abs(direction.x) < 15.0f && Mathf.Abs(direction.x) > 8.0f)
        {
            if(isShooting)
            {
                Vector2 moveDirection = direction.normalized;

                MonsterFaceWay();

                moveSpeed = 0;
            }
            if(!isShooting)
            {
                moveSpeed = 0.5f;

                Vector2 moveDirection = direction.normalized;

                MonsterFaceWay();

                animator.Play("running");

                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
            
        }
        else if (Mathf.Abs(direction.y) < 3f && Mathf.Abs(direction.x) >= 4.0f && Mathf.Abs(direction.x) <= 8.0f)
        {
            if (!isShooting)
            {
                Vector2 moveDirection = direction.normalized;

                MonsterFaceWay();

                StartCoroutine(ShootArrowInArc());
                
            }
        }
        else if (Mathf.Abs(direction.y) < 3f && Mathf.Abs(direction.x) < 4.0f)
        {
            if (!isShooting)
            {
                Vector2 moveDirection = direction.normalized;

                MonsterFaceWay();

                StartCoroutine(ShootStraightArrow());
               
            }
        }
        else
        {
            if (!isShooting)
            {
                animator.Play("Idle");
            }
        }
    }

    IEnumerator ShootArrowInArc()
    {
        animator.Play("parabolicAttack");

        isShooting = true;

        yield return YieldCache.WaitForSeconds(2.0f);
        Vector2 direction = player.position - transform.position;
        Vector2 spawnPosition = transform.position + new Vector3(0, 0.4f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * 7f;
        animator.Play("Idle");
        yield return YieldCache.WaitForSeconds(1.5f);
        isShooting = false;
    }

    IEnumerator ShootStraightArrow()
    {
        animator.Play("DirectAttack");

        isShooting = true;

        yield return YieldCache.WaitForSeconds(2.0f);
        Vector2 direction = player.position - transform.position;
        Vector2 spawnPosition = transform.position + new Vector3(0, 0.4f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction.normalized.x, 0) * 10f;
        animator.Play("Idle");
        yield return YieldCache.WaitForSeconds(1.5f);
        isShooting = false;
    }
    void MonsterFaceWay()
    {
        Vector2 direction = player.position - transform.position;
        Vector2 moveDirection = direction.normalized;

        if (moveDirection.x < 0) // 방향 전환 기능
        {
            transform.localScale = new Vector3(-1f, 1f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1);
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
        animator.Play("death");
        yield return YieldCache.WaitForSeconds(1f);
        Vector2 SelfPosition = selfPosition.position + new Vector3(0, 1);
        SoulObjectPool objectPool = FindObjectOfType<SoulObjectPool>();
        foreach (var pool in objectPool.pools)
        {
            GameObject obj = objectPool.SpawnFromPool(pool.tag);

            if (obj != null)
            {
                obj.transform.position = SelfPosition;
                obj.SetActive(true);
            }
        }
        Destroy(gameObject);
    }
}
