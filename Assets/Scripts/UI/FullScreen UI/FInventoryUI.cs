using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FInventoryUI : MonoBehaviour
{
    public ItemType selectType; // 타입에 따라 해당 타입만 인벤토리에 정렬하기 위함

    Inventory inven;
    public GameObject selectItemPanel;

    public Slot[] slots;
    public Transform slotHolder;
    public GameObject slotPrefab;


    private void Awake()
    {
        inven = Inventory.instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        AddSlot(24);
    }
    private void SlotChange(int val)
    {
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
        inven.SlotCount += 6;
    }
    public void AddSlot(int addCount)
    {
        inven.SlotCount += addCount;
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
