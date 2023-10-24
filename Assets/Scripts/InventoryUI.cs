using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject inventoryPanel;
    bool activeInventory = false;

    public Slot[] slots;
    public Transform slotHolder;


    private void Start()
    {
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        AddSlot();
        inventoryPanel.SetActive(activeInventory);
    }

    private void SlotChange(int val)
    {
        Debug.Log(inven.SlotCount);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotnum = i;
            if (i < inven.SlotCount)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // 이후 new Input System 사용
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }

    public void AddSlot() // 이후 특정 아이템 획득 or 챕터 클리어마다 슬롯 개수를 늘려줘도 좋을듯
    {
        inven.SlotCount += 4;
        // inven.SlotCount += 8;
    }

    private void RedrawSlotUI()
    {
        Debug.Log(inven.SlotCount+1);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
    }
}
