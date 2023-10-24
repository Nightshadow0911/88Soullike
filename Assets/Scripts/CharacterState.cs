using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //메인 스텟
    private enum GrowState
    {
        growthHP, 
        growthStemina, 
        growthStr, 
        growthDex, 
        growthInt, 
        growthLux
    }
    //서브 스텟
    private int[] subState = new int[Enum.GetNames(typeof(Substate)).Length];
    private enum Substate
    {
        characterHp,
        characterWeight,
        characterDefense,
        characterStemina,
        charactermana,
        nomallAttackDamage,
        nomallSkillDamage,
        parryTime,
        addGoods,
        propertyDamage,
        EquipWeight,
        critcal
    }
    private double attackSpeed;
    private double moveSpeed;
    //몬스터 스텟
    private int monsterHp;
    private int monsterDamage;

    private void Update()
    {
        WeightSpeed();  //Update에 넣긴 했으나 장비가 변경될때 넣는게 좋아보임.
     
    }

    // 스텟 증가시 서브스텟 증가 함수
    // 체력 증가시 HP, 무게, 방어력
    private void HPGrow(int i)
    {
        subState[(int)Substate.characterHp] += i;
        subState[(int)Substate.characterWeight] += i;
        subState[(int)Substate.characterDefense] += i;
    }
    // 지구력 증가시 HP, 스태미너
    private void StGrow(int i)
    {
        subState[(int)Substate.characterHp] = 1;
        subState[(int)Substate.characterStemina] += i;
    }
    // 힘 증가시 일반공격력, 무게, 물리스킬데미지
    private void StrGrow(int i)
    {
        subState[(int)Substate.nomallAttackDamage] += i;
        subState[(int)Substate.characterWeight] += i;
        subState[(int)Substate.nomallSkillDamage] += i;
    }
    // 민첩 증가시 공속, 이속
    private void DexGrow(int i)
    {
        attackSpeed += i;
        moveSpeed += i;
    }
    // 운 증가시 치명타율, 패리 시간, 재화획득량, 버티기(??)
    private void LuxGrow(int i)
    {
        subState[(int)Substate.critcal] += i;
        subState[(int)Substate.parryTime] += i;
        subState[(int)Substate.addGoods] += i;
    }
    // 지능 증가시 마나, 속성 데미지
    private void IntGrow(int i)
    {
        subState[(int)Substate.charactermana] += i;
        subState[(int)Substate.propertyDamage] += i;
    }
    
    //무게에 따라 속도가 다름?
    private void WeightSpeed()
    {
        if (subState[(int)Substate.EquipWeight] * 1000 <= subState[(int)Substate.characterWeight] * 1000 * 0.3)
        {
            moveSpeed = moveSpeed * 1.2;
            //UI표시 [가벼움]
        }
        else if (subState[(int)Substate.EquipWeight] * 1000 <= subState[(int)Substate.characterWeight] * 1000 * 0.6)
        {
            moveSpeed = moveSpeed * 0.8;
            //UI표시 [무거움]
        }
    }
}
