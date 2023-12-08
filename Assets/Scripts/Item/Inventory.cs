using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

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

    public NPCSentence currentNPC;

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> items = new List<Item>();

    private int slotCount;
    [SerializeField] private int soulCount = 1000;
    public int SlotCount
    {
        get => slotCount;
        set
        {
            slotCount = value;
            onSlotCountChange.Invoke(slotCount);
        }
    }
    public int SoulCount
    {
        get { return soulCount; }
        set { soulCount = value; }
    }

    void Start()
    {
        StartItem();
    }

    public bool AddItem(Item item)
    {
        if (items.Count < SlotCount)
        {
            if (item.CurItem.IsStackable())
            {
                bool itemAlreadyInInventory = false;
                foreach(Item inventoryItem in items)
                {
                    if(inventoryItem.CurItem.ItemName == item.CurItem.ItemName)
                    {
                        inventoryItem.Amount += item.CurItem.Amount;
                        itemAlreadyInInventory = true;
                    }
                }
                if(!itemAlreadyInInventory)
                {
                    items.Add(item);
                }

            }
            else
            {
                items.Add(item);
            }

            onChangeItem?.Invoke(); // if(onChangeItem != null)
            return true;
        }
        Debug.Log("인벤토리를 비워주세요");
        return false;
    }
    public void RemoveItem(int index)
    {
        if (index >= items.Count) return;
        items.RemoveAt(index);
        onChangeItem?.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FieldItem"))
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            if (AddItem(fieldItems.GetItem()))
            {
                fieldItems.DestroyItem();
            }
        }
    }

    public void StartItem()
    {
        ItemDatabase Ib = ItemDatabase.instance;
        Item newItem = new Item();

        for (int i = 0; i < Ib.itemDB.Count; i++)
        {
            if (!Ib.itemDB[i].Buyable() && Ib.itemDB[i].Type == ItemType.Potion)
            {
                newItem.CurItem = Ib.itemDB[i];
                newItem.Init();
                AddItem(newItem);
            }
        }
    }
}
