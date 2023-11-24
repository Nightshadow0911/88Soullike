using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public int slotnum;
    public TMP_Text amountTxt;
    public Item item;
    public Image itemIcon;
    [SerializeField] private string descriptions;

    public void UpdateSlotUI()
    {

        itemIcon.sprite = item.Sprite;
        itemIcon.transform.localScale = Vector3.one * 0.7f;
        itemIcon.gameObject.SetActive(true);

        if (item.CurItem.IsStackable())
        {
            amountTxt.text = item.Amount.ToString();
        }
        else
        {
            amountTxt.text = "";
            if ((item == Equipment.instance.equipItemList[0]) || item == Equipment.instance.equipItemList[1]) //플레이어가 현재 장착중인 아이템
            {
                amountTxt.text = "E";
            }
        }

        //if(GetComponentInChildren<DraggableItem>().parentPreviousDrag == null)
        //GetComponentInChildren<DraggableItem>().parentPreviousDrag = transform;
    }
    public void RemoveSlot()
    {
        item = null;
        amountTxt.text = "";
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
            if (item.CurItem == null) return;
        }

        InventoryUI.instance.usePanel.gameObject.SetActive(true);

        for (int i = 0; i < item.Description.Count; i++)
        {
            descriptions += $"{item.Description[i]}\n";

        }
        InventoryUI.instance.usePanel.GetComponent<UsePopup>().SetPopup(item.ItemName, descriptions, slotnum);
    }

    public void ApplyUse()
    {
        if (item == null) return;

        bool isUse = item.Use(); //아이템 효과 사용

        if (isUse && (item.Amount <= 0)) // 모두 사용되면 슬롯의 정보를 초기활
        {
            Inventory.instance.RemoveItem(slotnum);
        }
        else
        {
            UpdateSlotUI();
        }
    }


}
