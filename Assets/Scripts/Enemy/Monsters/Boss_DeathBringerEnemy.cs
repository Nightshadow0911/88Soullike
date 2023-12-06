using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using static Unity.Collections.AllocatorManager;
using Random = UnityEngine.Random;

public class Boss_DeathBringerEnemy : EnemyCharacter
{
    [Header("Unique Setting")]
    [SerializeField] private Boss_DeathBringerUniqueStat uniqueStats;
    [SerializeField] private LayerMask tileLayer;

    private RangedAttack rangedAttack;
    private PositionAttack positionAttack;
    private bool isRage = false;

    private int spellCount; //스펠 사용 횟수
    private int maxSpellCount = 3; //최대 스펠 사용 횟수

    protected override void Awake()
    {
        base.Awake();
        rangedAttack = GetComponent<RangedAttack>();
        positionAttack = GetComponent<PositionAttack>();


        #region CloseRangedPattern
        pattern.AddPattern(Distance.CloseRange, Slash);
        #endregion

        #region MediumRangePattern
        pattern.AddPattern(Distance.MediumRange, Run);
        #endregion

        #region LongRangePattern
        pattern.AddPattern(Distance.LongRange, Blink);
        pattern.AddPattern(Distance.LongRange, UseSpell);
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

    protected override void Rotate()
    {
        transform.rotation = targetTransform.position.x < transform.position.x
            ? Quaternion.Euler(0, 0, 0)
            : Quaternion.Euler(0, 180, 0);
    }
    protected override void SetPatternDistance() //거리에 따른 패턴 조절
    {
        float distance = Mathf.Abs(targetTransform.position.x - transform.position.x);
        if (distance < characterStat.closeRange)
        {
            pattern.SetDistance(Distance.CloseRange);
        }
        else if (distance < characterStat.mediumRange)
        {
            pattern.SetDistance(Distance.MediumRange);
        }
        else
        {
            pattern.SetDistance(Distance.LongRange);
        }
    }

    protected override void DetectPlayer() //플레이어 추적
    {
        targetTransform = GameManager.Instance.player.transform;
        detected = true;
    }

    private void MeleeAttack()
    {
        Collider2D collision = Physics2D.OverlapBox(
            attackPosition.position, uniqueStats.meleeAttackRange, 0, characterStat.target);
        if (collision != null)
        {
            // 데미지 주기
            Debug.Log("player hit");
        }
    }

    private IEnumerator Run()
    {
        RunningPattern();
        // soundManager.PlayClip(uniqueStats.runSound);
        anim.HashBool(anim.run, true);
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) > characterStat.closeRange && Mathf.Abs(distance) < characterStat.mediumRange)
        {
            distance = targetTransform.position.x - transform.position.x;
            rigid.velocity = GetDirection() * characterStat.speed;
            yield return YieldCache.WaitForFixedUpdate;
        }
        // soundManager.StopClip();
        anim.HashBool(anim.run, false);
        rigid.velocity = Vector2.zero;
        state = State.FAILURE;
    }

    private IEnumerator Slash()
    {
        RunningPattern();
        anim.StringTrigger("attack");
        yield return YieldCache.WaitForSeconds(1.8f); // 애니 싱크
        MeleeAttack();
        state = State.SUCCESS;
        
    }

    private IEnumerator Blink()
    {
        RunningPattern();
        anim.StringTrigger("disappear");
        yield return YieldCache.WaitForSeconds(1.5f); // 애니 싱크
        Vector3 position = transform.position += (targetTransform.position.x - transform.position.x) * Vector3.right;
        transform.position = position;
        anim.StringTrigger("appear");
        state = State.SUCCESS;
    }

    private IEnumerator UseSpell()
    {
        RunningPattern();
        anim.StringTrigger("cast");
        yield return YieldCache.WaitForSeconds(1.0f); // 애니 싱크
        Vector2 position = targetTransform.position;
        positionAttack.CreateProjectile(position, uniqueStats.spawnSpell);
        yield return YieldCache.WaitForSeconds(0.6f); // 애니 싱크
        spellCount++;
        state = State.SUCCESS;
    }

    private bool CheckTile(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, dir, 2f, tileLayer);
        if (hit.collider != null)
            return true;
        return false;
    }

    protected override void Death()
    {
        anim.StringTrigger("death");
    }
}