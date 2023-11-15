using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    [SerializeField] private ItemType type;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private List<ItemEffect> efts;
    [SerializeField] private int power; // weapon : 공격력, armor : 방어력, potion : 회복량 등 '메인 수치'
    [SerializeField] private List<String> descriptiion;
    [Range(1,3)][SerializeField] private int amount;
    [SerializeField] private float attackRange; // 타입이 무기인 경우만 사용
    [SerializeField] private float attackSpeed; // 타입이 무기인 경우만 사용
    [SerializeField] private PropertyType weaponProperty; // 타입이 무기인 경우만 사용
    [SerializeField] private int weight; //타입이 무기 or 아머인 경우만 사용(장비 아이템)
    [SerializeField] private int price; // 가격(상점에서 살때의 가격임, 팔때는 price의 몇%로 팔 예정)
    public bool IsStackable()
    {
        switch(type)
        {
            default:
            case ItemType.Potion:
                return true;
            case ItemType.Weapon:
            case ItemType.Armor:
                return false;
        }
    }
    public bool Buyable() // true인 아이템만 상점에 표시, ex: 특정보스를 잡고 해금되는 아이템이면 Buyable을 false에서 true로 변경
    {
        return true;
    }
    public ItemType Type { get { return type; } }
    public string ItemName { get { return itemName;} }
    public Sprite Sprite { get { return sprite;} }
    public List<ItemEffect> Efts { get {  return efts;} }
    public int Power { get { return power; } }
    public List<String> Descriptiion { get {  return descriptiion;} }
    public int Amount { get { return amount; } }
    public float AttackRange { get {  return attackRange; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public PropertyType WeaponProperty { get { return weaponProperty; } }
    public int Weight { get {  return weight; } }
    public int Price { get { return price; } }
}

public enum ItemType
{
    Weapon,
    Armor,
    Potion
}

public enum PropertyType
{
    Non,
    Bleeding,
    Poisoning,
    Corruption

}


