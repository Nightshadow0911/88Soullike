using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_NightBorn : EnemyCharacter
{
    private Boss_NightBornStat uniqueStats;
    private PositionAttack positionAttack;
    
    [Header("Unique Setting")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject backLight;
    private bool isRage = false;

    protected override void Awake()
    {
        base.Awake();
        positionAttack = GetComponent<PositionAttack>();
        backLight.SetActive(false);
        
        #region CloseRangedPattern
        // pattern.AddPattern(Distance.CloseRange, Slash);
        // pattern.AddPattern(Distance.CloseRange, ForwardDashSlash);
        //pattern.AddPattern(Distance.CloseRange, SpwanMonster);
        pattern.AddPattern(Distance.CloseRange, Run);
        #endregion
        
        #region MediumRangePattern
        // pattern.AddPattern(Distance.MediumRange, ForwardDashSlash);
        //pattern.AddPattern(Distance.MediumRange, StraightExplosion);
        // pattern.AddPattern(Distance.MediumRange, BlinkExplosion);
        //pattern.AddPattern(Distance.MediumRange, SpwanMonster);
        pattern.AddPattern(Distance.MediumRange, Run);
        #endregion
    }
    
    protected override void Start()
    {
        base.Start();
        uniqueStats = statusHandler.GetUniqueStat() as Boss_NightBornStat;
        foreach (ObjectPool.Pool projectile in uniqueStats.projectiles)
        {
            ProjectileManager.instance.InsertObjectPool(projectile);
        }
    }

    protected override void SetPatternDistance()
    {
        float distance = Mathf.Abs(targetTransform.position.x - transform.position.x);
        if (distance < characterStat.closeRange)
        {
            pattern.SetDistance(Distance.CloseRange);
        }
        else
        {
            pattern.SetDistance(Distance.MediumRange);
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

    private IEnumerator Run()
    {
        RunningPattern();
        // soundManager.PlayClip(uniqueStats.runSound);
        animationController.AnimationBool("Run", true);
        float distance = float.MaxValue;
        while (Mathf.Abs(distance) > characterStat.closeRange)
        {
            distance = targetTransform.position.x - transform.position.x;
            rigid.velocity = GetDirection() * characterStat.speed;
            yield return YieldCache.WaitForFixedUpdate;
        }
        // soundManager.StopClip();
        animationController.AnimationBool("Run", false);
        rigid.velocity = Vector2.zero;
        state = State.SUCCESS;
    }
    
    private IEnumerator Slash()
    {
        RunningPattern();
        float ran = Random.Range(0, 10);
        animationController.AnimationTrigger("Slash");
        yield return YieldCache.WaitForSeconds(0.7f); // 애니 싱크
        MeleeAttack();
        if (ran < 5)
        {
            animationController.AnimationTrigger("TwiceSlash");
            yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
            MeleeAttack();
        }
        state = State.SUCCESS;
    }

    private IEnumerator ForwardDashSlash()
    {
        RunningPattern();
        //soundManager.PlayClip(uniqueStats.);
        animationController.AnimationTrigger("ForwardDashSlash");
        Vector2 direction = GetDirection();
        yield return YieldCache.WaitForSeconds(1f);
        float moveDistance;
        bool hit = false;
        Vector2 startPosition = transform.position;
        do 
        {
            Vector3 position = transform.position;
            moveDistance = position.x - startPosition.x;
            rigid.velocity = direction * uniqueStats.fowardDashSlashSpeed;
            if (!hit)
            {
                Collider2D collision = Physics2D.OverlapBox(
                    attackPosition.position, uniqueStats.meleeAttackRange, 0, uniqueStats.target);
                if (collision != null)
                {
                    // 데미지 주기
                    hit = true;
                    Debug.Log("player hit");
                }
            }
            yield return YieldCache.WaitForFixedUpdate;
        }
        while (Mathf.Abs(moveDistance) < uniqueStats.fowardDashSlashDistance && !CheckTile(direction));
        rigid.velocity = Vector2.zero;
        state = State.SUCCESS;
    }

    private IEnumerator StraightExplosion()
    {
        RunningPattern();
        animationController.AnimationTrigger("OnStraightExplosion");
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        float positionX = targetTransform.position.x - transform.position.x < 0 ? uniqueStats.minX : uniqueStats.maxX;
        positionAttack.CreateMultipleProjectile((Vector2.right * positionX) + Vector2.up, uniqueStats.bornExplosion);
        yield return YieldCache.WaitForSeconds(2f); // 패턴끝나는거 기다리기
        animationController.AnimationTrigger("EndStraightExplosion");
        yield return YieldCache.WaitForSeconds(0.5f);
        state = State.SUCCESS;
    }

    private IEnumerator BlinkExplosion()
    {
        RunningPattern();
        animationController.AnimationTrigger("OnBlinkExplosion");
        yield return YieldCache.WaitForSeconds(0.7f); // 애니 싱크
        transform.position = targetTransform.position;
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        positionAttack.CreateMultipleProjectile(transform.position + Vector3.up, uniqueStats.bothSideExplosion);
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        animationController.AnimationTrigger("EndBlinkExplosion");
        state = State.SUCCESS;
    }

    private IEnumerator SpwanMonster()
    {
        RunningPattern();
        if (isRage)
        {
            state = State.FAILURE;
            yield break;
        }
        animationController.AnimationTrigger("SpwanMonster");
        yield return YieldCache.WaitForSeconds(0.5f); // 애니 싱크
        float ran = Random.Range(uniqueStats.minX, uniqueStats.maxX);
        positionAttack.CreateProjectile((Vector2.right * ran) + Vector2.up, uniqueStats.spwanBall);
        yield return YieldCache.WaitForSeconds(0.5f); // 애니 싱크
        state = State.SUCCESS;
    }
    
    private bool CheckTile(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, dir, 2f, wallLayer);
        if (hit.collider != null)
            return true;
        return false;
    }
}
