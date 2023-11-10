using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask baseCollision;
    private RangedAttackData attackData;
    private Vector2 direction;
    private float currentDuration;
    private bool isReady;

    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isReady)
            return;
        
        currentDuration += Time.deltaTime;

        if (currentDuration > attackData.duration) {
            DestroyProjectile(transform.position);
        }
        rigid.velocity = direction * attackData.speed;
    }

    public void InitializeAttack(Vector2 direction, RangedAttackData attackData)
    {
        this.attackData = attackData;
        this.direction = direction;
        currentDuration = 0;
        isReady = true;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (baseCollision.value == (baseCollision.value | (1 << collision.gameObject.layer)))
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * 0.2f);
        }
        else if (attackData.target.value == (attackData.target.value | (1 << collision.gameObject.layer)))
        {
            GameManager.Instance.playerStats.TakeDamage(attackData.power);
        }
    }

    private void DestroyProjectile(Vector2 position)
    {
        // 파티클이 있다면 position에 파티클 생성
        gameObject.SetActive(false);
    }
}
