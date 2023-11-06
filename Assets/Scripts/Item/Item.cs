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
    public string description;
    public int amount;


    public bool Use() //사용 아이템 사용
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
        amount--;
        isUsed = true;
        return isUsed; // 아이템 사용 성공 여부
    }

    public void Equip() // 장비 아이템 장착
    {
        // type을 검사해서 armor일때랑 weapon일때로 나눔
        // EquipUI의 armorSlot이나 weaponSlot의 Item이 null이면 장착할건지 묻기 null이 아니면 교체할건지 묻기(장착중인 아이템 나오기)
        Equipment.instance.ChangeEquipItem(this);
        
    }


}
