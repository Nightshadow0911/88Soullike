using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> items = new List<Item>();

    private int slotCount;
    public int SlotCount
    {
        get => slotCount;
        set
        {
            slotCount = value;
            onSlotCountChange.Invoke(slotCount);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        slotCount = 4;
    }

    public bool AddItem(Item item)
    {
        if(items.Count < SlotCount)
        {
            items.Add(item);
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
        if(collision.CompareTag("FieldItem"))
        {
            FieldItems fieldItems = collision.GetComponent<FieldItems>();
            if(AddItem(fieldItems.GetItem()))
            {
                fieldItems.DestroyItem();
            }
        }
    }
}
