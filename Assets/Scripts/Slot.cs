using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerUpHandler
{
    public int slotnum;
    public Item item;
    public Image itemIcon;

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.sprite;
        itemIcon.transform.localScale = Vector3.one * 0.6f;
        itemIcon.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (item == null) return;

        bool isUse = item.Use();
        if(isUse)
        {
            Inventory.instance.RemoveItem(slotnum);
        }
    }
}
