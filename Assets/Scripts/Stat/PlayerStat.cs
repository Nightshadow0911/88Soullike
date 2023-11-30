using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Stats/PlayerStat", order = 1)]
public class PlayerStat : BaseStat
{
    public int level;
    public int levelPoint;

    //grow 스텟
    #region 
    public int growHP;
    public int growStemina;
    public int growStr;
    public int growDex;
    public int growInt;
    public int growLux;
    #endregion 
    
    // 상세 스텟들
    #region
    public int currentHp;
    public int characterRegainHp; //재생체력
    public int characterWeight; // 캐릭터 무게
    public int characterStamina; // 캐릭터 스테미너
    public int characterMana; // 캐릭터 마나
    public int nomallSkillDamage; // 주문력
    public int EquipWeight; // 장비 무게
    public int maxStr;
    public int maxDex;
    public int maxInt;
    public int maxLuk;
    #endregion
    
    //float 및 double 스텟
    #region
    public double attackSpeed = 1f; // 공격 속도
    public double addGoods; 
    public double moveSpeed = 5f; // 이동속도
    public float attackRange = 1f; // 공격 범위
    public float extraMoveSpeed = 1.5f;
    public float parryTime = 0.05f;
    public float criticalChance; // 크리티컬 확률
    #endregion

}
