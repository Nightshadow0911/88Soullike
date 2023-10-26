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
        item.itemName = fieldItem.itemName;
        item.sprite = fieldItem.sprite;
        item.type = fieldItem.type;
        item.power = fieldItem.power;
        item.explane = fieldItem.explane;
        item.efts = fieldItem.efts;
        item.amount = fieldItem.amount;

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
