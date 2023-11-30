using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/Skill", order = 0)]
public class SkillSO : ScriptableObject
{
    [SerializeField] private SkillType type;
    [SerializeField] private bool activeType; //false근거리 true원거리
    [SerializeField] private string skillName;
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private List<SkillEffect> efts;
    [SerializeField] private int power; // 데미지 계산식(ex: 스킬공격력 * power) or 버프시간 등
    [SerializeField] private int cost; // 마나 코스트
    [SerializeField] private List<String> descriptiion;
    [SerializeField] private PropertyType skillProperty; // 타입이 무기인 경우만 사용
    [SerializeField] private int price; // 가격(상점에서 살때의 가격임, 상점판매가 불가능한 경우 0)
    public bool Buyable() // true인 스킬만 상점에 표시, ex: 특정보스를 잡고 해금되는 스킬이면 Buyable을 false에서 true로 변경
    {
        return true;
    }

    public SkillType Type { get { return type; }}
    public bool ActiveType { get { return activeType; }}
    public string SkillName { get {  return skillName; }}
    public Sprite SkillIcon { get {  return skillIcon; }}
    public List<SkillEffect> SkillEffects { get { return efts; }}
    public int Power { get { return power; }}
    public int Cost { get { return cost; }}
    public List<String> Description { get { return descriptiion; }}
    public PropertyType SkillProperty { get { return skillProperty; }}
    public int Price { get { return price; }}
}

public enum SkillType
{
    Active,
    Passive,
    Buff
}


