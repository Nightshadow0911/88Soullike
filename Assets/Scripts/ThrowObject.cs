using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ThrowObject : RangedAttackController
{
    [SerializeField] private GameObject AoE;
    private bool isThrow = false;
    
    protected override void Update()
    {
        base.Update();
        if (isThrow)
            return;
        rigid.AddForce(Vector2.up * attackData.speed, ForceMode2D.Impulse);
        isThrow = true;
    }

    protected override void DestroyProjectile(Vector2 position)
    {
        base.DestroyProjectile(position);
        // 폭발 애니메이션 추가 및 콜라이더 범위 변경
        Instantiate(AoE, transform.position, Quaternion.identity);
    }
}
