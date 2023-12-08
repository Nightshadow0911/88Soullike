using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Image itemIcon;
    //public TMP_Text itemName;
    //public TMP_Text itemDescription;
    public TMP_Text itemValue;

    [SerializeField] private FItemInformationUI fInformManager;
    [SerializeField] private Item shopItem;
    [SerializeField] private Inventory inven;

    private void Start()
    {
        fInformManager = FItemInformationUI.instance;
        inven = Inventory.instance;
    }

    public void SetItem(Item newItem)
    {
        shopItem = newItem;

        itemIcon.sprite = shopItem.Sprite;
        itemValue.text = $"{shopItem.Price}";
    }

    public void BuyItem()
    {
        // ��� ����
        if(shopItem.Price < inven.SoulCount)
        {
            inven.SoulCount -= shopItem.Price;
            inven.AddItem(shopItem);
            
        }
    }

    public void ShowInformation() // Onclick 이벤트
    {
        fInformManager.selectedItem = shopItem;
        fInformManager.selectedItem.Init();
        fInformManager.Init();

    }
}
