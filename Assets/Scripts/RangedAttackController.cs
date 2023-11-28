using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackController : MonoBehaviour
{
    [SerializeField] private LayerMask baseCollision;
    protected RangedAttackData attackData;
    protected Vector2 direction;
    protected float currentDuration;
    private bool isReady;

    protected Rigidbody2D rigid;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (!isReady)
            return;
    }

    public void InitializeAttack(Vector2 direction, RangedAttackData attackData)
    {
        this.attackData = attackData;
        this.direction = direction;
        currentDuration = 0;
        transform.right = direction;
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
            // 데미지 주기
            Debug.Log("player hit");
            DestroyProjectile(transform.position);
        }
    }

    protected virtual void DestroyProjectile(Vector2 position)
    {
        // 파티클이 있다면 position에 파티클 생성
        gameObject.SetActive(false);
    }
}
