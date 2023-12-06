using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FlyingEye : EnemyCharacter
{
    [SerializeField] private Vector2 meleeAttackRange;
    [SerializeField] private GameObject effect;
    [SerializeField] private LayerMask ignoreLayer;
    
     private bool find;

     protected override void Awake()
    {
        base.Awake();
        effect.SetActive(false);
        
        #region Pattern
        pattern.AddPattern(Distance.CloseRange, Attack);
        pattern.AddPattern(Distance.CloseRange, RollingAttack);
        pattern.AddPattern(Distance.Default, Run);
        #endregion
    }
     
    protected override void SetPatternDistance()
    {
        if (find)
            pattern.SetDistance(Distance.CloseRange);
        else 
            pattern.SetDistance(Distance.Default);
    }

    protected override void DetectPlayer()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, characterStat.detectRange,
            Vector2.right, 0, characterStat.target);
        if (hit.collider != null)
        {
            targetTransform = GameManager.instance.player.transform;
            detected = true;
        }
    }
    
    private void MeleeAttack(Vector2 position)
    {
        Collider2D collision = Physics2D.OverlapBox(
            position, meleeAttackRange, 0, characterStat.target);
        if (collision != null)
        {
            // 데미지 주기
            collision.GetComponent<PlayerStatusHandler>().TakeDamage(characterStat.damage);
        }
    }

    private IEnumerator Run()
    {
        RunningPattern();
        RaycastHit2D hit;
        while (!find)
        {
            hit = Physics2D.CircleCast(transform.position, characterStat.attackRange,
                Vector2.right, 0, characterStat.target);
            if (hit.collider != null)
            {
                find = true;
            }
            Rotate();
            Vector3 direction = (targetTransform.position + Vector3.up * 1.5f) - transform.position;
            rigid.velocity = direction.normalized * characterStat.speed;
            yield return YieldCache.WaitForFixedUpdate;
        }
        rigid.velocity = Vector2.zero;
        state = State.FAILURE;
    }

    private IEnumerator Attack()
    {
        RunningPattern();
        Vector2 direction = GetDirection();
        anim.HashTrigger(anim.attack);
        yield return YieldCache.WaitForSeconds(0.5f);// 애니메이션 싱크
        MeleeAttack((Vector2)transform.position + direction);
        find = false;
        state = State.SUCCESS;
    }
    
    private IEnumerator RollingAttack()
    {
        RunningPattern();
        anim.StringTrigger("RollingAttack");
        yield return YieldCache.WaitForSeconds(0.3f);// 애니메이션 싱크
        for (int i = 0; i < 2; i++)
        {
            yield return YieldCache.WaitForSeconds(0.2f);
            MeleeAttack(transform.position);
        }
        find = false;
        state = State.SUCCESS;
    }
}
