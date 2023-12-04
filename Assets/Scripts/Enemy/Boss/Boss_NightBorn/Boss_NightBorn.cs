using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_NightBorn : EnemyCharacter
{
    private Boss_NightBornStat uniqueStats;
    private PositionAttack positionAttack;
    
    [Header("Unique Setting")]
    [SerializeField] private LayerMask groundLayer;
    private bool isRage = false;

    protected override void Awake()
    {
        base.Awake();
        positionAttack = GetComponent<PositionAttack>();
        
        // #region CloseRangedPattern
        // AddPattern(Distance.CloseRange, );
        // AddPattern(Distance.CloseRange, );
        // AddPattern(Distance.CloseRange, );
        // #endregion
        //
        // #region MediumRangePattern
        // AddPattern(Distance.MediumRange, );
        // AddPattern(Distance.MediumRange, );
        // AddPattern(Distance.MediumRange, );
        // #endregion
        //
        // #region LongRangePattern
        // AddPattern(Distance.LongRange, );
        // AddPattern(Distance.LongRange, );
        // #endregion

    }
    
    protected override void Start()
    {
        base.Start();
        uniqueStats = statusHandler.GetUniqueStat() as Boss_NightBornStat;
        // foreach (ObjectPool.Pool projectile in uniqueStats.projectiles)
        // {
        //     ProjectileManager.instance.InsertObjectPool(projectile);
        // }
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
            pattern.SetDistance(Distance.Default);
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
    
    private IEnumerator RandomCut()
    {
        RunningPattern();
        float ran = Random.Range(0, 10);
        // 애니 트리거
        yield return YieldCache.WaitForSeconds(0.5f); // 애니 싱크
        MeleeAttack();
        if (ran < 5)
        {
            // 애니 트리거
            yield return YieldCache.WaitForSeconds(0.5f); // 애니 싱크
            MeleeAttack();
        }
        state = State.SUCCESS;
        yield return null;
    }

    private IEnumerator StraightExplosion()
    {
        RunningPattern();
        // 애니 트리거
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        // positionAttack.CreateMultipleProjectile(위치 수정, uniqueStats.data); // 포지션 맵끝으로 변경 필요
        // 애니 트리거
        yield return YieldCache.WaitForSeconds(1f); // 패턴끝나는거 기다리기
        state = State.SUCCESS;
        yield return null;
    }

    private IEnumerator BlinkExplosion()
    {
        RunningPattern();
        // 애니 트리거
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        transform.position = targetTransform.position;
        // 애니 트리거
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        // positionAttack.CreateMultipleProjectile(transform.position, uniqueStats.data);
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        state = State.SUCCESS;
        yield return null;
    }
    
    
    private Vector2 GetEndPosition(float distance, bool reverse)
    {
        Vector2 position = Vector2.right;
        if (reverse)
        {
            position *= targetTransform.position.x - transform.position.x < 0 ? distance : -distance;
        }
        else
        {
            position *= targetTransform.position.x - transform.position.x < 0  ? -distance : distance;
        }
        return (Vector2)transform.position + position;
    }
    
    private bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, Vector2.down, 0.7f, groundLayer);
        if (hit.collider != null)
            return true;
        return false;
    }
}
