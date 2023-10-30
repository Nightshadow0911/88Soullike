using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //레벨 관련
    private int level = 1;
    private int points = 5;
    
    //메인 성장 스텟
    private Dictionary<GrowState, int> growthValues = new Dictionary<GrowState, int>();
    private GrowState _currentState;
    private enum GrowState
    {
        growthHP, 
        growthStemina, 
        growthStr, 
        growthDex, 
        growthInt, 
        growthLux
    }
    //서브 성장 스텟
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

        GrowHP += 1;
        subState[(int)Substate.characterHp] += i;
        subState[(int)Substate.characterWeight] += i;
        subState[(int)Substate.characterDefense] += i;
    }
    // 지구력 증가시 HP, 스태미너
    private void StGrow(int i)
    {
        GrowStemina += 1;
        subState[(int)Substate.characterHp] = 1;
        subState[(int)Substate.characterStemina] += i;
    }
    // 힘 증가시 일반공격력, 무게, 물리스킬데미지
    private void StrGrow(int i)
    {
        GrowStr+= 1;
        subState[(int)Substate.nomallAttackDamage] += i;
        subState[(int)Substate.characterWeight] += i;
        subState[(int)Substate.nomallSkillDamage] += i;
    }
    // 민첩 증가시 공속, 이속
    private void DexGrow(int i)
    {
        GrowDex+= 1;
        attackSpeed += i;
        moveSpeed += i;
    }
    // 운 증가시 치명타율, 패리 시간, 재화획득량, 버티기(??)
    private void LuxGrow(int i)
    {
        GrowLux += 1;
        subState[(int)Substate.critcal] += i;
        subState[(int)Substate.parryTime] += i;
        subState[(int)Substate.addGoods] += i;
    }
    // 지능 증가시 마나, 속성 데미지
    private void IntGrow(int i)
    {
        GrowInt += 1;
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

    public CharacterStats()
    {
        growthValues[GrowState.growthHP] = 0;
        growthValues[GrowState.growthStemina] = 0;
        growthValues[GrowState.growthStr] = 0;
        growthValues[GrowState.growthDex] = 0;
        growthValues[GrowState.growthInt] = 0;
        growthValues[GrowState.growthLux] = 0;
    }
    public bool TryLevelUp(string selectedStat)
    {
        Debug.Log("Selected Stat: " + selectedStat);
        if (points > 0)
        {
            points--;
            level++;
            switch (selectedStat)
            {
                case "HP":
                    HPGrow(1); // HP를 1만큼 업데이트
                    Debug.Log("HP Increased");
                    break;
                case "Stamina":
                    StGrow(1); // 스태미너를 1만큼 업데이트
                    Debug.Log("Stemina Increased");
                    break;
                case "STR":
                    StrGrow(1); // 스태미너를 1만큼 업데이트
                    break;
                case "Dex":
                    DexGrow(1); // 스태미너를 1만큼 업데이트
                    break;
                case "Int":
                    IntGrow(1); // 스태미너를 1만큼 업데이트
                    break;
                case "Lux":
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
    
    //다른 곳에서 사용하기 위한 겟셋 함수들
    public int GrowHP
    {
        get
        {
            return growthValues[GrowState.growthHP];
        }
        set
        {
            growthValues[GrowState.growthHP] = value;
        }
    }

    public int GrowStemina
    {
        get
        {
            return growthValues[GrowState.growthStemina];
        }
        set
        {
            growthValues[GrowState.growthStemina] = value;
        }
    }

    public int GrowStr
    {
        get
        {
            return growthValues[GrowState.growthStr];
        }
        set
        {
            growthValues[GrowState.growthStr] = value;
        }
    }

    public int GrowDex
    {
        get
        {
            return growthValues[GrowState.growthDex];
        }
        set
        {
            growthValues[GrowState.growthDex] = value;
        }
    }

    public int GrowInt
    {
        get
        {
            return growthValues[GrowState.growthInt];
        }
        set
        {
            growthValues[GrowState.growthInt] = value;
        }
    }

    public int GrowLux
    {
        get
        {
            return growthValues[GrowState.growthLux];
        }
        set
        {
            growthValues[GrowState.growthLux] = value;
        }
    }
    public int Level
    {
        get { return level; }
    }

    public int Points
    {
        get { return points; }
    }
}
    

