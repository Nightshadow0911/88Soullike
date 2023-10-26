using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public ItemSO curItem; // 이름, 이미지, 타입, 파워, 설명

    public ItemType type;
    public string itemName;
    public Sprite sprite;
    public List<ItemEffect> efts;
    public int power;
    public string explane;
    public int amount;


    public bool Use() //사용 아이템 사용
    {
        bool isUsed = false;

        foreach(ItemEffect eft in efts)
        {
            isUsed = eft.ExcuteRole(power);
        }
        amount--;
        isUsed = true;
        return isUsed; // 아이템 사용 성공 여부
    }



}
