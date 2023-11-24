using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Pool;
using Object = System.Object;
using Random = UnityEngine.Random;



public class Boss_Archer1 : EnemyCharacter
{
    [Header("Unique Setting")]
    private Boss_ArcherStats uniqueStats;
    [SerializeField] private Transform attackPosition;
    //[SerializeField] private Animator anim;
    private RangedAttacking shoot;
    
    protected override void Awake()
    {
        base.Awake();
        shoot = GetComponent<RangedAttacking>();
        uniqueStats = baseStats as Boss_ArcherStats;
        
        #region CloseRanged
        AddPattern(Distance.CloseRange, TestMeleeAttack);
        AddPattern(Distance.CloseRange, DodgeMovement);
        AddPattern(Distance.CloseRange, BackStepMovement);
        #endregion
        
        #region MediumRange
        AddPattern(Distance.MediumRange, Moving);
        AddPattern(Distance.MediumRange, Tracking);
        #endregion
        
        #region LongRange
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
        shoot.CreateProjectile(GetDirection(), uniqueStats.arrowData);
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
        float distance = 0f;
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
        else
        {
            //soundManager.PlayClip();
            //애니메이션
            Vector3 startPosition = transform.position;
            Vector3 endPosition = GetEndPosition(uniqueStats.dodgeDistance);
            float elapsedTime = 0f;
            while (elapsedTime < uniqueStats.dodgeTime || !CheckBothSideWall())
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.dodgeTime);
                yield return null;
            }
        }
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
        Vector3 endPosition = -GetEndPosition(uniqueStats.dodgeDistance);
        float elapsedTime = 0f;
        while (elapsedTime < uniqueStats.backstepTime || !CheckBothSideWall())
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
        float distance = 0f;
        float trackingDistance = 0f;
        float startPosition = transform.position.x;
        //애니메이션
        while (Mathf.Abs(distance) > 1f || Mathf.Abs(trackingDistance) < uniqueStats.trackingDistance)
        {
            Vector3 position = transform.position;
            distance = targetTransform.position.x - position.x;
            trackingDistance = startPosition - position.x;
            rigid.velocity = GetDirection() * uniqueStats.trackingSpeed;
            yield return null;
        }
        yield return YieldCache.WaitForSeconds(0.1f);
        
        //soundManager.PlayClip();
        //애니메이션
        for (int i = 0; i < uniqueStats.numTrackingAttacks; i++)
        {
            Collider2D coll = Physics2D.OverlapBox(attackPosition.position,
                                                uniqueStats.meleeAttackRange, 0, uniqueStats.target);
            if (coll != null)
            {
                // 데미지 주기
                Debug.Log("player hit");
                yield return YieldCache.WaitForSeconds(uniqueStats.trackingAttacksWaitTime);
            }
            else
            {
                yield return YieldCache.WaitForSeconds(uniqueStats.trackingAttacksWaitTime);
            }
        }
        yield return YieldCache.WaitForSeconds(0.2f);
        state = State.SUCCESS;
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = Vector2.right * (targetTransform.position.x - transform.position.x);
        return direction.normalized;
    }
    
    private Vector2 GetEndPosition(float position)
    {
        return GetDirection() * (transform.position.x + position);
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