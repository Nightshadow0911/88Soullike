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
    
    
}


