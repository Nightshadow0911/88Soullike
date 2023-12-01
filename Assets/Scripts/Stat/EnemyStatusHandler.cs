using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusHandler :StatHandler
{
    private EnemyStat currentStat;

    private void Awake()
    {
        baseStatSO = baseStatSO as EnemyStat;
    }

    protected override void TakeDamage(int baseDamage)
    {
        if (currentStat != null)
        {
            currentStat.characterMaxHP-= baseDamage;
        }
    }

    protected override void SetStat()
    {
        currentStat = ScriptableObject.CreateInstance<EnemyStat>();
        currentStat.characterMaxHP = baseStatSO.characterMaxHP;
    }
}
