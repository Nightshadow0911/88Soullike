using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/Item", order = 0)]
public class ItemSO : MonoBehaviour
{
    public string itemName;
    public Sprite sprite;
    public ItemType type;
    public int power; // weapon : ���ݷ�, armor : ����, potion : ȸ���� �� '���� ��ġ'
    public string explane;
}

public enum ItemType
{
    Weapon,
    Armor,
    Potion
}
