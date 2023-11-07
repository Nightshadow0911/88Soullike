using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    #region Singleton
    public static Equipment instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion
    private const int WEAPON = 0, ARMOR = 1;

    public Item[] equipItemList = new Item[2];
    public Item[] quickSlotList = new Item[3];

    public void ChangeEquipItem(Item newItem)
    {

        switch (newItem.type)
        {
            case ItemType.Weapon:
                if (equipItemList[WEAPON] != null)
                {
                    UnEquipItem(0);
                    equipItemList[WEAPON] = newItem;
                }
                else
                {
                    equipItemList[WEAPON] = newItem;
                }
                break;
            case ItemType.Armor:
                if (equipItemList[ARMOR] != null)
                {
                    UnEquipItem(1);
                    equipItemList[ARMOR] = newItem;
                }
                else
                {
                    equipItemList[ARMOR] = newItem;
                }
                break;
            default:
                return;
        }
        UpdateStatus();
        EquipmentUI.instance.DrawEquipSlot();
    }

    public void UpdateStatus()
    {
        //현재 장착중인 무기와 아머의 power를 플레이어 스탯에 반영, 아래는 예시
        // PlayerStat.instance.atk += equipItemList[WEAPON].power;
        // PlayerStat.instance.def += equipItemList[ARMOR].power;
        // null일경우 return or += 0;
    }
    public void UnEquipItem(int equipIndex)
    {
        //장착아이템 능력치를 여기서 뺄지? 플레이어에서 관리할지?
        //PlayerStat.instance.atk -= equipItemLsit[equipIndex].power;
        equipItemList[equipIndex] = null;
        UpdateStatus();
    }
}
