using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public GameObject settableListPanel;
    public Transform settableListHolder;
    //public Item[] settableItem;
    public GameObject settablePrefab;
    public int setSlotIndex;

    public EquipmentSlot weaponSlot;
    public EquipmentSlot armorSlot;
    public quickSlot[] quickSlots = new quickSlot[3];

    public static EquipmentUI instance;
    private Equipment equipment;

    private void Start()
    {
        instance = this; // юс╫ц
        equipment = Equipment.instance;

    }

    public void DrawEquipSlot()
    {
        if (equipment.equipItemList[0].CurItem != null)
        {
            weaponSlot.item = equipment.equipItemList[0];
            weaponSlot.UpdateSlotUI();
        }

        if (equipment.equipItemList[1].CurItem != null)
        {
            armorSlot.item = equipment.equipItemList[1];
            armorSlot.UpdateSlotUI();
        }
    }

    public void DrawquickSlot()
    {
        for(int i = 0; i < Inventory.instance.items.Count; i++)
        {
            if (Inventory.instance.items[i].CurItem.IsStackable())
            {
                GameObject go = Instantiate(settablePrefab);
                go.transform.SetParent(settableListHolder);
                go.transform.GetChild(0).GetComponent<Image>().sprite = Inventory.instance.items[i].Sprite;
                go.transform.GetChild(0).transform.localScale = Vector3.one * 0.5f;
                go.GetComponent<SettableSlot>().itemIndex = i;
            }
            
            //settableItem[i] = Inventory.instance.items[i];
            //go.GetComponent<Item>().sprite = Inventory.instance.items[i].sprite;

        }
    }
}
