using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItems : MonoBehaviour
{
    public Item item;
    public SpriteRenderer image;

    public void SetItem(Item fieldItem)
    {
        item = fieldItem;
        item.Init();
        image.sprite = item.Sprite;
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
