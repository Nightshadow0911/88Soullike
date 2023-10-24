using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public ItemType type;
    public int power; // weapon : 공격력, armor : 방어력, potion : 회복량 등 '메인 수치'
    public string explane;
}

public enum ItemType
{
    Weapon,
    Armor,
    Potion
}
