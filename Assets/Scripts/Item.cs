using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public ItemSO curItem; // �̸�, �̹���, Ÿ��, �Ŀ�, ����

    public string itemName;
    public Sprite sprite;
    public ItemType type;
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

    public bool Use()
    {
        return false; // ������ ��� ���� ����
    }

}
