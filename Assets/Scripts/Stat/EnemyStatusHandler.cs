using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyStatusHandler :StatHandler
{
    private EnemyStat enemyCurrentStat;
    public EnemyStat GetStat() => enemyCurrentStat; // 현재 스탯 가져오기
    public event Action OnDamage; 
    public event Action OnDeath; 
        
    [HideInInspector]
    public int currentHp;
    
    private void Awake()
    {
        enemyCurrentStat = currentStatSO as EnemyStat;
        SetStat();
    }

    protected override void SetStat()
    {
        currentHp = enemyCurrentStat.hp;
    }

    public override void TakeDamage(int damage)
    {
        if (enemyCurrentStat == null || currentHp <= 0)
            return;
        damage = damage <= enemyCurrentStat.defense ? 0 : damage - enemyCurrentStat.defense;
        currentHp -= damage;
        OnDamage?.Invoke();
        if (currentHp <= 0)
            OnDeath?.Invoke();
        Debug.Log(currentHp);
    }
}
