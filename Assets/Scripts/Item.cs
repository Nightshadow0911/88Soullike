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


/*    public Item() // 생성할때 curItem을 기반으로 초기화 해주고 싶은데 왜 오류가 날까, 일단 쓸 필요 없음
    {
        itemName = curItem.itemName;
        sprite = curItem.sprite;
        type = curItem.type;
        power = curItem.power;
        explane = curItem.explane;
    }*/

    public bool Use() //사용 아이템 사용
    {
        bool isUsed = false;

        foreach(ItemEffect eft in efts)
        {
            isUsed = eft.ExcuteRole(power);
        }
        isUsed = true;
        return isUsed; // 아이템 사용 성공 여부
    }

}
