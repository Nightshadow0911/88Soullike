using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusHandler :StatHandler
{
    private EnemyStat enemyCurrentStat;
    public EnemyStat GetStat() => enemyCurrentStat; // 현재 스탯 가져오기
    
    public int currentHp;
    
    private void Awake()
    {
        enemyCurrentStat = currentStatSO as EnemyStat;
    }

    protected override void SetStat()
    {
        currentHp = enemyCurrentStat.hp;
    }

    public override void TakeDamage(int damage)
    {
        if (enemyCurrentStat == null)
            return;
        damage -= enemyCurrentStat.defense;
        currentHp -= damage;
    }
}
