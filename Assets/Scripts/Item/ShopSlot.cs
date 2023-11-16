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
    [SerializeField] private Inventory inven;

    private void Start()
    {
        inven = Inventory.instance;
    }

    public void SetItem(ItemSO item)
    {
        shopItem.curItem = item;
        shopItem.itemName = item.ItemName;
        shopItem.sprite = item.Sprite;
        shopItem.type = item.Type;
        shopItem.power = item.Power;
        shopItem.description = item.Descriptiion;
        shopItem.efts = item.Efts;
        shopItem.amount = item.Amount;
        shopItem.price = item.Price;

        itemIcon.sprite = shopItem.sprite;
        itemName.text = shopItem.itemName;
        itemDescription.text = "";
        for (int i = 0; i < shopItem.description.Count; i++)
        {
            itemDescription.text += $"{shopItem.description[i]}\n";

        }

    }

    public void BuyItem()
    {
        // 골드 지불
        if(shopItem.price < inven.SoulCount)
        {
            inven.SoulCount -= shopItem.price;
            Inventory.instance.AddItem(shopItem);
            Debug.Log(inven.SoulCount);
        } else
        {
            Debug.Log("소울이 부족합니다.");
        }
    }
}
