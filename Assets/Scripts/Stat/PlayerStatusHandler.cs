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

    
    public int CriticalCheck(int Damage)
    {
        if (Random.value < currentStat.criticalChance)
        {
            int criticalDamage = currentStat.damage * 2;
            return criticalDamage;
        }
        return currentStat.damage;
    }

    protected override void TakeDamage(int baseDamage)
    {
        if (currentStat == null)
            return;
        baseDamage -= currentStat.defense;
        currentStat.hp -= baseDamage;
    }
    
    protected override void SetStat()
    {
        currentStat = ScriptableObject.CreateInstance<PlayerStat>();
        currentStat.speed=playerMaxStat.speed;
        currentStat.parryTime = playerMaxStat.parryTime;

        UpdateStatus();
    }

    private void GrowUpStat(int num, Status status) // 레벨업 메서드
    {
        if (playerMaxStat == null)
            return;
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
    }
   
    private void UpdateStatus()  // 업데이트 스테이터스 매서드
    {
        currentStat.hp = playerMaxStat.healthStat * 10; 
        currentStat.defense = playerMaxStat.healthStat * 2;
        currentStat.stemina = playerMaxStat.steminaStat * 5; 
        currentStat.weight = playerMaxStat.steminaStat * 3;
        currentStat.damage = playerMaxStat.strStat * 4; 
        currentStat.increaseParryTime = playerMaxStat.strStat * 0.01f;
        currentStat.parryTime = playerMaxStat.parryTime + currentStat.increaseParryTime;
        currentStat.damage = playerMaxStat.dexStat * 2; 
        currentStat.increaseInvincibleTime = playerMaxStat.dexStat * 0.01f;
        currentStat.invincibleTime = playerMaxStat.invincibleTime + currentStat.increaseInvincibleTime;
        currentStat.mana = playerMaxStat.intStat * 1; // 수치 수정 필요
        currentStat.spellPower = playerMaxStat.intStat * 1; // 수치 수정 필요
        currentStat.propertyDamage = playerMaxStat.intStat * 1; // 수치 수정 필요
        currentStat.criticalChance = playerMaxStat.luxStat * 0.1f;
        currentStat.increaseSoulDropRate = playerMaxStat.luxStat * 10f;
        currentStat.soulDropRate = playerMaxStat.soulDropRate + currentStat.increaseSoulDropRate;
    }
}
