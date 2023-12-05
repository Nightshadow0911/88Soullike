using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public enum Status // <- 레벨업시 사용
{
    Health,
    Stemina, 
    Str, 
    Dex, 
    Int, 
    Lux 
}

public class PlayerStatusHandler :StatHandler
{
    private PlayerStat playerCurrentStat; // 현재 수치 저장 데이터 
    public PlayerStat GetStat() => playerCurrentStat;
    public PlayerStat growStatSO;
    public PlayerStat baseStatSO;

    public int currentHp;
    public float currentStemina;
    public int currentDamage;
    public int currentSpellPower;
    public int currentDefense;
    public int currentpropertyDamage;
    public int currentpropertyDefense;
    public int currentWeight;
    public int curretRegainHp;
    public int curretMana;
    public float currentSpeed;
    public float currentCritical;
    public float currentDelay;
    public float currentParryTime;
    public float currentSoulDrop;
    
    private void Awake()
    {
        playerCurrentStat = currentStatSO as PlayerStat;
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
        UpdateStat();
        currentHp = playerCurrentStat.hp;
        currentStemina = playerCurrentStat.stemina;
        currentDamage = playerCurrentStat.damage;
        currentSpellPower = playerCurrentStat.spellPower;
        currentDefense = playerCurrentStat.defense;
        currentpropertyDamage = playerCurrentStat.propertyDamage;
        currentpropertyDefense = playerCurrentStat.propertyDefense;
        currentWeight = playerCurrentStat.weight;
        curretRegainHp = playerCurrentStat.regainHp;
        curretMana = playerCurrentStat.mana;
        currentSpeed = playerCurrentStat.speed;
        currentCritical = playerCurrentStat.criticalChance;
        currentDelay = playerCurrentStat.delay;
        currentParryTime = playerCurrentStat.parryTime;
        currentSoulDrop = playerCurrentStat.soulDropRate;
    }

    public void UpdateStat()
    {
        growStatSO.DetailedStat(growStatSO);
        playerCurrentStat.PlusStatToMax(baseStatSO, growStatSO);
    }

    public bool GrowUpStat(int num, Status status) // 레벨업 메서드
    {
        if (growStatSO == null)
            return false;
        switch (status)
        {
            case Status.Health:
                growStatSO.healthStat += num;
                break;
            case Status.Stemina:
                growStatSO.steminaStat += num;
                break;
            case Status.Str:
                growStatSO.strStat += num;
                break;
            case Status.Dex:
                growStatSO.dexStat += num;
                break;
            case Status.Int:
                growStatSO.intStat += num;
                break;
            case Status.Lux:
                growStatSO.luxStat += num;
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
