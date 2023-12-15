using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FInventoryUI : MonoBehaviour
{
    public static FInventoryUI instance;
    public ItemType selectType; // Ÿ�Կ� ���� �ش� Ÿ�Ը� �κ��丮�� �����ϱ� ����

    Inventory inven;
    public GameObject selectItemPanel;
    public TMP_Text quantityLimitValue;

    public Slot[] slots;
    public Transform slotHolder;
    public GameObject slotPrefab;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        inven = Inventory.instance;
        inven.onSlotCountChange += SlotChange;
        inven.onChangeItem += RedrawSlotUI;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        AddSlot(28);
    }
    private void SlotChange(int val)
    {
        //if (inven.items.Count <= 0) return;
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

    public void AddSlot() // ���� Ư�� ������ ȹ�� or é�� Ŭ����� ���� ������ �÷��൵ ������
    {
        inven.SlotCount += 4;
        for (int i = 0; i < 4; i++)
        {
            Instantiate(slotPrefab, slotHolder);
        }
    }
    public void AddSlot(int addCount)
    {
        inven.SlotCount += addCount;
/*        for(int i = 0; i < addCount; i++)
        {
            Instantiate(slotPrefab, slotHolder);
        }*/

    }

    public void RedrawSlotUI()
    {
        //if (inven.items.Count <= 0) return;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.items.Count; i++)
        {
            slots[i].item = inven.items[i];
            slots[i].UpdateSlotUI();
        }
        quantityLimitValue.text = $"{inven.items.Count}/{slots.Length}"; // 28
    }
}
