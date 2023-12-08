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

    FullScreenUIManager fManager;
    FItemInformationUI fInformManager;
    SelectItemUI fSelectUI;
    

    void Awake()
    {

    }
    void Start()
    {
        fManager = FullScreenUIManager.instance;
        fInformManager = fManager.itemInformationUI.GetComponent<FItemInformationUI>();
        fSelectUI = fManager.itemSelectUI.GetComponent<SelectItemUI>();
        Init();
    }

    void Init()
    {
        if (item == null) return;
        itemIcon.gameObject.SetActive(true);
        amountTxt.text = "";
        if(item != null)
        {
            amountTxt.text = $"{item.Amount}";
        }
        itemIcon.sprite = item.Sprite;
        itemIcon.transform.localScale = Vector3.one * 0.9f;
        
    }

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.Sprite;
        itemIcon.transform.localScale = Vector3.one * 0.9f;
        itemIcon.gameObject.SetActive(true);

        if (item.CurItem.IsStackable())
        {
            amountTxt.text = $"{item.Amount}";
        }
        else
        {
            amountTxt.text = "";
            
        }

    }
    public void RemoveSlot()
    {
        item = null;
        amountTxt.text = "";
        itemIcon.gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item.CurItem == null)
        {
            return;
        }

        fManager.itemSelectUI.SetActive(true);
        fSelectUI.SetFunc(slotnum);
        fSelectUI.Init();
        ShowItemInformation();
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

    public void ShowItemInformation()
    {

        fInformManager.selectedItem = item;
        //fInformManager.selectedItem.Init();
        fInformManager.Init();
    }
}
