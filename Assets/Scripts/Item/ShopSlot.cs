using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    public Image itemIcon;
    public string itemName;
    public string itemDescription;
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
        itemName = shopItem.itemName;
        itemDescription = shopItem.description;
    }

    public void BuyItem()
    {
        // °ñµå ÁöºÒ
        Inventory.instance.AddItem(shopItem);
    }
}
