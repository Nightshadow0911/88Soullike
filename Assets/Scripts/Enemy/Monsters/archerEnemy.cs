using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerEnemy : MonoBehaviour
{
    public Transform player;
    private Animator animator;
    public GameObject arrowPrefab;
    public float fireInterval = 2.0f;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private bool isShooting = false;

    private float moveSpeed;

    public int maxHealth = 100;
    private int currentHealth;

    public Transform selfPosition;
    public GameObject soulDrop;

    [SerializeField]
    private bool applyKnockback;

    [SerializeField]
    private float knockbackSpeedX, knockbackSpeedY, knockbackDuration;
    private float knockbackStart;
    private bool knockback;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameManager.Instance.player.transform;
        currentHealth = maxHealth;
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        Animator animator = GetComponent<Animator>();
        Vector2 direction = player.position - transform.position;
        MonsterFaceWay();

        if (Mathf.Abs(direction.y) < 5f && Mathf.Abs(direction.x) <= 20 && Mathf.Abs(direction.x) > 8) //이동 모션
        {
            if(isShooting)
            {
                moveSpeed = 0;
            }
            if(!isShooting)
            {
                moveSpeed = 0.5f;
                Vector2 moveDirection = direction.normalized;
                animator.Play("running");
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
            
        }
        else if (Mathf.Abs(direction.y) < 5 && Mathf.Abs(direction.x) >= 4 && Mathf.Abs(direction.x) <= 8) //장거리 사격
        {
            if (!isShooting)
            {
                StartCoroutine(ShootArrowInArc());
            }
        }
        else if (Mathf.Abs(direction.y) < 5f && Mathf.Abs(direction.x) < 4) //근거리 사격
        {
            if (!isShooting)
            {
                StartCoroutine(ShootStraightArrow());  
            }
        }
        else //아무것도 안할 때
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
        Vector2 spawnPosition = selfPosition.position + new Vector3(0, 2.2f);
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
        Vector2 spawnPosition = selfPosition.position + new Vector3(0, 2.2f);
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


        if (direction.x < 0) // 방향 전환 기능
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
        if (applyKnockback && currentHealth > 0)
        {
            //knockback
            Knockback();
        }
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

    public void Knockback()
    {
        isShooting = true;
        knockback = true;
        knockbackStart = Time.time;
        rb.velocity = new Vector2(knockbackSpeedX * gameManager.lastPlayerController.facingDirection, knockbackSpeedY);
        Debug.Log(knockbackSpeedX * gameManager.lastPlayerController.facingDirection);
        Debug.Log("1:" + rb.velocity);
    }
    public void CheckKnockback()
    {
        if (Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            isShooting = false;
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            Debug.Log("2:" + rb.velocity);
        }
    }
}
