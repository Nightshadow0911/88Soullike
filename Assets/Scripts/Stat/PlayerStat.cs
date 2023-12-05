using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Stats/PlayerStat", order = 1)]
public class PlayerStat : BaseStat
{
    [Header("Level Info")]
    public int level;
    public int levelPoint;
    
    [Header("Stats")]
    public int healthStat;
    public int steminaStat;
    public int strStat;
    public int dexStat;
    public int intStat;
    public int luxStat;

    [Header("Player Stats")]
    public int mana;
    public int weight;
    public int spellPower; // 주문력
    public int regainHp; //재생체력
    public float stemina;
    public float invincibleTime;
    public float increaseInvincibleTime;
    public float parryTime;
    public float increaseParryTime; // 0.05f
    public float extraMoveSpeed; // 1.5f
    public float soulDropRate; 
    public float increaseSoulDropRate; 
    public float criticalChance; // 크리티컬 확률

    public void CopyBaseStat(PlayerStat basestat)
    {
        base.CopyStat(basestat);
        level = basestat.level;
        levelPoint = basestat.levelPoint;
        healthStat = basestat.healthStat;
        steminaStat = basestat.steminaStat;
        strStat = basestat.strStat;
        dexStat = basestat.dexStat;
        intStat = basestat.intStat;
        luxStat = basestat.luxStat;

        stemina = basestat.stemina;
        weight = basestat.weight;
        spellPower = basestat.spellPower;
        regainHp = basestat.regainHp;
        invincibleTime = basestat.invincibleTime;
        parryTime = basestat.parryTime;
        extraMoveSpeed = basestat.extraMoveSpeed;
        soulDropRate = basestat.soulDropRate;
        criticalChance = basestat.criticalChance;
        mana = basestat.mana;
    }
    public void DetailedStat(PlayerStat stat)
    {
        hp = stat.healthStat * 10;
        stemina = stat.steminaStat * 5;
        defense = stat.healthStat * 2;
        stemina = stat.steminaStat * 5;
        weight = stat.steminaStat * 3;
        parryTime = stat.strStat * 0.01f;
        invincibleTime = stat.dexStat * 0.01f;
        spellPower = stat.intStat * 1; // 수치 수정 필요
        propertyDamage = stat.intStat * 1; // 수치 수정 필요
        criticalChance = stat.luxStat * 0.1f;
        soulDropRate = stat.luxStat * 10f;

        damage = stat.strStat * 4 + stat.dexStat * 2;
    }
    public void PlusStatToMax(PlayerStat baseStat, PlayerStat growStat, PlayerStat buffStat)
    {
        hp = baseStat.hp + growStat.hp + buffStat.hp;
        damage = baseStat.damage + growStat.damage + buffStat.damage;
        defense = baseStat.defense + growStat.defense + buffStat.defense;
        speed = baseStat.speed + growStat.speed + buffStat.speed;
        delay = baseStat.delay + growStat.delay + buffStat.delay;
        attackRange = baseStat.attackRange + growStat.attackRange + buffStat.attackRange;
        propertyDamage = baseStat.propertyDamage + growStat.propertyDamage + buffStat.propertyDamage;
        propertyDefense = baseStat.propertyDefense + growStat.propertyDefense + buffStat.propertyDefense;

        level = baseStat.level + growStat.level + buffStat.level;
        levelPoint = baseStat.levelPoint + growStat.levelPoint + buffStat.levelPoint;

        healthStat = baseStat.healthStat + growStat.healthStat + buffStat.healthStat;
        steminaStat = baseStat.steminaStat + growStat.steminaStat + buffStat.steminaStat;
        strStat = baseStat.strStat + growStat.strStat + buffStat.strStat;
        dexStat = baseStat.dexStat + growStat.dexStat + buffStat.dexStat;
        intStat = baseStat.intStat + growStat.intStat + buffStat.intStat;
        luxStat = baseStat.luxStat + growStat.luxStat + buffStat.luxStat;

        stemina = baseStat.stemina + growStat.stemina + buffStat.stemina;
        weight = baseStat.weight + growStat.weight + buffStat.weight;
        spellPower = baseStat.spellPower + growStat.spellPower + buffStat.spellPower;
        regainHp = baseStat.regainHp + growStat.regainHp + buffStat.regainHp;
        invincibleTime = baseStat.invincibleTime + growStat.invincibleTime + buffStat.invincibleTime;
        parryTime = baseStat.parryTime + growStat.parryTime + buffStat.parryTime;
        extraMoveSpeed = baseStat.extraMoveSpeed + growStat.extraMoveSpeed + buffStat.extraMoveSpeed;
        soulDropRate = baseStat.soulDropRate + growStat.soulDropRate + buffStat.soulDropRate;
        criticalChance = baseStat.criticalChance + growStat.criticalChance + buffStat.criticalChance;
    }

}


