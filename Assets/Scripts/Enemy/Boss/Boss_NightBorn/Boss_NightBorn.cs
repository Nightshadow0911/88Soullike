using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_NightBorn : EnemyCharacter
{
    [Header("Unique Setting")]
    [SerializeField] private Boss_NightBornUniqueStat uniqueStats;
    [SerializeField] private GameObject backLight;
    [SerializeField] private LayerMask wallLayer;

    private PositionAttack positionAttack;
    public static int spiritNum;
    private bool isRage = false;

    protected override void Awake()
    {
        base.Awake();
        positionAttack = GetComponent<PositionAttack>();
        backLight.SetActive(false);
        
        
        #region CloseRangedPattern
        pattern.AddPattern(Distance.CloseRange, Slash);
        pattern.AddPattern(Distance.CloseRange, ForwardDashSlash);
        pattern.AddPattern(Distance.CloseRange, SpwanMonster);
        #endregion
        
        #region MediumRangePattern
        pattern.AddPattern(Distance.MediumRange, ForwardDashSlash);
        pattern.AddPattern(Distance.MediumRange, StraightExplosion);
        pattern.AddPattern(Distance.MediumRange, BlinkExplosion);
        pattern.AddPattern(Distance.MediumRange, SpwanMonster);
        pattern.AddPattern(Distance.MediumRange, Run);
        #endregion
    }
    
    protected override void Start()
    {
        base.Start();
        foreach (ObjectPool.Pool projectile in uniqueStats.projectiles)
        {
            ProjectileManager.instance.InsertObjectPool(projectile);
        }

        spiritNum = 0;
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
    
    protected override void DetectPlayer()
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
        float ran = Random.Range(0, 10);
        anim.StringTrigger("Slash");
        yield return YieldCache.WaitForSeconds(0.7f); // 애니 싱크
        MeleeAttack();
        if (ran < 5)
        {
            anim.StringTrigger("TwiceSlash");
            yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
            MeleeAttack();
        }
        state = State.SUCCESS;
    }

    private IEnumerator ForwardDashSlash()
    {
        RunningPattern();
        //soundManager.PlayClip(uniqueStats.);
        anim.StringTrigger("ForwardDashSlash");
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
                    attackPosition.position, uniqueStats.meleeAttackRange, 0, characterStat.target);
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
        anim.StringTrigger("OnStraightExplosion");
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        if (GetDirection().x < 0)
        {
            Vector3 position = new Vector2(uniqueStats.minX, transform.position.y + 1f);
            positionAttack.CreateMultipleProjectile( position, uniqueStats.bornExplosion, false);
        }
        else
        {
            Vector3 position = new Vector2(uniqueStats.maxX, transform.position.y + 1f);
            positionAttack.CreateMultipleProjectile(position, uniqueStats.bornExplosion, true);
        }
        yield return YieldCache.WaitForSeconds(2f); // 패턴끝나는거 기다리기
        anim.StringTrigger("EndStraightExplosion");
        yield return YieldCache.WaitForSeconds(0.5f);
        state = State.SUCCESS;
    }

    private IEnumerator BlinkExplosion()
    {
        RunningPattern();
        anim.StringTrigger("OnBlinkExplosion");
        yield return YieldCache.WaitForSeconds(0.7f); // 애니 싱크
        Vector3 position = transform.position += (targetTransform.position.x - transform.position.x) * Vector3.right;
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        positionAttack.CreateBothSideProjectile(position + Vector3.up,
            uniqueStats.bothSideExplosion);
        yield return YieldCache.WaitForSeconds(1f); // 애니 싱크
        anim.StringTrigger("EndBlinkExplosion");
        state = State.SUCCESS;
    }

    private IEnumerator SpwanMonster()
    {
        Debug.Log("시작");
        RunningPattern();   
        if (isRage || spiritNum >= 5)
        {
            state = State.FAILURE;
            yield break;
        }
        anim.StringTrigger("SpwanMonster");
        yield return YieldCache.WaitForSeconds(0.5f); // 애니 싱크
        float ran = Random.Range(uniqueStats.minX, uniqueStats.maxX);
        Vector2 position = new Vector2(ran, transform.position.y + 1f);
        positionAttack.CreateProjectile(position, uniqueStats.spwanBall);
        yield return YieldCache.WaitForSeconds(0.5f); // 애니 싱크
        state = State.SUCCESS;
        Debug.Log("끝");
    }
    
    private bool CheckTile(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, dir, 2f, wallLayer);
        if (hit.collider != null)
            return true;
        return false;
    }

    protected override void Death()
    {
        anim.HashTrigger(anim.death);
    }
}
