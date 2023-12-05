using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_NightBornStats", menuName = "Stats/EnemyStat/Boss_NightBornStats", order = 3)]
public class Boss_NightBornStat : EnemyStat
{
    [Header("NightBorn Stats")] 
    public Vector2 meleeAttackRange;

    [Header("Sound Setting")] 
    public AudioClip runSound;
}