using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Archer1 : EnemyCharacter
{
    private Boss_ArcherStats uniqueStats;
    private RangedAttack rangedAttack;
    
    [Header("Unique Setting")]
    [SerializeField] private Transform attackPosition;
    
    protected override void Awake()
    {
        base.Awake();
        uniqueStats = GetBaseStats() as Boss_ArcherStats;
        rangedAttack = GetComponent<RangedAttack>();
        
        #region CloseRangedPattern
        //(Distance.CloseRange, TestMeleeAttack);
        AddPattern(Distance.CloseRange, DodgeMovement);
        //AddPattern(Distance.CloseRange, BackStepMovement);
        #endregion
        
        #region MediumRangePattern
        AddPattern(Distance.MediumRange, Moving);
        AddPattern(Distance.MediumRange, Tracking);
        #endregion
        
        #region LongRangePattern
        AddPattern(Distance.LongRange, TestRangedAttack);
        #endregion
    }

    protected override void Start()
    {
        base.Start();
        foreach (ObjectPool.Pool projectile in uniqueStats.projectiles)
        {
            ProjectileManager.instance.InsertObjectPool(projectile);
        }
    }

    private void MeleeAttack()
    {
        //soundManager.PlayClip();
        Collider2D collision = Physics2D.OverlapBox(attackPosition.position,
                                                uniqueStats.meleeAttackRange, 0, uniqueStats.target);
        if (collision != null)
        {
            // 데미지 주기
            Debug.Log("player hit");
        }
    }

    private void ShootArrow()
    {
        //soundManager.PlayClip();
        rangedAttack.CreateProjectile(GetDirection(), uniqueStats.arrowData);
    }

    private IEnumerator TestMeleeAttack()
    {
        RunningPattern();
        MeleeAttack();
        state = State.SUCCESS;
        yield return null;
    }
    
    private IEnumerator TestRangedAttack()
    {
        RunningPattern();
        ShootArrow();
        state = State.SUCCESS;
        yield return null;
    }
    
    private IEnumerator Moving()
    {
        RunningPattern();
        //soundManager.PlayClip();
        //애니메이션
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) > 2f)
        {
            distance = targetTransform.position.x - transform.position.x;
            rigid.velocity = GetDirection() * currentStats.speed;
            yield return null;
        }
        //soundManager.StopClip();
        //애니메이션
        rigid.velocity = Vector2.zero;
        state = State.SUCCESS;
    }

    private IEnumerator DodgeMovement()
    {
        RunningPattern();
        if (CheckWall(GetDirection()))
        {
            state = State.FAILURE;
            yield break;
        }
        //soundManager.PlayClip();
        animationController.AnimationTrigger("DodgeAttack");
        Vector3 startPosition = transform.position;
        Vector3 endPosition = GetEndPosition(uniqueStats.dodgeDistance);
        float elapsedTime = 0f;
        while (elapsedTime < uniqueStats.dodgeTime && !CheckBothSideWall())
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.dodgeTime);
            yield return null;
        }
        Rotate();
        yield return YieldCache.WaitForSeconds(0.2f);
        MeleeAttack();
        yield return YieldCache.WaitForSeconds(0.25f);
        startPosition = transform.position;
        endPosition = GetEndPosition(uniqueStats.secondAttackDistance);
        elapsedTime = 0f;
        while (elapsedTime < uniqueStats.secondAttackTime && !CheckBothSideWall())
        { 
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.secondAttackTime);
            yield return null;
        }
        MeleeAttack();
        state = State.SUCCESS;
    }
    
    private IEnumerator BackStepMovement()
    {
        RunningPattern();
        if (CheckWall(-GetDirection()))
        {
            state = State.FAILURE;
            yield break;
        }
        //soundManager.PlayClip();
        //애니메이션
        rigid.AddForce(Vector2.up * uniqueStats.jumpForce, ForceMode2D.Impulse);
        Vector3 startPosition = transform.position;
        Vector3 endPosition = -GetEndPosition(uniqueStats.backstepDistance);
        float elapsedTime = 0f;
        while (elapsedTime < uniqueStats.backstepTime && !CheckBothSideWall())
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.backstepTime);
            yield return null;
        }
        state = State.SUCCESS;
    }
    
    private IEnumerator Tracking() {
        RunningPattern();
        //애니메이션
        yield return YieldCache.WaitForSeconds(0.5f);
        float distance = float.MaxValue;
        Vector3 position = Vector3.zero;
        Vector2 direction = GetDirection();
        Vector3 endPosition = GetEndPosition(uniqueStats.trackingDistance);
        //애니메이션
        while (Mathf.Abs(distance) > 1f && endPosition != position  && !CheckWall(direction))
        {
            position = transform.position;
            distance = targetTransform.position.x - position.x;
            rigid.velocity = direction * uniqueStats.trackingSpeed;
            yield return null;
        }
        rigid.velocity = Vector2.zero;
        yield return YieldCache.WaitForSeconds(0.1f);
        //soundManager.PlayClip();
        //애니메이션
        for (int i = 0; i < uniqueStats.numTrackingAttacks; i++)
        {
            Collider2D coll = Physics2D.OverlapBox(attackPosition.position,
                                                uniqueStats.meleeAttackRange, 0, uniqueStats.target);
            if (coll != null)
                // 데미지 주기
                Debug.Log("player hit");
            yield return YieldCache.WaitForSeconds(uniqueStats.trackingAttacksWaitTime);
        }
        yield return YieldCache.WaitForSeconds(0.2f);
        state = State.SUCCESS;
    }

    private Vector2 GetDirection()
    {
        return targetTransform.position.x - transform.position.x < 0 ? Vector2.left : Vector2.right;
    }
    
    private Vector2 GetEndPosition(float distance)
    {
        float positionX = transform.position.x;
        positionX += targetTransform.position.x - positionX < 0 ? -distance : distance;
        return Vector2.right * positionX;
    }

    private bool CheckWall(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
                                    dir, uniqueStats.checkDistance, uniqueStats.wallLayer);
        if (hit.collider != null)
            return true;
        return false;
    }

    private bool CheckBothSideWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(GetEndPosition(uniqueStats.checkDistance),
            -GetDirection(), uniqueStats.checkDistance * 2, uniqueStats.wallLayer);
        if (hit.collider != null)
            return true;
        return false;
    }
}