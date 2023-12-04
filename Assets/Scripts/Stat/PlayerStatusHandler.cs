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
    private PlayerStat currentStat; // 현재 수치 저장 데이터 
    public PlayerStat GetStat() => currentStat;
    public PlayerStat growStatSO;// 현재 스탯 가져오기
    
    protected override void Awake()
    {
        playerBaseStat = baseStatSO as PlayerStat;
        playerGrowStat = growStatSO;
        base.Awake();
        SetStat();
    }

    
    public int CriticalCheck(int damage)
    {
        if (UnityEngine.Random.Range(0,100) < currentStat.criticalChance)
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
        playerMaxStat = ScriptableObject.CreateInstance<PlayerStat>();
        currentStat = ScriptableObject.CreateInstance<PlayerStat>();

        PlusStatsToMax();
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
        UpdateStatus();
        return true;
    }
   
    private void UpdateStatus()  // 업데이트 스테이터스 매서드
    {
        playerMaxStat.hp = playerMaxStat.healthStat * 10;
        playerMaxStat.stemina = playerMaxStat.steminaStat * 5;
        playerMaxStat.defense = playerMaxStat.healthStat * 2;
        playerMaxStat.stemina = playerMaxStat.steminaStat * 5; 
        playerMaxStat.weight = playerMaxStat.steminaStat * 3;
        playerMaxStat.parryTime = playerMaxStat.strStat * 0.01f;
        playerMaxStat.invincibleTime = playerMaxStat.dexStat * 0.01f;
        playerMaxStat.spellPower = playerMaxStat.intStat * 1; // 수치 수정 필요
        playerMaxStat.propertyDamage = playerMaxStat.intStat * 1; // 수치 수정 필요
        playerMaxStat.criticalChance = playerMaxStat.luxStat * 0.1f;
        playerMaxStat.soulDropRate = playerMaxStat.luxStat * 10f;
        
        playerMaxStat.damage = playerMaxStat.strStat * 4 + playerMaxStat.dexStat * 2; 
    }

    private void PlusStatsToMax()
    {
        if (playerBaseStat != null && playerGrowStat != null)
        {
            playerMaxStat.hp = playerBaseStat.hp + playerGrowStat.hp;
            playerMaxStat.damage = playerBaseStat.damage + playerGrowStat.damage;
            playerMaxStat.defense = playerBaseStat.defense + playerGrowStat.defense;
            playerMaxStat.speed = playerBaseStat.speed + playerGrowStat.speed;
            playerMaxStat.delay = playerBaseStat.delay + playerGrowStat.delay;
            playerMaxStat.attackRange = playerBaseStat.attackRange + playerGrowStat.attackRange;
            playerMaxStat.propertyDamage = playerBaseStat.propertyDamage + playerGrowStat.propertyDamage;
            playerMaxStat.propertyDefense = playerBaseStat.propertyDefense + playerGrowStat.propertyDefense;
            
            playerMaxStat.level = playerBaseStat.level + playerGrowStat.level;
            playerMaxStat.levelPoint = playerBaseStat.levelPoint + playerGrowStat.levelPoint;
            
            playerMaxStat.healthStat = playerBaseStat.healthStat + playerGrowStat.healthStat;
            playerMaxStat.steminaStat = playerBaseStat.steminaStat + playerGrowStat.steminaStat;
            playerMaxStat.strStat = playerBaseStat.strStat + playerGrowStat.strStat;
            playerMaxStat.dexStat = playerBaseStat.dexStat + playerGrowStat.dexStat;
            playerMaxStat.intStat = playerBaseStat.intStat + playerGrowStat.intStat;
            playerMaxStat.luxStat = playerBaseStat.luxStat + playerGrowStat.luxStat;
            
            playerMaxStat.stemina = playerBaseStat.stemina + playerGrowStat.stemina;
            playerMaxStat.weight = playerBaseStat.weight + playerGrowStat.weight;
            playerMaxStat.spellPower = playerBaseStat.spellPower + playerGrowStat.spellPower;
            playerMaxStat.regainHp = playerBaseStat.regainHp + playerGrowStat.regainHp;
            playerMaxStat.invincibleTime = playerBaseStat.invincibleTime + playerGrowStat.invincibleTime;
            playerMaxStat.parryTime = playerBaseStat.parryTime + playerGrowStat.parryTime;
            playerMaxStat.extraMoveSpeed = playerBaseStat.extraMoveSpeed + playerGrowStat.extraMoveSpeed;
            playerMaxStat.soulDropRate = playerBaseStat.soulDropRate + playerGrowStat.soulDropRate;
            playerMaxStat.criticalChance = playerBaseStat.criticalChance + playerGrowStat.criticalChance;
        }

        UpdateStatus();
        CopyStatsToCurrent();
    }

    private void CopyStatsToCurrent()
    {
      currentStat.hp =  playerMaxStat.hp;
      currentStat.damage = playerMaxStat.damage;
      currentStat.defense = playerMaxStat.defense;
      currentStat.speed = playerMaxStat.speed;
      currentStat.delay = playerMaxStat.delay;
      currentStat.attackRange = playerMaxStat.attackRange;
      currentStat.propertyDamage = playerMaxStat.propertyDamage;
      currentStat.propertyDefense = playerMaxStat.propertyDefense;
      
      currentStat.level = playerMaxStat.level;
      currentStat.levelPoint = playerMaxStat.levelPoint;
      
      currentStat.healthStat = playerMaxStat.healthStat;
      currentStat.steminaStat = playerMaxStat.steminaStat;
      currentStat.strStat = playerMaxStat.strStat;
      currentStat.dexStat = playerMaxStat.dexStat;
      currentStat.intStat = playerMaxStat.intStat;
      currentStat.luxStat = playerMaxStat.luxStat;
      
      currentStat.stemina = playerMaxStat.stemina;
      currentStat.weight = playerMaxStat.weight;
      currentStat.spellPower =  playerMaxStat.spellPower;
      currentStat.regainHp =  playerMaxStat.regainHp;
      currentStat.invincibleTime =  playerMaxStat.invincibleTime;
      currentStat.parryTime = playerMaxStat.parryTime;
      currentStat.extraMoveSpeed =  playerMaxStat.extraMoveSpeed;
      currentStat.soulDropRate = playerMaxStat.soulDropRate;
      currentStat.criticalChance = playerMaxStat.criticalChance;
    }

    private void StatHandicap(int level, Status statType)
    {
        double handicap = 5;
        double correctionStat = 0;
        int x = level;
        double floorStat = 0;

        switch (statType)
        {
            case Status.Health:
                correctionStat = 2;
                while (!(1 <= x && x <= 10) && !(handicap < 0))
                {
                    floorStat += (correctionStat * handicap) * 10;
                    handicap -= 0.2;
                    x -= 10;
                }
                floorStat += (correctionStat * handicap) * x;
                playerMaxStat.hp += (int)Math.Floor(floorStat);
                break;
            
            case Status.Stemina:
                correctionStat = 1;
                while (!(1 <= x && x <= 10) && !(handicap < 0))
                {
                    floorStat += (correctionStat * handicap) * 10;
                    handicap -= 0.1;
                    x -= 10;
                }
                floorStat += (correctionStat * handicap) * x;
                playerMaxStat.hp += (int)Math.Floor(floorStat);
                break;
        }
    }

}
