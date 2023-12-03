using System.Collections;
using System.Collections.Generic;
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
    private PlayerStat playerMaxStat; // MAX 수치 저장 데이터
    private PlayerStat currentStat; // 현재 수치 저장 데이터 
    public PlayerStat GetStat() => currentStat; // 현재 스탯 가져오기
    
    protected override void Awake()
    {
        playerMaxStat = baseStatSO as PlayerStat;
        base.Awake();
    }

    
    public int CriticalCheck(int damage)
    {
        if (Random.Range(0,100) < currentStat.criticalChance)
        {
            int criticalDamage = damage * 2;
            return criticalDamage;
        }
        return damage;
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
        //추가
        currentStat = ScriptableObject.CreateInstance<PlayerStat>();
        currentStat.speed=playerMaxStat.speed;

        
        UpdateStatus();
    }

    public bool GrowUpStat(int num, Status status) // 레벨업 메서드
    {
        if (playerMaxStat == null)
            return false;
        switch (status)
        {
            case Status.Health:
                playerMaxStat.healthStat += num;
                break;
            case Status.Stemina:
                playerMaxStat.steminaStat += num;
                break;
            case Status.Str:
                playerMaxStat.strStat += num;
                break;
            case Status.Dex:
                playerMaxStat.dexStat += num;
                break;
            case Status.Int:
                playerMaxStat.intStat += num;
                break;
            case Status.Lux:
                playerMaxStat.luxStat += num;
                break;
        }
        UpdateStatus();
        return true;
    }
   
    private void UpdateStatus()  // 업데이트 스테이터스 매서드
    {
        currentStat.hp = playerMaxStat.healthStat * 10; 
        currentStat.defense = playerMaxStat.healthStat * 2;
        currentStat.stemina = playerMaxStat.steminaStat * 5; 
        currentStat.weight = playerMaxStat.steminaStat * 3;
        currentStat.increaseParryTime = playerMaxStat.strStat * 0.01f;
        currentStat.parryTime = playerMaxStat.parryTime + currentStat.increaseParryTime;
        currentStat.increaseInvincibleTime = playerMaxStat.dexStat * 0.01f;
        currentStat.invincibleTime = playerMaxStat.invincibleTime + currentStat.increaseInvincibleTime;
        currentStat.mana = 4;
        currentStat.spellPower = playerMaxStat.intStat * 1; // 수치 수정 필요
        currentStat.propertyDamage = playerMaxStat.intStat * 1; // 수치 수정 필요
        currentStat.criticalChance = playerMaxStat.luxStat * 0.1f;
        currentStat.increaseSoulDropRate = playerMaxStat.luxStat * 10f;
        currentStat.soulDropRate = playerMaxStat.soulDropRate + currentStat.increaseSoulDropRate;
        
        currentStat.damage = playerMaxStat.strStat * 4 + playerMaxStat.dexStat * 2; 
        
        playerMaxStat.hp = playerMaxStat.healthStat * 10;
        playerMaxStat.stemina = playerMaxStat.steminaStat * 5;
    }
}
