using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CharacterStats : MonoBehaviour
{
    //레벨 관련
    private int level = 1;
    private int points = 5;

    [SerializeField]
    public int characterHp;
    [SerializeField]
    public float characterStamina;
    [SerializeField]
    public int characterNomallAttackDamage;

    //메인 성장 스텟
    private Dictionary<GrowState, int> growthValues = new Dictionary<GrowState, int>();
    private GrowState _currentState;
    private enum GrowState
    {
        growthHP,
        growthStamina,
        growthStr,
        growthDex,
        growthInt,
        growthLux
    }
    //서브 성장 스텟
    private int[] subState = new int[Enum.GetNames(typeof(Substate)).Length];
    private enum Substate
    {
        characterHp, // 현재체력
        characterWeight, // 캐릭터 무게
        characterDefense, // 캐릭터 방어력
        characterStamina, // 캐릭터 스테미너
        charactermana, // 현재 마나
        nomallAttackDamage, // 기본 공격력
        nomallSkillDamage, // 주문력
        parryTime, // 패링 가능 시간
        addGoods, // 재화 획득량 증가
        propertyDamage, // 속성 데미지 
        propertyDefense, //속성 방어력
        EquipWeight, // 장비 무게
        critcal // 크리티컬 확률
    }
    private double attackSpeed; // 공격 속도
    private double moveSpeed; // 이동속도
    public int Exp;
    public int Gold;

    public int curExp = 0;
    public int maxExp = 100;

    //몬스터 스텟
    [SerializeField]
    private int monsterHp;

    private void Start()
    {
        subState[(int)Substate.characterHp] = 100; // max
        characterHp = subState[(int)Substate.characterHp];
        subState[(int)Substate.characterStamina] = 100;
        characterStamina = subState[(int)Substate.characterStamina];
        subState[(int)Substate.nomallAttackDamage] = 10;
        characterNomallAttackDamage = subState[(int)Substate.nomallAttackDamage];
        subState[(int)Substate.critcal] = 50;
        subState[(int)Substate.propertyDefense] = 10;
        curExp = 27;
        maxExp = 100;
    }

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
        subState[(int)Substate.characterStamina] += i;
    }
    // 힘 증가시 일반공격력, 무게, 물리스킬데미지
    private void StrGrow(int i)
    {
        GrowStr += 1;
        subState[(int)Substate.nomallAttackDamage] += i;
        subState[(int)Substate.characterWeight] += i;
        subState[(int)Substate.nomallSkillDamage] += i;
    }
    // 민첩 증가시 공속, 이속
    private void DexGrow(int i)
    {
        GrowDex += 1;
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
        growthValues[GrowState.growthStamina] = 0;
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
    public void TakeDamage(int damage)//MonsterToPlayer
    {
        characterHp -= damage;
        Debug.Log("HP : " + characterHp);

        if (characterHp <= 0)  //플레이어가 파괴되므로 수정해야함.
        {
            // 게임 오브젝트를 즉시 파괴
            if (gameObject != null)
            {
                DestroyImmediate(gameObject, true);
            }
        }
    }

    public void AttackDamage(int monsterHP)//PlayerToMonster
    {
        int playerAttack;
        playerAttack = subState[(int)Substate.nomallAttackDamage] * 10;
        var criDamage = 0;
        float critChance = subState[(int)Substate.critcal];
        float crit = UnityEngine.Random.Range(0f, 1f);
        if (crit < critChance)
        {
            criDamage = playerAttack;
        }
        int totalDamage = playerAttack + criDamage;
        monsterHP -= totalDamage;
    }

    public void ProperyAttackDamage(int monsterHP) //????
    {
        int playerAttack;
        playerAttack = subState[(int)Substate.propertyDamage];
        monsterHP -= playerAttack;
    }

    public void PropertyAttack(int monsterPropertyDeffence)
    {
        monsterPropertyDeffence -= subState[(int)Substate.propertyDamage];
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
            return growthValues[GrowState.growthStamina];
        }
        set
        {
            growthValues[GrowState.growthStamina] = value;
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
    public int MaxHP
    {
        get{ return subState[(int)Substate.characterHp]; }
       
    }
    public int MaxStemina
    {
        get { return subState[(int)Substate.characterStamina]; }

    }
    public int CharacterWeight
    {
        get { return subState[(int)Substate.characterWeight]; }
    }
    public int NormalAttackDamage
    {
        get { return subState[(int)Substate.nomallAttackDamage]; }
        set { subState[(int)Substate.nomallAttackDamage] = value; }
    }
    public int NormalSkillDamage
    {
        get { return subState[(int)Substate.nomallSkillDamage]; }
    }
    public int PropertyDamage
    {
        get { return subState[(int)Substate.propertyDamage]; }
    }
    public int critical // float
    {
        get { return subState[(int)Substate.critcal]; }
    }
    public int CharacterDefense
    {
        get { return subState[(int)Substate.characterDefense]; }
        set { subState[(int)Substate.characterDefense] = value; }
    }
    public double CharacterSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public double AttackSpeed
    {
        get { return attackSpeed; }
        set { attackSpeed = value; }
    }
    public float ParryTime // float
    {
        get { return (int)Substate.parryTime; }
    } 
    public float AddGoods // float
    {
        get { return (int)Substate.addGoods; }
    }
    public int Level
    {
        get { return level; }
    }

    public int Points
    {
        get { return points; }
    }
    
    // 상태 이상
    
    public enum StatusEffectType
    {
        None,
        Poison,
        Bleeding,
        // 다른 상태 이상 유형을 여기에 추가할 수 있습니다.
    }
    
    private int poisonAccumulation = 0; // 독 상태 이상의 축적치
    private int bleedingAccumulation = 0; // 출혈 상태 이상의 축적치
    private int monsterPoisonAccumulation = 0; // 몬스터 독 상태 이상의 축적치
    private int monsterBleedingAccumulation = 0; // 몬스터 출혈 상태 이상의 축적치
    
    // 독 상태 이상을 적용하는 함수
    public void ApplyPoisonStatus(int damagePerTick, float duration, int amount)// 가드 초기화 부분
    {
        IncreaseAccumulation(StatusEffectType.Poison, amount);
        Debug.Log("축적치 : " + poisonAccumulation);
        if (poisonAccumulation >= 100)
        {
            StartCoroutine(DoPoisonEffect(damagePerTick, duration));
            poisonAccumulation = 0;
        }
    }

    // 출혈 상태 이상을 적용하는 함수
    public void ApplyBleedingStatus(int damagePerTick, float duration, int amount)
    {
        IncreaseAccumulation(StatusEffectType.Bleeding, amount);
        if (bleedingAccumulation >= 100)
        {
            StartCoroutine(DoBleedingEffect(damagePerTick, duration));
            bleedingAccumulation = 0;
        }
    }
    // 몬스터의 독 상태 이상을 적용하는 함수
 public void HitApplyPoisonStatus(int monsterHP, float duration, int monsterAmount,int monsterpropertydefense)
    {
        MonsterIncreaseAccumulation(StatusEffectType.Poison,monsterAmount,monsterpropertydefense);
        if (monsterPoisonAccumulation >= 100)
        {
            StartCoroutine(HITDoPoisonEffect(monsterHP, duration));
            monsterPoisonAccumulation = 0;
        }
    }
    // 몬스터의 출혈 상태 이상을 적용하는 함수
    public void HitBleedingStatus(int monsterHP, float duration, int monsterAmount,int monsterpropertydefense)
    {
        MonsterIncreaseAccumulation(StatusEffectType.Bleeding,monsterAmount,monsterpropertydefense);
        if (monsterBleedingAccumulation >= 100)
        {
            StartCoroutine(HITDoBleedingEffect(monsterHP, duration));
            monsterBleedingAccumulation = 0;
        }
    }
   

    // 축적치를 증가시키는 함수
    public void IncreaseAccumulation(StatusEffectType type, int amount)
    {
        int totalamount;
        totalamount = amount;
        switch (type)
        {
            case StatusEffectType.Poison:
                totalamount -= subState[(int)Substate.propertyDefense];
                poisonAccumulation += totalamount;
                break;
            case StatusEffectType.Bleeding:
                totalamount -= subState[(int)Substate.propertyDefense];
                bleedingAccumulation += totalamount;
                break;
        }
    }
    public void MonsterIncreaseAccumulation(StatusEffectType type, int monsteramount, int monsterpropertydefense)
    {
        int totalamount;
        totalamount = monsteramount;
        switch (type)
        {
            case StatusEffectType.Poison:
                totalamount -= monsterpropertydefense;
                monsterPoisonAccumulation += totalamount;
                break;
            case StatusEffectType.Bleeding:
                totalamount -= monsterpropertydefense;
                monsterBleedingAccumulation += totalamount;
                break;
        }
    }

    // 각 상태 이상에 대한 축적치를 가져오는 함수
    public int GetAccumulation(StatusEffectType type)
    {
        switch (type)
        {
            case StatusEffectType.Poison:
                return poisonAccumulation;
            case StatusEffectType.Bleeding:
                return bleedingAccumulation;
            default:
                return 0;
        }
    }
    
    private IEnumerator DoPoisonEffect(int damagePerTick, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            TakeDamage(damagePerTick);
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    private IEnumerator DoBleedingEffect(int damagePerTick, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            TakeDamage(damagePerTick * 2);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator HITDoPoisonEffect(int monsterHP, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            ProperyAttackDamage(monsterHP); 
            yield return new WaitForSeconds(1.0f);
        }
    }
    private IEnumerator HITDoBleedingEffect(int monsterHP, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            ProperyAttackDamage(monsterHP); 
            yield return new WaitForSeconds(1.0f);
        }
    }
}

