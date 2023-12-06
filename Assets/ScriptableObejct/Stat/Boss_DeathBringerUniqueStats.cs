using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_DBUniqueStats", menuName = "Stats/EnemyStat/Boss_DBUniqueStats", order = 4)]
public class Boss_DeathBringerUniqueStat : ScriptableObject
{
    [Header("Boss_DB Stats")]
    public Vector2 meleeAttackRange;

    [Header("meleeAttack")]


    [Header("UseSpell")]
    public PositionAttackData spawnSpell;

    [Header("Projectile")]
    public List<ObjectPool.Pool> projectiles;

    [Header("Sound")]
    public AudioClip runSound;
    public AudioClip meleeAttackSound;
}