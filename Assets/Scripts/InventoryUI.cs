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

    public Slot[] slots; // 리스트로 바꾸면 추가하는 방식도 가능

    public Transform slotHolder;
    public GameObject slotPrefab;


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
        /*if (inven.SlotCount > slots.Length) // 슬롯을 추가할 때 사용
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject sl = Instantiate(slotPrefab, transform.position, Quaternion.identity);
                sl.transform.SetParent(slotHolder);
            }
            
        }*/
        Debug.Log(inven.SlotCount);
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotnum = i;
            Debug.Log(i);
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
        Debug.Log("여기");
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        Debug.Log("하나");
        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
        Debug.Log("둘");
    }
}
