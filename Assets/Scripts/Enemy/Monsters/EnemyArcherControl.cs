using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using static Unity.Collections.AllocatorManager;
using Random = UnityEngine.Random;

public class EnemyArcherControl : EnemyCharacter
{
    [SerializeField] private ArcherUniqueStat uniqueStats;
    [SerializeField] private Vector2 meleeAttackRange;
    [SerializeField] private LayerMask tileLayer;

    private RangedAttack rangedAttack;
    private PositionAttack positionAttack;
    private bool isRage = false;

    protected override void Awake()
    {
        base.Awake();
        rangedAttack = GetComponent<RangedAttack>();
        positionAttack = GetComponent<PositionAttack>();


        #region CloseRangedPattern
        pattern.AddPattern(Distance.CloseRange, ArrowShot);
        #endregion

        #region MediumRangePattern
        pattern.AddPattern(Distance.MediumRange, Run);
        #endregion

        #region LongRangePattern
        pattern.AddPattern(Distance.LongRange, ArrowShot);
        #endregion
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Rotate()
    {
        transform.rotation = targetTransform.position.x < transform.position.x
            ? Quaternion.Euler(0, 180, 0)
            : Quaternion.Euler(0, 0, 0);
    }
    protected override void SetPatternDistance()
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

    protected override void DetectPlayer()
    {
        targetTransform = GameManager.Instance.player.transform;
        detected = true;
    }

    private IEnumerator Run()
    {
        RunningPattern();
        // soundManager.PlayClip(uniqueStats.runSound);
        anim.HashBool(anim.run, true);
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) > characterStat.closeRange && Mathf.Abs(distance) < characterStat.longRange)
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

    private IEnumerator ArrowShot()
    {
        RunningPattern();
        anim.StringTrigger("ArrowShot");
        yield return YieldCache.WaitForSeconds(1.0f);
        Vector2 position = targetTransform.position + (Vector3.up * 3.5f);
        positionAttack.CreateProjectile(position, uniqueStats.spawnArrow);
        yield return YieldCache.WaitForSeconds(0.6f);
        state = State.SUCCESS;
        yield return null;
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