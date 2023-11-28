using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class Boss_Archer : EnemyCharacter
{
    private Boss_ArcherStats uniqueStats;
    private RangedAttack rangedAttack;
    
    [Header("Unique Setting")]
    [SerializeField] private Transform attackPosition;
    private bool isRage = false;
    
    protected override void Awake()
    {
        base.Awake();
        uniqueStats = GetBaseStats() as Boss_ArcherStats;
        rangedAttack = GetComponent<RangedAttack>();
        
        #region CloseRangedPattern
        AddPattern(Distance.CloseRange, BackTumbling);
        #endregion
        
        #region MediumRangePattern
        AddPattern(Distance.MediumRange, BackTumbling);
        #endregion
        
        #region LongRangePattern
        AddPattern(Distance.LongRange, BackTumbling);
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
        Collider2D collision = Physics2D.OverlapBox(
            attackPosition.position, uniqueStats.meleeAttackRange, 0, uniqueStats.target);
        if (collision != null)
        {
            // 데미지 주기
            Debug.Log("player hit");
        }
    }

    private void ShootArrow(bool multiple)
    {
        soundManager.PlayClip(uniqueStats.arrowAttackSound);
        if (multiple) 
            rangedAttack.CreateMultipleProjectile(GetDirection(), uniqueStats.arrowData);
        else
            rangedAttack.CreateProjectile(GetDirection(), uniqueStats.arrowData);
    }

    private void ShootSpecialArrow(Vector3 dir, RangedAttackData data)
    {
        soundManager.PlayClip(uniqueStats.arrowAttackSound);
        //rangedAttack.CreateProjectile(dir, uniqueStats.poisonArrowData);
    }
    
    private IEnumerator Run()
    {
        RunningPattern();
        soundManager.PlayClip(uniqueStats.runSound);
        animationController.AnimationBool("Run", true);
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) > 3f)
        {
            distance = targetTransform.position.x - transform.position.x;
            rigid.velocity = GetDirection() * currentStats.speed;
            yield return YieldCache.WaitForFixedUpdate;
        }
        soundManager.StopClip();
        animationController.AnimationBool("Run", false);
        rigid.velocity = Vector2.zero;
        state = State.FAILURE;
    }
    
    private IEnumerator ArrowShot()
    {
        RunningPattern();
        animationController.AnimationTrigger("ArrowShot");
        //yield return YieldCache.WaitForSeconds();
        if (!isRage) //에셋
            ShootSpecialArrow(GetDirection(), uniqueStats.poisonArrowData);
        else
            ShootArrow(false);
        state = State.SUCCESS;
        yield return null;
    }

    private IEnumerator DodgeAttack()
    {
        RunningPattern();
        Vector2 direction = GetDirection();
        if (CheckWall(direction))
        {
            state = State.FAILURE;
            yield break;
        }
        soundManager.PlayClip(uniqueStats.dodgeSound);
        animationController.AnimationTrigger("DodgeAttack");
        Vector3 startPosition = transform.position;
        Vector3 endPosition = GetEndPosition(uniqueStats.dodgeDistance, false);
        float elapsedTime = 0f;
        while (elapsedTime < uniqueStats.dodgeTime && !CheckWall(direction))
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.dodgeTime);
            yield return null;
        }
        Rotate();
        direction = GetDirection();
        yield return YieldCache.WaitForSeconds(0.3f);
        soundManager.PlayClip(uniqueStats.meleeAttackSound);
        MeleeAttack();
        yield return YieldCache.WaitForSeconds(0.3f);
        startPosition = transform.position;
        endPosition = startPosition + (Vector3)(direction * uniqueStats.secondAttackDistance);
        elapsedTime = 0f;
        while (elapsedTime < uniqueStats.secondAttackTime && !CheckWall(direction))
        { 
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.secondAttackTime);
            yield return null;
        }
        soundManager.PlayClip(uniqueStats.meleeAttackSound);
        MeleeAttack();
        if (isRage)
        {
            yield return YieldCache.WaitForSeconds(0.2f);
            yield return StartCoroutine(TrackingAttack());
        }
        else
            state = State.SUCCESS;
    }
    
    private IEnumerator BackstepAttack()
    {
        RunningPattern();
        Vector2 direction = -GetDirection();
        if (CheckWall(direction))
        {
            state = State.FAILURE;
            yield break;
        }
        //soundManager.PlayClip();
        animationController.AnimationTrigger("BackstepAttack");
        Vector3 startPosition = transform.position;
        Vector3 endPosition = GetEndPosition(uniqueStats.backstepDistance, true);
        Vector3 middlePosition = ((startPosition + endPosition) / 2) + (Vector3.up * uniqueStats.backstepJumpPosition);
        float elapsedTime = 0f;
        bool shoot = false;
        if (!isRage) // 에셋
        {
            rangedAttack.CreateProjectile(Vector2.up, uniqueStats.poisonFlaskData);
        }
        while (elapsedTime < uniqueStats.backstepTime && !CheckWall(direction))
        {
            elapsedTime += Time.deltaTime;
            Vector3 a = Vector3.Lerp(startPosition, middlePosition, elapsedTime / uniqueStats.backstepTime);
            Vector3 b = Vector3.Lerp(middlePosition, endPosition, elapsedTime / uniqueStats.backstepTime);
            transform.position = Vector3.Lerp(a, b, elapsedTime / uniqueStats.backstepTime);
            if (!shoot && (elapsedTime / uniqueStats.backstepTime) > 0.5f)
            {
                ShootArrow(true);
                shoot = true;
            }
            yield return null;
        }
        if (!shoot)
        {
            yield return YieldCache.WaitForSeconds(0.1f);
            ShootArrow(true);
        }
        state = State.SUCCESS;
    }
    
    private IEnumerator TrackingAttack() {
        RunningPattern();
        animationController.AnimationTrigger("OnTrackingAttack");
        yield return YieldCache.WaitForSeconds(0.4f);
        float distance,moveDistance;
        Vector3 startPosition = transform.position;
        Vector2 direction = GetDirection();
        do 
        {
            Vector3 position = transform.position;
            distance = targetTransform.position.x - position.x;
            moveDistance = position.x - startPosition.x;
            rigid.velocity = direction * uniqueStats.trackingSpeed;
            yield return YieldCache.WaitForFixedUpdate;
        } 
        while (
            Mathf.Abs(distance) > 1.5f &&
            Mathf.Abs(moveDistance) < uniqueStats.trackingDistance &&
            !CheckWall(direction)
            );
        rigid.velocity = Vector2.zero;
        yield return YieldCache.WaitForSeconds(0.15f);
        soundManager.PlayClip(uniqueStats.spinAttackSound);
        animationController.AnimationTrigger("TrackingAttack");
        for (int i = 0; i < uniqueStats.numberOfTrackingAttacks; i++)
        {
            MeleeAttack();
            yield return YieldCache.WaitForSeconds(uniqueStats.trackingAttacksWaitTime);
            yield return YieldCache.WaitForFixedUpdate;
        }
        state = State.SUCCESS;
    }

    private IEnumerator BackTumbling()
    {
        RunningPattern();
        Vector2 direction = -GetDirection();
        if (CheckWall(direction))
        {
            state = State.FAILURE;
            yield break;
        }
        //soundManager.PlayClip(uniqueStats.);
        //animationController.AnimationTrigger("BackTumbling");
        //rangedAttack.CreateProjectile(Vector2.up, uniqueStats.bombMineData);
        Vector3 startPosition = transform.position;
        Vector3 endPosition = GetEndPosition(uniqueStats.backTumblingDistance, true);
        float elapsedTime = 0f;
        while (elapsedTime < uniqueStats.backTumblingTime && !CheckWall(direction))
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.backTumblingTime);
            yield return null;
        }
        float ran = Random.Range(0, 10);
        if (ran < 5)
        {
            Debug.Log("독화살");
            //animationController.AnimationTrigger("ChargeArrowShot");
            //yield return YieldCache.WaitForSeconds();
            ShootSpecialArrow(GetDirection(), uniqueStats.poisonArrowData);
        }
        else
        {
            //soundManager.PlayClip(uniqueStats.);
            //animationController.AnimationTrigger("SpinDashAttack");
            Debug.Log("요네");
            float moveDistance;
            direction = GetDirection();
            startPosition = transform.position;
            do 
            {
                Vector3 position = transform.position;
                moveDistance = position.x - startPosition.x;
                rigid.velocity = direction * uniqueStats.spinDashAttackSpeed;
                MeleeAttack();
                yield return YieldCache.WaitForFixedUpdate;
            } 
            while (Mathf.Abs(moveDistance) < uniqueStats.spinDashAttackDistance && !CheckWall(direction));
        }
        state = State.SUCCESS;
    }

    private IEnumerator LeapShot()
    {
        RunningPattern();
        if (isRage) //에셋
        {
            state = State.FAILURE;
            yield break;
        }
        //soundManager.PlayClip(uniqueStats.);
        //animationController.AnimationTrigger("OnLeapShot");
        rigid.gravityScale = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + (Vector3.up * uniqueStats.leapPosition);
        float elapsedTime = 0f;
        while (elapsedTime < uniqueStats.leapTime)
        { 
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.leapTime);
            yield return null;
        }
        float ran = Random.Range(0, 10);
        Vector3 direction;
        elapsedTime = 0f;
        for (int i = 0; i < uniqueStats.numberOfLeapShot; i++)
        {
            //soundManager.PlayClip();
            //animationController.AnimationTrigger("LeapShot");
            while (elapsedTime < uniqueStats.aimingTime)
            { 
                elapsedTime += Time.deltaTime;
                direction = targetTransform.position - transform.position;
                float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if (direction.x < 0)
                    transform.rotation = Quaternion.Euler(0, 180, -180 - rotZ);
                else
                    transform.rotation = Quaternion.Euler(0, 0, rotZ);
                yield return null;
            }
            direction = targetTransform.position - transform.position;
            yield return YieldCache.WaitForSeconds(0.5f);   
            // if (ran < 5)    
            // {
            //     //폭탄화살
            //     ShootSpecialArrow(direction, uniqueStats.bombArrowData);
            // }
            // else
            // {
            //     //갈래화살
            //     ShootSpecialArrow(direction, uniqueStats.scatterArrowData);
            // }
        }
        //animationController.AnimationTrigger("LeapShot");
        Rotate();
        rigid.gravityScale = 1f;
        yield return YieldCache.WaitForSeconds(0.1f);
        state = State.SUCCESS;
    }

    private Vector2 GetDirection()
    {
        return targetTransform.position.x - transform.position.x < 0 ? Vector2.left : Vector2.right;
    }
    
    private Vector2 GetEndPosition(float distance, bool reverse)
    {
        float positionX = transform.position.x;
        if (reverse)
        {
            positionX += targetTransform.position.x - positionX < 0 ? distance : -distance;
        }
        else
        {
            positionX += targetTransform.position.x - positionX < 0 ? -distance : distance;
        }
        return Vector2.right * positionX;
    }

    private bool CheckWall(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, dir, 1.2f, 1 << LayerMask.NameToLayer("Wall"));
        if (hit.collider != null)
            return true;
        return false;
    }
}