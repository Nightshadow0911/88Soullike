using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusHandler :StatHandler
{
    private PlayerStat playerStat;
    private void Awake()
    {
        playerStat = BaseStatSO as PlayerStat;
    }

    
    public float CriticalCheck(float Damage)
    {
        if (Random.value < playerStat.criticalChance)
        {
            return Damage * 2.0f; 
        }
        return Damage;
    }

    public void TakeDamage(int baseDamage)
    {
        if (playerStat != null)
        {
            playerStat.currentHp -= baseDamage;
        }
    }
    
    //Gro매서드
    #region
    private void HPGrow(int i)
    {
        if (playerStat != null)
        {
            playerStat.growHP += 1;
            playerStat.characterMaxHP += i;
            playerStat.characterWeight += i;
            playerStat.characterDefense += i;
        }
    }

    private void StGrow(int i)
    {
        if (playerStat != null)
        {
            playerStat.growStemina += 1;
            playerStat.characterMaxHP += 1;
            playerStat.characterStamina += i;
        }
    }

    private void StrGrow(int i)
    {
        if (playerStat != null)
        {
            playerStat.growStr += 1;
            playerStat.characterDamage += i;
            playerStat.characterWeight += i;
            playerStat.nomallSkillDamage += i;
            playerStat.maxStr += i;
        }
    }
    private void DexGrow(int i)
    {
        if (playerStat != null)
        {
            playerStat.growDex += 1;
            playerStat.attackSpeed += i * 0.05;
            playerStat.moveSpeed += i * 0.05;
            playerStat.maxDex += 1;
        }
    }
    private void LuxGrow(int i)
    {
        if (playerStat != null)
        {
            playerStat.growLux += 1;
            playerStat.criticalChance += i;
            playerStat.parryTime += i * 0.025f;
            playerStat.addGoods += i;
            playerStat.characterRegainHp += i;
            playerStat.maxLuk += 1;
        }
    }
    private void IntGrow(int i)
    {
        if (playerStat != null)
        {
            playerStat.growInt += 1;
            playerStat.characterMana += i;
            playerStat.propertyDamage += i;
            playerStat.maxInt += 1;
        }
    }

    #endregion
    
    //레벨업 매서드
    #region
    public bool TryLevelUp(string selectedStat)
    {
        if (playerStat.levelPoint > 0)
        {
            playerStat.levelPoint --;
            switch (selectedStat)
            {
                case "HP":
                    HPGrow(1); // HP를 1만큼 업데이트
                    break;
                case "ST":
                    StGrow(1); // 스태미너를 1만큼 업데이트
                    break;
                case "STR":
                    StrGrow(1); // 스태미너를 1만큼 업데이트
                    break;
                case "DEX":
                    DexGrow(1); // 스태미너를 1만큼 업데이트
                    break;
                case "INT":
                    IntGrow(1); // 스태미너를 1만큼 업데이트
                    break;
                case "LUK":
                    LuxGrow(1); // 스태미너를 1만큼 업데이트
                    break;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
