using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
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

    private readonly Vector3 atkRightPos = new Vector3(0.2f, 0.2f, 0);
    private readonly Vector3 atkLeftPos = new Vector3(-0.2f, 0.2f, 0);

    [Header("RangedAttack Info")] 
    [SerializeField] private PoolManager.Pool rangedObj;
    [SerializeField] private RangedAttackData rangedData;

    private Ray2D[] rays;
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
        PoolManager.instance.InsertToPool(rangedObj);
        rays = new Ray2D[2]
        {
            new Ray2D(transform.position, Vector2.left),
            new Ray2D(transform.position, Vector2.right)
        };
    }

    private void Update()
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
    
    protected override void BossPatton(Distance distance)
    {
        int randomPatton = Random.Range(1, 10);
        switch (distance) 
        {
            case Distance.CloseRange :
                if (CheckWall(1.5f))
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
                    if (randomPatton < 4)
                    {
                        anim.OnBakcstepAttack();
                    }
                    else if (randomPatton < 6)
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
                if (randomPatton < 5)
                {
                    anim.OnRangedAttack();
                }
                else if (randomPatton < 9)
                {
                    anim.OnSnapshot();
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
    }
    
    private void MeleeAttack()
    {
        Collider2D collision = Physics2D.OverlapBox(attackPosition.position, meleeAttackRange, 0, targetLayer);
        if (collision != null)
        {
            Debug.Log("player hit");
        }
        isAttackReady = true;
    }

    private void RangedAttack()
    {
        ShootArrow(1, GetDirection());
        isAttackReady = true;
    }
    
    private void TripleShot()
    {
        ShootArrow(3, GetDirection());
    }
    
    private void BackStep()
    {
        destination = (-GetDirection() * backstepDistance) + (Vector2)transform.position;
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
        while (true) 
        {
            Vector2 distance = (target.position * Vector2.right) - (Vector2)transform.position;
            if (Mathf.Abs(distance.x) < 0.8)
            {
                rigid.velocity = Vector2.zero;
                anim.Running(rigid.velocity);
                break;
            }
            rigid.velocity = GetDirection() * speed;
            anim.Running(rigid.velocity);
            yield return null;
        }
        isAttackReady = true;
        lastAttackTime = delay + 1;
    }

    private IEnumerator DodgeMovement(Vector2 dest)
    {
        yield return YieldCache.WaitForSeconds(0.1f);
        while (true)
        {
            if ((Vector2)transform.position == dest)
            {
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, destination, 0.05f);
            yield return null;
        }
        yield return YieldCache.WaitForSeconds(0.1f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), false);
        dodging = false;
        isAttackReady = true;
        lastAttackTime = delay + 1;
    }
    
    private IEnumerator BackStepMovement(Vector2 dest)
    {
        // 점프
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position,  dest, 0.05f);
            // 목표지점에 도착하거나 벽에 닿으면 종료
            if ((Vector2)transform.position == dest || CheckWall(0.5f))
            {
                break;
            }
            yield return null;
        }
        isAttackReady = true;
    }
    
    private IEnumerator Tracking() {
        anim.StartTracking();
        // 적을 추격하거나 위치에 도달하면 공격
        destination = GetDestination(trackingDistance);
        while (true)
        {
            Vector2 distance = (target.position * Vector2.right) - (Vector2)transform.position;
            if (Mathf.Abs(distance.x) < 0.5)
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
        for (int i = 0; i < 3; i++)
        {
            Collider2D collision = Physics2D.OverlapBox(attackPosition.position, meleeAttackRange, 0, targetLayer);
            if (collision != null)
            {
                Debug.Log("player skill hit");
                yield return YieldCache.WaitForSeconds(0.1f);
            }
            else
            {
                yield return YieldCache.WaitForSeconds(0.1f);
            }
        }
        yield return YieldCache.WaitForSeconds(0.2f);
        isAttackReady = true;
    }

    private IEnumerator FireSnapshot()
    {
        for (int i = 0; i < 3; i++)
        {
            ShootArrow(1, GetDirection());
            yield return YieldCache.WaitForSeconds(0.05f);
        }
        isAttackReady = true;
    }
    
    private void ShootArrow(int numberOfArrows, Vector2 dir)
    {
        float dataAngle = rangedData.angle;
        // 각도 아래로 내림
        float minAngle = -(numberOfArrows / 2f) * dataAngle + 0.5f * dataAngle;
        for (int i = 0; i < numberOfArrows; i++)
        {
            float angle = minAngle + dataAngle * i;
            PoolManager.instance.CreateRangedObject(
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
    private bool CheckWall(float dist)
    {
        for (int i = 0; i < rays.Length; i++)
        {
            rays[i].origin = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(rays[i].origin, rays[i].direction, dist, wallLayer);
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

    private void AllStop()
    {
        StopAllCoroutines();
        isAttackReady = false;
        rigid.velocity = Vector2.zero;
        anim.Running(rigid.velocity);
        OffRedEye();
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
        isAttackReady = true;
        groggyMeter = baseGroggyMeter;
        anim.EndStun();
        isGroggy = false;
    }

    private void Death()
    {
        AllStop();
        anim.OnDeath();
        isDie = true;
    }
    
    private void OnRedEye()
    {
        attackWarning.SetActive(true);
    }
    
    private void OffRedEye()
    {
        attackWarning.SetActive(false);
    }
}
