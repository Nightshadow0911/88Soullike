using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    [SerializeField] private ItemSO curItem; // �̸�, �̹���, Ÿ��, �Ŀ�, ����

    [SerializeField] private ItemType type;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private List<ItemEffect> efts;
    [SerializeField] private int power;
    [SerializeField] private List<String> description;
    [SerializeField] private int amount;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private PropertyType weaponProperty;
    [SerializeField] private int propertyAmount;
    [SerializeField] private int weight;
    [SerializeField] private int price;

    public void Init()
    {
        if (curItem == null) return;

        type = curItem.Type;
        itemName = curItem.ItemName;
        sprite = curItem.Sprite;
        efts = curItem.Efts;
        power = curItem.Power;
        description = curItem.Descriptiion;
        amount = curItem.Amount;
        attackRange = curItem.AttackRange;
        attackSpeed = curItem.AttackSpeed;
        weaponProperty = curItem.WeaponProperty;
        propertyAmount = curItem.PropertyAmount;
        weight = curItem.Weight;
        price = curItem.Price;

    }

    public bool Use() //��� ������ ���
    {
        bool isUsed = false;
        if (type == ItemType.Armor || type == ItemType.Weapon)
        {
            Equip();
            return isUsed;
        }
        

        foreach(ItemEffect eft in efts)
        {
            isUsed = eft.ExcuteRole(power);
        }
        if(isUsed)
        {
            AmountDecrease();
        }
        return isUsed; // ������ ��� ���� ����
    }

    public void Equip() // ��� ������ ����
    {
        Equipment.instance.ChangeEquipItem(this);
        
    }
    void AmountDecrease()
    {
        amount--;
    }
    void AmountIncrease()
    {
        amount++;
    }

    #region ������Ƽ
    public ItemSO CurItem { get { return curItem; } set { curItem = value; } }
    public ItemType Type { get { return type; } }
    public string ItemName { get { return itemName; } }
    public Sprite Sprite { get { return sprite; } }
    public List<ItemEffect> Efts { get { return efts; } }
    public int Power { get { return power; } }
    public List<String> Description { get { return description; } }
    public int Amount { get { return amount; } set { amount = value; } }
    public float AttackRange { get { return attackRange; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public PropertyType WeaponProperty { get { return weaponProperty; } }
    public int PropertyAmount { get { return propertyAmount; } }
    public int Weight { get { return weight; } }
    public int Price { get { return price; } }
    #endregion
}
