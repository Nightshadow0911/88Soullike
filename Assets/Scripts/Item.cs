using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public ItemSO curItem; // �̸�, �̹���, Ÿ��, �Ŀ�, ����

    public ItemType type;
    public string itemName;
    public Sprite sprite;
    public List<ItemEffect> efts;
    public int power;
    public string explane;


/*    public Item() // �����Ҷ� curItem�� ������� �ʱ�ȭ ���ְ� ������ �� ������ ����, �ϴ� �� �ʿ� ����
    {
        itemName = curItem.itemName;
        sprite = curItem.sprite;
        type = curItem.type;
        power = curItem.power;
        explane = curItem.explane;
    }*/

    public bool Use() //��� ������ ���
    {
        bool isUsed = false;

        foreach(ItemEffect eft in efts)
        {
            isUsed = eft.ExcuteRole(power);
        }
        isUsed = true;
        return isUsed; // ������ ��� ���� ����
    }

}
