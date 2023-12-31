using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    Inventory inven;

    public GameObject usePanel;
    public GameObject shopPanel;

    public Slot[] slots; // 리스트로 바꾸면 추가하는 방식도 가능

    public Transform slotHolder;
    public GameObject slotPrefab;
    [SerializeField] private TMP_Text soulTxt;


    private void Awake()
    {



    }

    private void Start()
    {
        instance = this; // 임시
        inven = Inventory.instance;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        AddSlot();
        soulTxt.text = $"{inven.SoulCount:N0}";
        //inventoryPanel.SetActive(activeInventory);
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

    public void AddSlot() // 이후 특정 아이템 획득 or 챕터 클리어마다 슬롯 개수를 늘려줘도 좋을듯
    {
        inven.SlotCount += 26;
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
        soulTxt.text = $"{inven.SoulCount:N0}";
    }
}
