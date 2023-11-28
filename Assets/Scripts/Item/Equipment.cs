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

        switch (newItem.Type)
        {
            case ItemType.Weapon:
                {
                    if (equipItemList[WEAPON] != null)
                    {
                        UnEquipItem(WEAPON);
                        equipItemList[WEAPON] = newItem;
                    }
                    else
                    {
                        equipItemList[WEAPON] = newItem;
                    }
                    UpdateStatus(WEAPON);

                }
                break;
            case ItemType.Armor:
                {
                    if (equipItemList[ARMOR] != null)
                    {
                        UnEquipItem(ARMOR);
                        equipItemList[ARMOR] = newItem;
                    }
                    else
                    {
                        equipItemList[ARMOR] = newItem;

                    }
                    UpdateStatus(ARMOR);

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
            case WEAPON:
                characterStats.NormalAttackDamage += equipItemList[equipIndex].Power;
                characterStats.AttackSpeed += equipItemList[equipIndex].AttackSpeed;
                characterStats.AttackRange += equipItemList[equipIndex].AttackRange;
                characterStats.EquipWeight += equipItemList[equipIndex].Weight;
                characterStats.PropertyDamage += equipItemList[equipIndex].PropertyAmount;
                characterStats.WeightSpeed();
                break;
            case ARMOR:
                characterStats.CharacterDefense += equipItemList[equipIndex].Power;
                characterStats.EquipWeight += equipItemList[equipIndex].Weight;
                characterStats.PropertyDefense += equipItemList[equipIndex].PropertyAmount;
                characterStats.WeightSpeed();
                break;
        }
    }
    public void UnEquipItem(int equipIndex)
    {
        //if (equipItemList[equipIndex] == null) return;

        switch (equipIndex)
        {
            case WEAPON:
                characterStats.NormalAttackDamage -= equipItemList[equipIndex].Power;
                characterStats.AttackSpeed -= equipItemList[equipIndex].AttackSpeed;
                characterStats.AttackRange -= equipItemList[equipIndex].AttackRange;
                characterStats.EquipWeight -= equipItemList[equipIndex].Weight;
                characterStats.PropertyDamage -= equipItemList[equipIndex].PropertyAmount;
                break;
            case ARMOR:
                characterStats.CharacterDefense -= equipItemList[equipIndex].Power;
                characterStats.EquipWeight -= equipItemList[equipIndex].Weight;
                characterStats.PropertyDefense -= equipItemList[equipIndex].PropertyAmount;
                break;
        }

        equipItemList[equipIndex] = null;
    }
}
