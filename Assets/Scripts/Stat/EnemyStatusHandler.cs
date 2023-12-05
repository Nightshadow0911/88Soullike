using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusHandler :StatHandler
{
    private EnemyStat currentStat; // 현재 수치 저장 데이터
    public EnemyStat GetStat() => currentStat; // 현재 스탯 가져오기
    
    private void Awake()
    {
        currentStat = currentStatSO as EnemyStat;
    }

    protected override void SetStat()
    {
        throw new NotImplementedException();
    }

    public override void TakeDamage(int damage)
    {
        if (currentStat == null)
            return;
        damage -= currentStat.defense;
        currentStat.hp -= damage;
    }
}
