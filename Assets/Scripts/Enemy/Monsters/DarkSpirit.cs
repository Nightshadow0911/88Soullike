using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DarkSpirit : EnemyCharacter
{
    [Header("Unique Setting")]
    private Vector2 meleeAttackRange;
    
    protected override void Awake()
    {
        base.Awake();
        
        #region Pattern
        pattern.AddPattern(Distance.Default, Run);
        pattern.AddPattern(Distance.CloseRange, Attack);
        #endregion
    }

    protected override void SetPatternDistance()
    {
        float distance = Mathf.Abs(targetTransform.position.x - transform.position.x);
        if (distance < characterStat.closeRange)
        {
            pattern.SetDistance(Distance.CloseRange);
        }
        else
        {
            pattern.SetDistance(Distance.Default);
        }
    }
    
    protected override void Rotate()
    {
        transform.rotation = targetTransform.position.x < transform.position.x
            ? Quaternion.Euler(0, 0, 0)
            : Quaternion.Euler(0, 180, 0);
    } 
    

    private void MeleeAttack()
    {
        Collider2D collision = Physics2D.OverlapBox(
            attackPosition.position, meleeAttackRange, 0, characterStat.target);
        if (collision != null)
        {
            // 데미지 주기
            Debug.Log("player hit");
        }
    }

    private IEnumerator Run()
    {
        RunningPattern();
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) > characterStat.attackRange)
        {
            distance = targetTransform.position.x - transform.position.x;
            rigid.velocity = GetDirection() * characterStat.speed;
            yield return null;
        }
        rigid.velocity = Vector2.zero;
        state = State.SUCCESS;
        yield return null;
    }

    private IEnumerator Attack()
    {
        RunningPattern();
        yield return YieldCache.WaitForSeconds(0.5f);// 애니메이션 싱크
        MeleeAttack();
        state = State.SUCCESS;
        yield return null;
    }
}
