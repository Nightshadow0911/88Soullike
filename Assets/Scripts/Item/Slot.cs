using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public int slotnum;
    public Item item;
    public Image itemIcon;

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.sprite;
        itemIcon.transform.localScale = Vector3.one * 0.6f;
        itemIcon.gameObject.SetActive(true);
        GetComponentInChildren<DraggableItem>().parentPreviousDrag = transform;
        GetComponentInChildren<DraggableItem>().parentAfterDrag = transform;
    }
    public void RemoveSlot()
    {
        item = null;
        itemIcon.gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        else
        {
            if (item.curItem == null) return;
        }
        

        InventoryUI.instance.usePanel.gameObject.SetActive(true);
        InventoryUI.instance.usePanel.GetComponent<UIPopup>().slotnum = slotnum;
    }

    public void ApplyUse()
    {
        if (item == null) return;

        bool isUse = item.Use(); //������ ȿ�� ��
        if (isUse) // ���Ǹ� ������ ������ �ʱ�Ȱ
        {
            Inventory.instance.RemoveItem(slotnum);
        }
    }


}
