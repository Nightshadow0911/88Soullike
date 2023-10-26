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
    public ItemType type;
    public string itemName;
    public Sprite sprite;
    public List<ItemEffect> efts;
    public int power; // weapon : 공격력, armor : 방어력, potion : 회복량 등 '메인 수치'
    public string explane;
    [Range(1,3)]public int amount;
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
}

public enum ItemType
{
    Weapon,
    Armor,
    Potion
}
