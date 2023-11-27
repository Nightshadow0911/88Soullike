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
    private Rigidbody2D rb;
    private float moveSpeed = 0.2f;
    private bool isAttacking = false;
    private GameManager gameManager;
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

        if (Mathf.Abs(direction.y) < 3f && Mathf.Abs(direction.x) < 15.0f && Mathf.Abs(direction.x) > 3f) //�����̴� ����
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
        CheckKnockback();
    }

    IEnumerator AttackPlayer() //���Ͱ� �����ϸ� ���ݹ����� �������� ��ȯ�ϰ�, �� �����տ� ������ �÷��̾�� ���ظ� �ֵ���
    {

        Vector2 direction = player.position - transform.position;
        Vector2 moveDirection = direction.normalized;

        if (moveDirection.x < 0) // ���� ��ȯ ���
        {
            MonsterFaceWay();
            animator.Play("Attack");
            isAttacking = true;


            Vector2 spawnPosition = transform.position + new Vector3(-1.7f, 2f);
            GameObject skeletonSword = Instantiate(skeletonWeapon, spawnPosition, Quaternion.identity);
            skeletonSword.SetActive(false); //������ ���� ������ ��Ȱ��ȭ
            yield return YieldCache.WaitForSeconds(0.8f);
            skeletonSword.SetActive(true);
            yield return YieldCache.WaitForSeconds(0.2f);
            Destroy(skeletonSword);
            if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) <= 0.5f) //������ �÷��̾ ���� ���� ���̸� 2�� ����
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
            skeletonSword.SetActive(false); //������ ���� ������ ��Ȱ��ȭ
            yield return YieldCache.WaitForSeconds(0.8f);
            skeletonSword.SetActive(true);
            yield return YieldCache.WaitForSeconds(0.2f);
            Destroy(skeletonSword);
            if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) <= 0.5f) //������ �÷��̾ ���� ���� ���̸� 2�� ����
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
    }

    IEnumerator SecondAttackPlayer() //���� ����
    {
        animator.Play("SecondAttack");
        isAttacking = true;
        yield return YieldCache.WaitForSeconds(0.8f);
        Vector2 direction = player.position - transform.position;
        Vector2 moveDirection = direction.normalized;

        if (moveDirection.x < 0) // ���� ��ȯ ���
        {
            Vector2 spawnPosition = transform.position + new Vector3(-0.14f, 0f);
            GameObject skeletonSword = Instantiate(skeletonWeapon, spawnPosition, Quaternion.identity);
            yield return YieldCache.WaitForSeconds(0.2f);
            Destroy(skeletonSword);
            animator.Play("idle");
            yield return YieldCache.WaitForSeconds(1.5f);
            isAttacking = false;
        }
        else
        {
            Vector2 spawnPosition = transform.position + new Vector3(0.14f, 0f);
            GameObject skeletonSword = Instantiate(skeletonWeapon, spawnPosition, Quaternion.identity);
            yield return YieldCache.WaitForSeconds(0.2f);
            Destroy(skeletonSword);
            animator.Play("idle");
            yield return YieldCache.WaitForSeconds(1.5f);
            isAttacking = false;
        }
    }

    void MonsterFaceWay()
    {
        Vector2 direction = player.position - transform.position;
        Vector2 moveDirection = direction.normalized;

        if (moveDirection.x < 0) // ���� ��ȯ ���
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
        isAttacking = true;
        animator.Play("death");
        yield return YieldCache.WaitForSeconds(0.9f);
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
        isAttacking = true;
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
            isAttacking = false;
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            Debug.Log("2:" + rb.velocity);
        }
    }

}
