using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusHandler :StatHandler
{
    public void TakeDamage(int baseDamage)
    {
        EnemyStat EnemyStat = BaseStatSO as EnemyStat;

        if (EnemyStat != null)
        {
            EnemyStat.characterMaxHP-= baseDamage;
        }
    }
}
