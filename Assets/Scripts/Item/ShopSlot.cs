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

    [SerializeField] private Item shopItem;
    [SerializeField] private Inventory inven;

    private void Start()
    {
        inven = Inventory.instance;
    }

    public void SetItem(Item newItem)
    {
        //newItem.Init();
        shopItem = newItem;
        //shopItem.Init();

        itemIcon.sprite = shopItem.Sprite;
        itemName.text = shopItem.ItemName;
        itemDescription.text = "";
        for (int i = 0; i < shopItem.Description.Count; i++)
        {
            itemDescription.text += $"{shopItem.Description[i]}\n";
        }
    }
/*    public void SetItem(ItemSO item)
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

    }*/

    public void BuyItem()
    {
        // ��� ����
        if(shopItem.Price < inven.SoulCount)
        {
            inven.SoulCount -= shopItem.Price;
            Inventory.instance.AddItem(shopItem);
            Debug.Log(inven.SoulCount);
        } else
        {
            Debug.Log("�ҿ��� �����մϴ�.");
        }
    }
}
