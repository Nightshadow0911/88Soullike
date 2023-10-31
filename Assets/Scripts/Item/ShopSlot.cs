using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public GameObject buyBtn;

    public Item shopItem;

    public void SetItem(ItemSO item)
    {
        shopItem.curItem = item;
        shopItem.itemName = item.itemName;
        shopItem.sprite = item.sprite;
        shopItem.type = item.type;
        shopItem.power = item.power;
        shopItem.description = item.descriptiion;
        shopItem.efts = item.efts;
        shopItem.amount = item.amount;

        itemIcon.sprite = shopItem.sprite;
        itemName.text = shopItem.itemName;
        itemDescription.text = shopItem.description;
    }

    public void BuyItem()
    {
        // °ñµå ÁöºÒ
        Inventory.instance.AddItem(shopItem);
    }
}
