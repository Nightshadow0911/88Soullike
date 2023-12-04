using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public enum Status // <- 레벨업시 사용
{
    // 주석 == 기본 스탯 수치
    Health, // 10
    Stemina, // 10
    Str, // 5
    Dex, // 5
    Int, // 기본 수치 수정 필요
    Lux // 5
}

public class PlayerStatusHandler :StatHandler
{
    private PlayerStat playerBaseStat;
    private PlayerStat playerGrowStat;
    private PlayerStat playerMaxStat; // MAX 수치 저장 데이터
    private PlayerStat playerCurrentStat; // 현재 수치 저장 데이터 
    public PlayerStat GetStat() => playerCurrentStat;
    public PlayerStat GetMaxStat() => playerMaxStat;
    public PlayerStat growStatSO;
    public PlayerStat baseStatSO;
    
    protected override void Awake()
    {
        playerCurrentStat = currentStatSO as PlayerStat;
        playerBaseStat = baseStatSO;
        playerGrowStat = growStatSO;
        base.Awake();
        SetStat();
    }

    
    public int CriticalCheck(int damage)
    {
        if (UnityEngine.Random.Range(0,100) < playerCurrentStat.criticalChance)
        {
            int criticalDamage = damage * 2;
            return criticalDamage;
        }
        return damage;
    }

    public override void TakeDamage(int damage)
    {
        if (playerCurrentStat == null)
            return;
        damage -= playerCurrentStat.defense;
        playerCurrentStat.hp -= damage;
    }
    
    protected override void SetStat()
    {
        playerCurrentStat = ScriptableObject.CreateInstance<PlayerStat>();
        playerBaseStat = ScriptableObject.CreateInstance<PlayerStat>();
        playerGrowStat = ScriptableObject.CreateInstance<PlayerStat>();
        UpdateStat();
    }

    public void UpdateStat()
    {
        playerGrowStat.DetailedStat(playerGrowStat);
        playerBaseStat.DetailedStat(playerBaseStat);
        playerCurrentStat.PlusStatToMax(playerBaseStat, playerGrowStat);
        playerMaxStat.CopyBaseStat(playerCurrentStat);
    }

    public bool GrowUpStat(int num, Status status) // 레벨업 메서드
    {
        if (playerGrowStat == null)
            return false;
        switch (status)
        {
            case Status.Health:
                playerGrowStat.healthStat += num;
                break;
            case Status.Stemina:
                playerGrowStat.steminaStat += num;
                break;
            case Status.Str:
                playerGrowStat.strStat += num;
                break;
            case Status.Dex:
                playerGrowStat.dexStat += num;
                break;
            case Status.Int:
                playerGrowStat.intStat += num;
                break;
            case Status.Lux:
                playerGrowStat.luxStat += num;
                break;
        }
        return true;
    }
    
    
    public void UpdateWeapon(int power, float attackSpeed, float attackRange, int weight, int propertyAmount)
    {
        playerCurrentStat.damage += power;
        playerCurrentStat.delay += attackSpeed;
        playerCurrentStat.attackRange += attackRange;
        playerCurrentStat.weight += weight;
        playerCurrentStat.propertyDamage += propertyAmount;

    }
    public void UpdateArmor(int power, int weight, int propertyAmount)
    {
        playerCurrentStat.defense += power;
        playerCurrentStat.weight += weight;
        playerCurrentStat.propertyDefense += propertyAmount;
    }

}
