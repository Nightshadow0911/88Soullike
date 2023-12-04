using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Stats/PlayerStat", order = 1)]
public class PlayerStat : BaseStat
{
    [Header("Level Info")]
    public int level;
    public int levelPoint;
    
    [Header("Grow Stats")]
    public int healthStat;
    public int steminaStat;
    public int strStat;
    public int dexStat;
    public int intStat;
    public int luxStat;
    
    [Header("Player Stats")]
    public int stemina;
    public int weight;
    public int spellPower; // 주문력
    public int regainHp; //재생체력
    public float invincibleTime;
    public float parryTime;
    public float extraMoveSpeed; // 1.5f
    public float soulDropRate; 
    public float criticalChance; // 크리티컬 확률

    public PlayerStat CopyPlayerStat(PlayerStat stat)
    {
        base.CopyStat(stat);
        stat.level = level;
        stat.levelPoint = levelPoint;
        stat.healthStat = healthStat;
        stat.steminaStat = steminaStat;
        stat.strStat = strStat;
        stat.dexStat = dexStat;
        stat.intStat = intStat;
        stat.luxStat = luxStat;
        
        stat.stemina = stemina;
        stat.weight = weight;
        stat.spellPower = spellPower;
        stat.regainHp = regainHp;
        stat.invincibleTime = invincibleTime;
        stat.parryTime = parryTime;
        stat.extraMoveSpeed = extraMoveSpeed;
        stat.soulDropRate = soulDropRate;
        stat.criticalChance = criticalChance;
        return stat;
    }
}


