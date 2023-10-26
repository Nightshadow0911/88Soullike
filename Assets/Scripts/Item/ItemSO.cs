using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    public ItemType type;
    public string itemName;
    public Sprite sprite;
    public List<ItemEffect> efts;
    public int power; // weapon : ���ݷ�, armor : ����, potion : ȸ���� �� '���� ��ġ'
    public string explane;
}

public enum ItemType
{
    Weapon,
    Armor,
    Potion
}
