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
        for (int i = 0; i < itmDB.itemDB.Count; i++)
        {
            //if (itmDB.itemDB[i].itemName.Equals(slots[i].itemName)) continue;

            GameObject go = Instantiate(slotPrefab);
            go.transform.SetParent(slotHolder);
            go.GetComponent<ShopSlot>().SetItem(itmDB.itemDB[i]);

        }
    }

}
