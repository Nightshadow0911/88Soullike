using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using static Unity.Collections.AllocatorManager;
using Random = UnityEngine.Random;

public class EnemyWizardControl : EnemyCharacter
{
    [SerializeField] private WizardUniqueStat uniqueStats;
    [SerializeField] private LayerMask tileLayer;

    private RangedAttack rangedAttack;


    protected override void Awake()
    {
        base.Awake();
        rangedAttack = GetComponent<RangedAttack>();


        #region CloseRangedPattern
        pattern.AddPattern(Distance.CloseRange, MoveBack);
        pattern.AddPattern(Distance.CloseRange, FireMissile);
        #endregion

        #region MediumRangePattern
        pattern.AddPattern(Distance.MediumRange, FireMissile);
        #endregion

        #region LongRangePattern
        pattern.AddPattern(Distance.LongRange, Run);
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

    private IEnumerator Run()
    {
        RunningPattern();
        // soundManager.PlayClip(uniqueStats.runSound);
        anim.HashBool(anim.run, true);
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) > characterStat.mediumRange)
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
    private IEnumerator MoveBack()
    {
        RunningPattern();
        // soundManager.PlayClip(uniqueStats.runSound);
        anim.HashBool(anim.run, true);
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) < characterStat.closeRange)
        {
            distance = targetTransform.position.x - transform.position.x;
            rigid.velocity = -GetDirection() * characterStat.speed;
            yield return YieldCache.WaitForFixedUpdate;
        }
        // soundManager.StopClip();
        anim.HashBool(anim.run, false);
        rigid.velocity = Vector2.zero;
        state = State.FAILURE;
    }

    private IEnumerator FireMissile()
    {
        
        RunningPattern();
        anim.StringTrigger("FireMissile");
        yield return YieldCache.WaitForSeconds(2f);
        Vector2 position = targetTransform.position;
        rangedAttack.CreateProjectile(GetDirection(), uniqueStats.FireMissile);
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

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}