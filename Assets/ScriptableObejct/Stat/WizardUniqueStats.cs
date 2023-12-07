using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WizardUniqueStats", menuName = "Stats/EnemyStat/WizardUniqueStats", order = 5)]
public class WizardUniqueStat : ScriptableObject
{
    [Header("Spell")]
    public RangedAttackData FireMissile;

    [Header("Projectile")]
    public List<ObjectPool.Pool> projectiles;

    [Header("Sound")]
    public AudioClip runSound;
    public AudioClip meleeAttackSound;
}