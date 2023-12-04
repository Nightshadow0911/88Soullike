using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusHandler :StatHandler
{
    private EnemyStat enemyMaxStat; // MAX 수치 저장 데이터
    private EnemyStat currentStat; // 현재 수치 저장 데이터
    public EnemyStat GetUniqueStat() => enemyMaxStat; // 몬스터 고유 스탯 가져오기
    public EnemyStat GetStat() => currentStat; // 현재 스탯 가져오기

    protected override void Awake()
    {
        enemyMaxStat = baseStatSO as EnemyStat;
        base.Awake();
    }

    public override void TakeDamage(int damage)
    {
        if (currentStat == null)
            return;
        damage -= currentStat.defense;
        currentStat.hp -= damage;
    }
   
    protected override void SetStat()
    {
        currentStat = ScriptableObject.CreateInstance<EnemyStat>();
        currentStat.hp = enemyMaxStat.hp;
        currentStat.damage = enemyMaxStat.damage;
        currentStat.defense = enemyMaxStat.defense;
        currentStat.speed = enemyMaxStat.speed;
        currentStat.delay = enemyMaxStat.delay;
        currentStat.attackRange = enemyMaxStat.attackRange;
        currentStat.propertyDamage = enemyMaxStat.propertyDamage;
        currentStat.propertyDefense = enemyMaxStat.propertyDefense;
        currentStat.detectRange = enemyMaxStat.detectRange;
        currentStat.target = enemyMaxStat.target;
        currentStat.closeRange = enemyMaxStat.closeRange;
        currentStat.mediumRange = enemyMaxStat.mediumRange;
        currentStat.longRange = enemyMaxStat.longRange;
    }
}
