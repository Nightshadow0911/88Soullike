using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Pool;
using Object = System.Object;
using Random = UnityEngine.Random;

public class Boss_Archer : Boss
{
    [Header("Boss_Archer")]
    [SerializeField] private float backstepDistance;
    [SerializeField] private float dodgeDistance;
    [SerializeField] private float trackingDistance;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private Vector2 meleeAttackRange;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] public int maxHealth = 1000;
    [SerializeField] public Transform selfPosition;
    private int currentHealth;

    private readonly Vector3 atkRightPos = new Vector3(0.4f, 0.4f, 0);
    private readonly Vector3 atkLeftPos = new Vector3(-0.4f, 0.4f, 0);

    [Header("RangedAttack Info")] 
    [SerializeField] private ObjectPool.Pool ProjectileObj;
    [SerializeField] private RangedAttackData rangedData;

    [Header("Sound Info")]
    [SerializeField] private AudioClip runSound;
    [SerializeField] private AudioClip dodgeSound;
    [SerializeField] private AudioClip meleeAttackSound;
    [SerializeField] private AudioClip rangedAttackSound;
    [SerializeField] private AudioClip spinAttackSound;
    [SerializeField] private AudioClip snapShotSound;
    [SerializeField] private AudioClip danger;
    
    private Vector2 direction;
    private Vector2 destination;
    private bool dodging = false;
    private Boss_Archer_Animation anim;

   

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Boss_Archer_Animation>();
    }

    private void Start()
    {
        ProjectileManager.instance.InsertObjectPool(ProjectileObj);
        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        if (groggyMeter < 0)
        {
            currentTime += Time.deltaTime;
            OnGroggy();
            if (currentTime > groggyTime)
            {
                EndGroggy();
            }
        }
        
        if (!dodging && !isGroggy && !isDie)
            Rotate();
        
        base.Update();
    }
    
    protected override void BossPattern(Distance distance)
    {
        int randomPatton = Random.Range(1, 10);
        switch (distance) 
        {
            case Distance.CloseRange :
                if (CheckFrontWall(2.5f, sprite.flipX))
                {
                    if (randomPatton < 4)
                    {
                        anim.OnBakcstepAttack();
                    } 
                    else if (randomPatton < 8)
                    {
                        anim.OnMeleeAttack();
                    }
                    else
                    {
                        anim.OnTrakingAttack();
                    }
                }
                else if (CheckBackWall(2f, sprite.flipX))
                {
                    if (randomPatton < 1)
                    {
                        anim.OnBakcstepAttack();
                    }
                    else if (randomPatton < 4)
                    {
                        anim.OnMeleeAttack();
                    } 
                    else if (randomPatton < 9)
                    {
                        anim.OnDodge();
                    }
                    else
                    {
                        anim.OnTrakingAttack();
                    }
                }
                else
                {
                    if (randomPatton < 3)
                    {
                        anim.OnBakcstepAttack();
                    }
                    else if (randomPatton < 6)
                    {
                        anim.OnMeleeAttack();
                    }
                    else if (randomPatton < 8)
                    {
                        anim.OnDodge();
                    }
                    else
                    {
                        anim.OnTrakingAttack();
                    }
                }
                break;
            
            case Distance.MediumRange :
                if (randomPatton < 1)
                {
                    anim.OnBakcstepAttack();
                }
                else if (randomPatton < 4)
                {
                    anim.OnTrakingAttack();
                }
                else if (randomPatton < 7)
                {
                    anim.OnRangedAttack();
                }
                else
                {
                    Move();
                }
                break;
            
            case Distance.LongRange :
                if (randomPatton < 4)
                {
                    anim.OnRangedAttack();
                }
                else if (randomPatton < 8)
                {
                    anim.OnSnapshot(sprite.flipX);
                }
                else
                {
                    Move();
                }
                break;
        }
    }
    
    private void Rotate()
    {
        sprite.flipX = target.position.x < transform.position.x;
        attackPosition.localPosition = sprite.flipX ? atkLeftPos : atkRightPos;
    }

    private void Move()
    {
        StartCoroutine(Movement());
    }

    private void Dodge()
    {
        destination = GetDestination(dodgeDistance);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));
        dodging = true;
        StartCoroutine(DodgeMovement(destination));
        SoundManager.instance.PlayClip(dodgeSound);
    }
    
    private void MeleeAttack()
    {
        Collider2D collision = Physics2D.OverlapBox(attackPosition.position, meleeAttackRange, 0, targetLayer);
        if (collision != null)
        {
            GameManager.Instance.playerStats.TakeDamage(power);
            Debug.Log("player hit");
        }
        SoundManager.instance.PlayClip(meleeAttackSound);
        AttackReady();
    }

    private void RangedAttack()
    {
        ShootArrow(1, GetDirection());
        SoundManager.instance.PlayClip(rangedAttackSound);
        AttackReady();
    }
    
    private void TripleShot()
    {
        ShootArrow(3, GetDirection());
        SoundManager.instance.PlayClip(rangedAttackSound);
    }
    
    private void BackStep()
    {
        destination = (-GetDirection() * backstepDistance) + (Vector2)transform.position;
        SoundManager.instance.PlayClip(dodgeSound);
        StartCoroutine(BackStepMovement(destination));
    }
    
    
    private void TrackingAttack()
    {
        // 애니메이션 실행도중 그로기,데스 발생시 실행 취소
        if (isGroggy || isDie)
        {
            return;
        }
        StartCoroutine(Tracking());
    }

    private void Snapshot()
    {
        if (isGroggy || isDie)
        {
            return;
        }
        StartCoroutine(FireSnapshot());
    }

    private IEnumerator Movement()
    {
        SoundManager.instance.PlayClip(runSound);
        while (true) 
        {
            Vector2 distance = (target.position * Vector2.right) - (Vector2)transform.position;
            if (Mathf.Abs(distance.x) < 1)
            {
                SoundManager.instance.StopClip();
                rigid.velocity = Vector2.zero;
                anim.Running(rigid.velocity);
                break;
            }
            rigid.velocity = GetDirection() * speed;
            anim.Running(rigid.velocity);
            
            yield return null;
        }
        AttackReady();
        NoDelay();
    }

    private IEnumerator DodgeMovement(Vector2 dest)
    {
        yield return YieldCache.WaitForSeconds(0.1f);
        while (true)
        {
            if ((Vector2)transform.position == dest || CheckFrontWall(1f, sprite.flipX))
            {
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, destination, 0.05f);
            yield return null;
        }
        yield return YieldCache.WaitForSeconds(0.1f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), false);
        dodging = false;
        AttackReady();
        NoDelay();
    }
    
    private IEnumerator BackStepMovement(Vector2 dest)
    {
        // 점프
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position,  dest, 0.05f);
            // 목표지점에 도착하거나 벽에 닿으면 종료
            if ((Vector2)transform.position == dest || CheckBackWall(1f, sprite.flipX))
            {
                break;
            }
            yield return null;
        }
        AttackReady();
    }
    
    private IEnumerator Tracking() {
        anim.StartTracking();
        // 적을 추격하거나 위치에 도달하면 공격
        destination = GetDestination(trackingDistance);
        while (true)
        {
            Vector2 distance = (target.position * Vector2.right) - (Vector2)transform.position;
            if (Mathf.Abs(distance.x) < 1)
            {
                break;
            }
            else if ((Vector2)transform.position == destination)
            {
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, destination, 0.2f);
            yield return null;
        }
        yield return YieldCache.WaitForSeconds(0.1f);
        
        // 3번 공격
        anim.EndTracking();
        SoundManager.instance.PlayClip(spinAttackSound);
        for (int i = 0; i < 4; i++)
        {
            Collider2D collision = Physics2D.OverlapBox(attackPosition.position, meleeAttackRange, 0, targetLayer);
            if (collision != null)
            {
                GameManager.Instance.playerStats.TakeDamage(power);
                yield return YieldCache.WaitForSeconds(0.1f);
            }
            else
            {
                yield return YieldCache.WaitForSeconds(0.1f);
            }
        }
        yield return YieldCache.WaitForSeconds(0.2f);
        AttackReady();
    }

    private IEnumerator FireSnapshot()
    {
        //SoundManager.instance.PlayClip(snapShotSound);
        for (int i = 0; i < 3; i++)
        {
            ShootArrow(1, GetDirection());
            yield return YieldCache.WaitForSeconds(0.05f);
        }
        AttackReady();
    }
    
    private void ShootArrow(int numberOfArrows, Vector2 dir)
    {
        float dataAngle = rangedData.angle;
        // 각도 아래로 내림
        float minAngle = -(numberOfArrows / 2f) * dataAngle + 0.5f * dataAngle;
        for (int i = 0; i < numberOfArrows; i++)
        {
            float angle = minAngle + dataAngle * i;
            ProjectileManager.instance.CreateProjectile(
                attackPosition.position,
                RotateVector2(dir, angle),
                rangedData
            );
        }
    }
    
    private Vector2 RotateVector2(Vector2 v, float degree) {
        return Quaternion.Euler(0, 0, degree) * v;
    }

    // 벽에 닿았는지 확인
    private bool CheckFrontWall(float dist, bool flip)
    {
        if (flip)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, dist, wallLayer);
            if (hit.collider != null)
            {
                return true;
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, dist, wallLayer);
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }
    
    private bool CheckBackWall(float dist, bool flip)
    {
        if (!flip)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, dist, wallLayer);
            if (hit.collider != null)
            {
                return true;
            }
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, dist, wallLayer);
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    // 플레이어쪽 x축 방향을 찾음
    private Vector2 GetDirection()
    {
        direction = target.position - transform.position;
        direction *= Vector2.right;
        return direction.normalized;
    }
    
    // x축 목적지를 찾음
    private Vector2 GetDestination(float dist)
    {
        Vector2 dest = (GetDirection() * dist) + (Vector2)transform.position;
        return dest;
    }

    private void AttackReady()
    {
        isAttackReady = true;
    }

    private void NoDelay()
    {
        lastAttackTime = delay - 0.2f;
    }

    private void AllStop()
    {
        StopAllCoroutines();
        isAttackReady = false;
        rigid.velocity = Vector2.zero;
        anim.Running(rigid.velocity);
        OffDangerSign();
    }

    private void OnGroggy()
    {
        if (isGroggy || groggyMeter > 0)
            return;
        AllStop();
        anim.OnStun();
        isGroggy = true;
    }

    private void EndGroggy()
    {
        if (!isGroggy || groggyMeter > 0)
            return;
        currentTime = 0f;
        AttackReady();
        groggyMeter = baseGroggyMeter;
        anim.EndStun();
        isGroggy = false;
    }

    private void Death()
    {
        AllStop();
        anim.OnDeath();
        isDie = true;
        ProjectileManager.instance.DeleteObjectPool(ProjectileObj.tag);
        Vector2 SelfPosition = selfPosition.position + new Vector3(0, 1);
        SoulObjectPool objectPool = FindObjectOfType<SoulObjectPool>();
        foreach (var pool in objectPool.pools)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = objectPool.SpawnFromPool(pool.tag);

                if (obj != null)
                {
                    obj.transform.position = SelfPosition;
                    obj.SetActive(true);
                }
            }
        }
    }
    
    //private void OnDangerSign()
    //{
    //    SoundManager.instance.PlayClip(danger);
    //    dangerSign.SetActive(true);
    //}
    
    private void OffDangerSign()
    {
        dangerSign.SetActive(false);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Death();
        }
    }
}