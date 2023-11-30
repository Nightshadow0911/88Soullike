using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat", menuName = "Stats/EnemyStat/DefaultStat", order = 2)]
public class EnemyStat : BaseStat
{
   [Header("Enemy Stats")]
   public int speed;
   public float delay;
   public LayerMask target;
}
