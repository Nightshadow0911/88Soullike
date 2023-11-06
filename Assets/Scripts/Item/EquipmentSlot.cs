using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public Item item;
    public Image itemIcon;

    public void UpdateSlotUI()
    {
        itemIcon.sprite = item.sprite;
        itemIcon.transform.localScale = Vector3.one * 0.7f;
        itemIcon.gameObject.SetActive(true);
    }
}
