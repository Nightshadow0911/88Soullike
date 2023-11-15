using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
    public SpriteRenderer image;

    public void SetItem(ItemSO fieldItem)
    {
        item.curItem = fieldItem;
        item.itemName = fieldItem.ItemName;
        item.sprite = fieldItem.Sprite;
        item.type = fieldItem.Type;
        item.power = fieldItem.Power;
        item.description = fieldItem.Descriptiion;
        item.efts = fieldItem.Efts;
        item.amount = fieldItem.Amount;

        image.sprite = item.sprite;
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroyItem() // 이후 오브젝트 풀링 사용
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

}
