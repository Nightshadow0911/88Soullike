using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    Inventory inven;

    public GameObject inventoryPanel;
    public GameObject usePanel;
    bool activeInventory = false;

    public Slot[] slots; // ����Ʈ�� �ٲٸ� �߰��ϴ� ��ĵ� ����

    public Transform slotHolder;
    public GameObject slotPrefab;


    private void Start()
    {
        instance = this; // �ӽ�
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        AddSlot();
        inventoryPanel.SetActive(activeInventory);
    }

    private void SlotChange(int val)
    {
        /*if (inven.SlotCount > slots.Length) // ������ �߰��� �� ���
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject sl = Instantiate(slotPrefab, transform.position, Quaternion.identity);
                sl.transform.SetParent(slotHolder);
            }
            
        }*/

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
        if (Input.GetKeyDown(KeyCode.I)) // ���� new Input System ���
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }

    public void AddSlot() // ���� Ư�� ������ ȹ�� or é�� Ŭ����� ���� ������ �÷��൵ ������
    {
        inven.SlotCount += 4;
    }

    private void RedrawSlotUI()
    {
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
