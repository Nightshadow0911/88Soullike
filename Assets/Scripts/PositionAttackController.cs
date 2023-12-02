using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAttackController : MonoBehaviour
{
    protected PositionAttackData attackData;
    private float currentDuration;
    private bool isReady;
    
    private void Update()
    {
        if (!isReady)
            return;
        currentDuration += Time.deltaTime;

        if (currentDuration > attackData.duration) {
            DestroyProjectile();
        }
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attackData.AoE && attackData.target.value == (attackData.target.value | (1 << collision.gameObject.layer)))
        {
            // 데미지 주기
            Debug.Log("player hit");
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (attackData.AoE && attackData.target.value == (attackData.target.value | (1 << collision.gameObject.layer)))
        {
            // 데미지 주기
            Debug.Log("poison hit");
        }
    }

    public void InitializeAttack(Vector2 position, PositionAttackData attackData)
    {
        this.attackData = attackData;
        transform.position = position;
        currentDuration = 0;
        isReady = true;
    }

    protected void DestroyProjectile()
    {
        gameObject.SetActive(false);
    }
}
