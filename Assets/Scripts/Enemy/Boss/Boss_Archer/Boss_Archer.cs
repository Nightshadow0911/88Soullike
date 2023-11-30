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
    private PositionAttack positionAttack;
    
    [Header("Unique Setting")]
    [SerializeField] private Transform attackPosition;
    [SerializeField] private LayerMask tileLayer;
    private bool isRage = false;
    
    [Header("ArrowEffects Type")]
    [SerializeField] private GameObject bomb;
    [SerializeField] private GameObject scatter;
    [SerializeField] private GameObject poison;

    protected override void Awake()
    {
        base.Awake();
        uniqueStats = GetBaseStats() as Boss_ArcherStats;
        rangedAttack = GetComponent<RangedAttack>();
        positionAttack = GetComponent<PositionAttack>();

        #region CloseRangedPattern
        AddPattern(Distance.CloseRange, DodgeAttack);
        AddPattern(Distance.CloseRange, BackstepAttack);
        AddPattern(Distance.CloseRange, BackTumbling);
        #endregion

        #region MediumRangePattern
        AddPattern(Distance.MediumRange, LeapShot);
        AddPattern(Distance.MediumRange, TrackingAttack);
        AddPattern(Distance.MediumRange, Run);
        #endregion

        #region LongRangePattern
        AddPattern(Distance.LongRange, LeapShot);
        AddPattern(Distance.LongRange, RangedAttack);
        #endregion

        bomb.SetActive(false);
        scatter.SetActive(false);
        poison.SetActive(false);
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
        rangedAttack.CreateProjectile(dir, data);
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
        yield return YieldCache.WaitForSeconds(0.5f);
        ShootArrow(false);
        state = State.SUCCESS;
        yield return null;
    }
    
    private IEnumerator ChargeShot()
    {
        RunningPattern();
        animationController.AnimationTrigger("ChargeShot");
        Vector3 direction = GetDirection();
        yield return YieldCache.WaitForSeconds(0.3f);
        SpecialArrowEffect(poison);
        yield return YieldCache.WaitForSeconds(0.7f);
        SpecialArrowEffect(poison);
        ShootSpecialArrow(direction, uniqueStats.poisonArrowData);
        state = State.SUCCESS;
        yield return null;
    }

    private IEnumerator RangedAttack()
    {
        if (!isRage)
            StartCoroutine(ArrowShot());
        else
            StartCoroutine(ChargeShot());
        yield return null;
    }

    private IEnumerator DodgeAttack()
    {
        RunningPattern();
        Vector2 direction = GetDirection();
        if (CheckTile(direction, true))
        {
            state = State.FAILURE;
            yield break;
        }
        soundManager.PlayClip(uniqueStats.dodgeSound);
        animationController.AnimationTrigger("DodgeAttack");
        Vector3 startPosition = transform.position;
        Vector3 endPosition = GetEndPosition(uniqueStats.dodgeDistance, false);
        float elapsedTime = 0f;
        while (elapsedTime < uniqueStats.dodgeTime && !CheckTile(direction, false))
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
        while (elapsedTime < uniqueStats.secondAttackTime && !CheckTile(direction, false))
        { 
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.secondAttackTime);
            yield return null;
        }
        soundManager.PlayClip(uniqueStats.meleeAttackSound);
        MeleeAttack();
        if (!isRage)
            state = State.SUCCESS;
        else
        {
            yield return YieldCache.WaitForSeconds(0.2f);
            yield return StartCoroutine(TrackingAttack());
        }
    }
    
    private IEnumerator BackstepAttack()
    {
        RunningPattern();
        Vector2 direction = -GetDirection();
        if (CheckTile(direction, true))
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
        while (elapsedTime < uniqueStats.backstepTime && !CheckTile(direction, false))
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
        Vector2 direction = GetDirection();
        yield return YieldCache.WaitForSeconds(0.4f);
        float distance,moveDistance;
        Vector3 startPosition = transform.position;
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
            !CheckTile(direction, false)
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
        if (CheckTile(direction, true))
        {
            state = State.FAILURE;
            yield break;
        }
        //soundManager.PlayClip(uniqueStats.);
        animationController.AnimationTrigger("BackTumbling");
        if (!isRage)
        {
            positionAttack.CreateProjectile((Vector2)transform.position + (Vector2.up * 2f)
                                            , uniqueStats.poisonFlaskData);
        }
        Vector3 startPosition = transform.position;
        Vector3 endPosition = GetEndPosition(uniqueStats.backTumblingDistance, true);
        float elapsedTime = 0f;
        while (elapsedTime < uniqueStats.backTumblingTime && !CheckTile(direction, false))
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.backTumblingTime);
            yield return null;
        }
        yield return YieldCache.WaitForSeconds(0.3f);
        
        float ran = Random.Range(0, 10);
        if (ran < 5)
        {
            yield return StartCoroutine(ChargeShot());
        }
        else
        {
            animationController.AnimationTrigger("SpinDashAttack");
            Rotate();
            direction = GetDirection();
            yield return YieldCache.WaitForSeconds(0.5f);
            //soundManager.PlayClip(uniqueStats.);
            float moveDistance;
            bool hit = false;
            startPosition = transform.position;
            do 
            {
                Vector3 position = transform.position;
                moveDistance = position.x - startPosition.x;
                rigid.velocity = direction * uniqueStats.spinDashAttackSpeed;
                if (!hit)
                {
                    MeleeAttack();
                    hit = true;
                }
                yield return YieldCache.WaitForFixedUpdate;
            }
            while (Mathf.Abs(moveDistance) < uniqueStats.spinDashAttackDistance && !CheckTile(direction, false));
            rigid.velocity = Vector2.zero;
        }
        state = State.SUCCESS;
    }

    private IEnumerator LeapShot()
    {
        RunningPattern();
        if (isRage)
        {
            state = State.FAILURE;
            yield break;
        }
        //soundManager.PlayClip(uniqueStats.);
        animationController.AnimationTrigger("OnLeapShot");
        rigid.gravityScale = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + (Vector3.up * uniqueStats.leapPosition);
        float elapsedTime = 0f;
        yield return YieldCache.WaitForSeconds(0.1f);
        while (elapsedTime < uniqueStats.leapTime)
        { 
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / uniqueStats.leapTime);
            yield return null;
        }
        yield return YieldCache.WaitForSeconds(0.2f);
        Vector3 direction;
        for (int i = 0; i < uniqueStats.numberOfLeapShot; i++)
        {
            float ran = Random.Range(0, 10);
            //soundManager.PlayClip();
            if (ran < 5)
                SpecialArrowEffect(bomb);
            else
                SpecialArrowEffect(scatter);
            elapsedTime = 0f;
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
            direction = (targetTransform.position - transform.position).normalized;
            yield return YieldCache.WaitForSeconds(0.3f);
            if (ran < 5)    
            {
                SpecialArrowEffect(bomb);
                ShootSpecialArrow(direction, uniqueStats.bombArrowData);
            }
            else
            {
                SpecialArrowEffect(scatter);
                ShootSpecialArrow(direction, uniqueStats.scatterArrowData);
            }
        }
        animationController.AnimationTrigger("EndLeapShot");
        Rotate();
        rigid.gravityScale = 1f;
        while (!CheckGround())
        {
            rigid.velocity = Vector2.down * currentStats.speed;
            yield return YieldCache.WaitForFixedUpdate;
        }
        rigid.velocity = Vector2.zero;
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

    private bool CheckTile(Vector2 dir, bool detect)
    {
        float distance = detect ? 2f : 1f;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, dir, distance, tileLayer);
        if (hit.collider != null)
            return true;
        return false;
    }

    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, Vector2.down, 0.7f, tileLayer);
        if (hit.collider != null)
            return true;
        return false;
    }

    private void SpecialArrowEffect(GameObject type)
    {
        if (!type.activeSelf)
            type.SetActive(true);
        else
            type.SetActive(false);
    }
}