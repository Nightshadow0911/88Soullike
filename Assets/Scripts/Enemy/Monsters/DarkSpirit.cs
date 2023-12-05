using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DarkSpirit : EnemyCharacter
{
    [Header("Unique Setting")]
    [SerializeField] private Vector2 meleeAttackRange;

    protected override void Awake()
    {
        base.Awake();
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer);
        
        #region Pattern
        pattern.AddPattern(Distance.Default, Run);
        #endregion
    }
    
    protected override void SetPatternDistance()
    {
        pattern.SetDistance(Distance.Default);
    }

    protected override void DetectPlayer()
    {
        targetTransform = GameManager.Instance.player.transform;
        Invoke("DetectedTrue", 0.5f);
    }

    private void DetectedTrue()
    {
        detected = true;
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
        anim.HashBool(anim.run, true);
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) > characterStat.closeRange)
        {
            distance = targetTransform.position.x - transform.position.x;
            rigid.velocity = GetDirection() * characterStat.speed;
            yield return YieldCache.WaitForFixedUpdate;
        }
        anim.HashBool(anim.run, false);
        rigid.velocity = Vector2.zero;
        yield return StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        RunningPattern();
        anim.HashTrigger(anim.attack);
        yield return YieldCache.WaitForSeconds(0.4f);// 애니메이션 싱크
        MeleeAttack();
        state = State.SUCCESS;
    }

    protected override void Death()
    {
        anim.HashTrigger(anim.death);
        Boss_NightBorn.spiritNum--;
        Destroy(gameObject);
    }
}
