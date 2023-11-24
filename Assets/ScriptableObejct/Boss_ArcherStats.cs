using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_ArcherStats", menuName = "EnemyStats/BossStats/Boss_ArcherStats", order = 0)]
public class Boss_ArcherStats : EnemyStats
{
    [Header("Boss_Archer Stats")]
    public float backstepDistance;
    public float dodgeDistance;
    public float trackingDistance;
    public float jumpForce;
    public Vector2 meleeAttackRange;
    public LayerMask wallLayer;
    
    [Header("RanedAttack Info")] 
    public List<ObjectPool.Pool> projectiles;
    public RangedAttackData rangedAttackData;
}
