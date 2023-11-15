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

    private CharacterStats characterStats;

    private void Start()
    {
        characterStats = GameManager.Instance.playerStats;
    }



    public void ChangeEquipItem(Item newItem)
    {

        switch (newItem.type)
        {
            case ItemType.Weapon:
                if (equipItemList[WEAPON] != null)
                {
                    UnEquipItem(0);
                    equipItemList[WEAPON] = newItem;
                    UpdateStatus(0);
                }
                else
                {
                    equipItemList[WEAPON] = newItem;
                    UpdateStatus(0);

                }
                break;
            case ItemType.Armor:
                if (equipItemList[ARMOR] != null)
                {
                    UnEquipItem(1);
                    equipItemList[ARMOR] = newItem;
                    UpdateStatus(1);
                }
                else
                {
                    equipItemList[ARMOR] = newItem;
                    UpdateStatus(1);

                }
                break;
            default:
                return;
        }
        EquipmentUI.instance.DrawEquipSlot();
    }

    public void UpdateStatus(int equipIndex)
    {
        switch (equipIndex)
        {
            case 0:
                characterStats.NormalAttackDamage += equipItemList[equipIndex].power;
                break;
            case 1:
                characterStats.CharacterDefense += equipItemList[equipIndex].power;
                break;
        }
    }
    public void UnEquipItem(int equipIndex)
    {
        //if (equipItemList[equipIndex] == null) return;

        switch (equipIndex)
        {
            case 0:
                characterStats.NormalAttackDamage -= equipItemList[equipIndex].power;
                break;
            case 1:
                characterStats.CharacterDefense -= equipItemList[equipIndex].power;
                break;
        }
        equipItemList[equipIndex] = null;
    }
}
