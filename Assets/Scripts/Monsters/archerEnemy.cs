using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Transform player;
    private Animator animator;
    public GameObject arrowPrefab;
    public float fireInterval = 2.0f;

    private bool isShooting = false;

    private float moveSpeed;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Animator animator = GetComponent<Animator>();
        Vector2 direction = player.position - transform.position;

        if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) < 10.0f && Mathf.Abs(direction.x) > 4.0f)
        {
            if(isShooting)
            {
                Vector2 moveDirection = direction.normalized;

                if (moveDirection.x < 0) // 방향 전환 기능
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                moveSpeed = 0;
            }
            if(!isShooting)
            {
                moveSpeed = 0.5f;

                Vector2 moveDirection = direction.normalized;

                if (moveDirection.x < 0) // 방향 전환 기능
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                animator.Play("running");

                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
            
        }
        else if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) >= 2.0f && Mathf.Abs(direction.x) <= 4.0f)
        {
            if (!isShooting)
            {
                Vector2 moveDirection = direction.normalized;

                if (moveDirection.x < 0) // 방향 전환 기능
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                StartCoroutine(ShootArrowInArc());
                
            }
        }
        else if (Mathf.Abs(direction.y) < 0.5f && Mathf.Abs(direction.x) < 2.0f)
        {
            if (!isShooting)
            {
                Vector2 moveDirection = direction.normalized;

                if (moveDirection.x < 0) // 방향 전환 기능
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
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
        
        yield return new WaitForSeconds(2.0f);
        Vector2 direction = player.position - transform.position;
        Vector2 spawnPosition = transform.position + new Vector3(0, 0.4f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * 7f;
        animator.Play("Idle");
        yield return new WaitForSeconds(1.5f);
        isShooting = false;

    }

    IEnumerator ShootStraightArrow()
    {
        animator.Play("DirectAttack");

        isShooting = true;
       
        yield return new WaitForSeconds(2.0f);
        Vector2 direction = player.position - transform.position;
        Vector2 spawnPosition = transform.position + new Vector3(0, 0.4f);
        GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction.normalized.x, 0) * 10f;
        animator.Play("Idle");
        yield return new WaitForSeconds(1.5f);
        isShooting = false;
    }
}
