using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public ShopSlot[] slots;
    public Transform slotHolder;
    public GameObject slotPrefab;
    ItemDatabase itmDB;

    private void Start()
    {
        itmDB = ItemDatabase.instance;
        slots = slotHolder.GetComponentsInChildren<ShopSlot>();
        SetShop();

    }

    void SetShop() // 첫 시작이나 챕터 깰 때 상위 아이템 열림
    {
        
        foreach(Transform tr in slotHolder) // 초기화
        {
            Destroy(tr.gameObject);
        }

        for (int i = 0; i < itmDB.itemDB.Count; i++)
        {
            if (!itmDB.itemDB[i].Buyable()) continue;
            Item newItem = new Item();
            newItem.CurItem = itmDB.itemDB[i];
            newItem.Init();

            GameObject go = Instantiate(slotPrefab, slotHolder);
            go.GetComponent<ShopSlot>().SetItem(newItem);

        }
    }

}
