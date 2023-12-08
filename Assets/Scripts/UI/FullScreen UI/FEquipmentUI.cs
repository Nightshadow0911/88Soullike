using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FEquipmentUI : MonoBehaviour
{
    public static FEquipmentUI instance;
    public int setSlotIndex;

    public EquipmentSlot weaponSlot;
    public EquipmentSlot armorSlot;
    public EquipmentSlot potionSlot;
    public EquipmentSlot[] skillSlot = new EquipmentSlot[2];
    public GameObject itemSlots; 

    private Equipment equipment;
    private Inventory inven;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        equipment = Equipment.instance;
        inven = Inventory.instance;

        DrawEquipSlot();
        DrawSkillSlot();
    }

    public void DrawEquipSlot()
    {
        if (equipment.equipItemList[0].CurItem != null)
        {
            //weaponSlot.item = equipment.equipItemList[0];
            weaponSlot.item.CurItem = equipment.equipItemList[0].CurItem;
            weaponSlot.item.Init();
            weaponSlot.UpdateSlotUI();
        }

        if (equipment.equipItemList[1].CurItem != null)
        {
            armorSlot.item = equipment.equipItemList[1];
            armorSlot.UpdateSlotUI();
        }

        foreach(Item it in inven.items)
        {
            if(it.Type == ItemType.Potion)
            {
                potionSlot.item = it;
                potionSlot.UpdateSlotUI();
                break;
            }
        }

    }

    public void DrawSkillSlot()
    {
        Debug.Log(equipment.skillSlotList[0].SkillName);
        if (equipment.skillSlotList.Length > 0)
        {
            for(int i = 0; i < equipment.skillSlotList.Length; i++)
            {
                if (equipment.skillSlotList[i] == null) break;

                skillSlot[i].skill = new Skill();

                skillSlot[i].skill.CurSkill = equipment.skillSlotList[i];
                skillSlot[i].skill.Init();
                skillSlot[i].UpdateSkillUI();
            }

        }
    }

/*    public void DrawquickSlot()
    {
        for (int i = 0; i < Inventory.instance.items.Count; i++)
        {
            if (Inventory.instance.items[i].CurItem.IsStackable())
            {
                GameObject go = Instantiate(settablePrefab);
                go.transform.SetParent(settableListHolder);
                go.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.instance.items[i].Sprite;
                go.transform.GetChild(0).transform.localScale = Vector3.one * 0.5f;
                go.GetComponent<SettableSlot>().itemIndex = i;
            }

        }
    }*/
}
