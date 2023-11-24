using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Pool;
using Object = System.Object;
using Random = UnityEngine.Random;

public class Boss_Archer1 : EnemyCharacter
{
    [Header("Unique Setting")]
    private Boss_ArcherStats UniqueStats;
    [SerializeField] private Transform attackPosition;
    //[SerializeField] private Animator anim;
    private Shooting shoot;
    
    [Header("Sound Info")]
    public AudioClip runSound;
    public AudioClip dodgeSound;
    public AudioClip meleeAttackSound;
    public AudioClip rangedAttackSound;
    public AudioClip spinAttackSound;
    public AudioClip snapShotSound;

    private List<IEnumerator> closeRange;
    private List<IEnumerator> mediumRange;
    private List<IEnumerator> longRange;
    
    protected override void Awake()
    {
        base.Awake();
        shoot = GetComponent<Shooting>();
        UniqueStats = baseStats as Boss_ArcherStats;
    }

    protected override void Start()
    {
        base.Start();
        foreach (ObjectPool.Pool projectile in UniqueStats.projectiles)
        {
            ProjectileManager.instance.InsertObjectPool(projectile);
        }
        
        closeRange = new List<IEnumerator>
        {
            Moving()
        };
        mediumRange = new List<IEnumerator>
        {
            Moving()
        };
        longRange = new List<IEnumerator>
        {
            Moving()
        };
        SetPattern(Distance.CloseRange, closeRange);
        SetPattern(Distance.MediumRange, mediumRange);
        SetPattern(Distance.LongRange, longRange);
    }

    private void Rotate()
    {
        transform.rotation = targetTransform.position.x < transform.position.x
            ? Quaternion.Euler(0, 180, 0)
            : Quaternion.Euler(0, 0, 0);
    }

    // private void Dodge()
    // {
    //     destination = GetDestination(dodgeDistance);
    //     StartCoroutine(DodgeMovement(destination));
    //     SoundManager.instance.PlayClip(dodgeSound);
    // }
    //
    // private void MeleeAttack()
    // {
    //     Collider2D collision = Physics2D.OverlapBox(attackPosition.position, meleeAttackRange, 0, targetLayer);
    //     if (collision != null)
    //     {
    //         Debug.Log("player hit");
    //     }
    //     SoundManager.instance.PlayClip(meleeAttackSound);
    // }
    //
    // private void RangedAttack()
    // {
    //     shot.CreateProjectile();
    //     SoundManager.instance.PlayClip(rangedAttackSound);
    // }
    //
    // private void BackStep()
    // {
    //     destination = (-GetDirection() * backstepDistance) + (Vector2)transform.position;
    //     SoundManager.instance.PlayClip(dodgeSound);
    //     StartCoroutine(BackStepMovement(destination));
    // }
   
    private IEnumerator Moving()
    {
        //soundManager.PlayClip(runSound);
        //애니메이션
        RunningPattern();
        Rotate();
        while (true)
        {
            float distance = targetTransform.position.x - transform.position.x;
            if (Mathf.Abs(distance) < 2f)
            {
                //soundManager.StopClip();
                //애니메이션
                rigid.velocity = Vector2.zero;
                break;
            }
            rigid.velocity = GetDirection() * currentStats.speed;
            yield return null;
        }
        state = PatternState.SUCCESS;
    }

    // private IEnumerator DodgeMovement(Vector2 dest)
    // {
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirection(), );
    //     if (hit.collider)
    //     while (true)
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, destination, 0.05f);
    //         yield return null;
    //     }
    //     yield return YieldCache.WaitForSeconds(0.1f);
    //     Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), false);
    // }
    
    // private IEnumerator BackStepMovement(Vector2 dest)
    // {
    //     // 점프
    //     rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    //     while (true)
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position,  dest, 0.05f);
    //         // 목표지점에 도착하거나 벽에 닿으면 종료
    //         if ((Vector2)transform.position == dest || CheckBackWall(1f, sprite.flipX))
    //         {
    //             break;
    //         }
    //         yield return null;
    //     }
    // }
    //
    // private IEnumerator Tracking() {
    //     anim.StartTracking();
    //     // 적을 추격하거나 위치에 도달하면 공격
    //     destination = GetDestination(trackingDistance);
    //     while (true)
    //     {
    //         Vector2 distance = (target.position * Vector2.right) - (Vector2)transform.position;
    //         if (Mathf.Abs(distance.x) < 1)
    //         {
    //             break;
    //         }
    //         else if ((Vector2)transform.position == destination)
    //         {
    //             break;
    //         }
    //         transform.position = Vector3.MoveTowards(transform.position, destination, 0.2f);
    //         yield return null;
    //     }
    //     yield return YieldCache.WaitForSeconds(0.1f);
    //     
    //     // 3번 공격
    //     anim.EndTracking();
    //     SoundManager.instance.PlayClip(spinAttackSound);
    //     for (int i = 0; i < 4; i++)
    //     {
    //         Collider2D collision = Physics2D.OverlapBox(attackPosition.position, meleeAttackRange, 0, targetLayer);
    //         if (collision != null)
    //         {
    //             GameManager.Instance.playerStats.TakeDamage(power);
    //             yield return YieldCache.WaitForSeconds(0.1f);
    //         }
    //         else
    //         {
    //             yield return YieldCache.WaitForSeconds(0.1f);
    //         }
    //     }
    //     yield return YieldCache.WaitForSeconds(0.2f);
    // }
    //
    // private IEnumerator FireSnapshot()
    // {
    //     SoundManager.instance.PlayClip(snapShotSound);
    //     for (int i = 0; i < 3; i++)
    //     {
    //         ShootArrow(1, GetDirection());
    //         yield return YieldCache.WaitForSeconds(0.05f);
    //     }
    // }

    private Vector2 GetDirection()
    {
        Vector2 direction = Vector2.right * (targetTransform.position.x - transform.position.x);
        return direction.normalized;
    }
    
    private Vector2 GetEndPosition(float position)
    {
        return (GetDirection() * position) + (Vector2)transform.position;
    }
}