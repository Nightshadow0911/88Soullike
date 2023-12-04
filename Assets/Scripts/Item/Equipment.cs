using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Skill[] skillSlotList = new Skill[3]; // ��ų ����, 0���� ���� �տ� ����
    public GameObject[] skillIcons = new GameObject[3];
    public Transform skillHolder;

    private PlayerStatusHandler playerStatusHandler;


    private void Start()
    {
        playerStatusHandler = transform.GetComponent<PlayerStatusHandler>(); //
        for (int i = 0; i < skillIcons.Length; i++)
        {
            //skillIcons[i] = skillHolder.GetChild(i).gameObject;
        }
    }

    public void ChageEquipSkill()
    {
        int skillIndex = transform.GetComponent<LastPlayerController>().skillIndex;
        if (skillIndex != 0)
        {
            skillIcons[skillIndex - 1].SetActive(false);
        }
        else
        {
            skillIcons[2].SetActive(false);
        }
        skillIcons[skillIndex].SetActive(true);

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
                    EquipItem(WEAPON);

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
                    EquipItem(ARMOR);

                }
                break;
            default:
                return;
        }
        EquipmentUI.instance.DrawEquipSlot();
    }
    public void EquipItem(int equipIndex) //
    {
        switch (equipIndex)
        {
            case WEAPON:
                playerStatusHandler.UpdateWeapon(equipItemList[equipIndex].Power, equipItemList[equipIndex].AttackSpeed,
                    equipItemList[equipIndex].AttackRange, equipItemList[equipIndex].Weight, equipItemList[equipIndex].PropertyAmount);

                //characterStats.WeightSpeed();
                // ���ݷ�, ���ݼӵ�, ���ݹ���, ����, �Ӽ����ݷ�, 
                break;
            case ARMOR:
                playerStatusHandler.UpdateArmor(equipItemList[equipIndex].Power,
                    equipItemList[equipIndex].Weight, equipItemList[equipIndex].PropertyAmount);

                //characterStats.WeightSpeed();
                break;
        }
    }
    public void UnEquipItem(int equipIndex) //
    {
        if (equipItemList[equipIndex] == null) return;

        switch (equipIndex)
        {
            case WEAPON:
                playerStatusHandler.UpdateWeapon(-equipItemList[equipIndex].Power, -equipItemList[equipIndex].AttackSpeed,
                    -equipItemList[equipIndex].AttackRange, -equipItemList[equipIndex].Weight, -equipItemList[equipIndex].PropertyAmount);
                break;
            case ARMOR:
                playerStatusHandler.UpdateArmor(-equipItemList[equipIndex].Power,
                    -equipItemList[equipIndex].Weight, -equipItemList[equipIndex].PropertyAmount);
                break;
        }

        equipItemList[equipIndex] = null;
    }

    public void ChangeSkill()
    {

    }
}
