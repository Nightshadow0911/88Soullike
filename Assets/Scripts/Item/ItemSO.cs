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
    [SerializeField] private int power; // weapon : ���ݷ�, armor : ����, potion : ȸ���� �� '���� ��ġ'
    [SerializeField] private List<String> descriptiion;
    [Range(1, 10)][SerializeField] private int amount;
    [SerializeField] private float attackRange; // Ÿ���� ������ ��츸 ���
    [SerializeField] private float attackSpeed; // Ÿ���� ������ ��츸 ���
    [SerializeField] private PropertyType weaponProperty; // Ÿ���� ������ ��츸 ���
    [SerializeField] private int propertyAmount; // Ÿ���� ������ ��츸 �Ӽ� ����, ���ϰ�� �Ӽ� ����
    [SerializeField] private int weight; //Ÿ���� ���� or �Ƹ��� ��츸 ���(��� ������)
    [SerializeField] private int price; // ����(�������� �춧�� ������, �ȶ��� price�� ��%�� �� ����)
    public bool IsStackable()
    {
        switch (type)
        {
            default:
            case ItemType.Potion:
            case ItemType.Buff:
                return true;
            case ItemType.Weapon:
            case ItemType.Armor:
                return false;
        }
    }
    public bool Buyable() // true�� �����۸� ������ ǥ��, ex: Ư�������� ��� �رݵǴ� �������̸� Buyable�� false���� true�� ����
    {
        switch (type)
        {
            case ItemType.Potion:
                {
                    return false;
                }
            default:
                return true;
        }
    }
    public ItemType Type { get { return type; } }
    public string ItemName { get { return itemName; } }
    public Sprite Sprite { get { return sprite; } }
    public List<ItemEffect> Efts { get { return efts; } }
    public int Power { get { return power; } }
    public List<String> Descriptiion { get { return descriptiion; } }
    public int Amount { get { return amount; } }
    public float AttackRange { get { return attackRange; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public PropertyType WeaponProperty { get { return weaponProperty; } }
    public int PropertyAmount { get { return propertyAmount; } }
    public int Weight { get { return weight; } }
    public int Price { get { return price; } }
}

public enum ItemType
{
    Weapon,
    Armor,
    Potion,
    Buff,
    Skill
}

public enum PropertyType
{
    Non,
    Bleeding,
    Poisoning,
    Corruption

}


