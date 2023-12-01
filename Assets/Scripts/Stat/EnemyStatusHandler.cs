using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusHandler :StatHandler
{
    private EnemyStat enemyStat;
    private EnemyStat currentStat;

    private void Awake()
    {
        enemyStat = baseStatSO as EnemyStat;
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
        currentStat.characterMaxHP = enemyStat.characterMaxHP;
        currentStat.characterDamage = enemyStat.characterDamage;
        currentStat.characterDefense = enemyStat.characterDefense;
        currentStat.propertyDamage = enemyStat.propertyDamage;
        currentStat.propertyDefense = enemyStat.propertyDefense;
        currentStat.speed = enemyStat.speed;
        currentStat.delay = enemyStat.delay;
        currentStat.target = enemyStat.target;
    }
}
