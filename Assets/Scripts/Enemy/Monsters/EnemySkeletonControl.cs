using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using static Unity.Collections.AllocatorManager;
using Random = UnityEngine.Random;

public class SkeletonEnemy : EnemyCharacter
{
    [SerializeField] private Vector2 meleeAttackRange;
    [SerializeField] private LayerMask tileLayer;

    protected override void Awake()
    {
        base.Awake();


        #region CloseRangedPattern
        pattern.AddPattern(Distance.CloseRange, Slash);
        #endregion

        #region MediumRangePattern
        pattern.AddPattern(Distance.MediumRange, Run);
        #endregion

        #region LongRangePattern
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

    private void MeleeAttack()
    {
        Collider2D collision = Physics2D.OverlapBox(
            attackPosition.position, meleeAttackRange, 0, characterStat.target);
        if (collision != null)
        {
            // ������ �ֱ�
            Debug.Log("player hit");
        }
    }

    private IEnumerator Run()
    {
        RunningPattern();
        // soundManager.PlayClip(uniqueStats.runSound);
        anim.HashBool(anim.run, true);
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) > characterStat.closeRange)
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
        yield return YieldCache.WaitForSeconds(1f);
        MeleeAttack();
        anim.StringTrigger("secondattack");
        yield return YieldCache.WaitForSeconds(1.2f);
        MeleeAttack();
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