using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_ArcherStats", menuName = "EnemyStats/BossStats/Boss_ArcherStats", order = 0)]
public class Boss_ArcherStats : EnemyStats
{
    [Header("Boss_Archer Stats")]
    public float backstepDistance;
    public float backstepTime;
    public float dodgeDistance;
    public float dodgeTime;
    public float trackingDistance;
    public int numTrackingAttacks;
    public float trackingAttacksWaitTime;
    public float trackingSpeed;
    public float jumpForce;
    public Vector2 meleeAttackRange;

    [Header("Check Wall")] 
    public float checkDistance;
    public LayerMask wallLayer;
    
    [Header("RanedAttack Info")] 
    public List<ObjectPool.Pool> projectiles;
    public RangedAttackData arrowData;
    
    [Header("Sound Setting")]
    public AudioClip runSound;
    public AudioClip dodgeSound;
    public AudioClip meleeAttackSound;
    public AudioClip rangedAttackSound;
    public AudioClip spinAttackSound;
    public AudioClip snapShotSound;
}
