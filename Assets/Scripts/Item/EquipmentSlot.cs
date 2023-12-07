using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public ItemType slotType;
    public Item item;
    public Image itemIcon;
    public Skill skill;

    public void UpdateSlotUI()
    {
        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = item.Sprite;
        itemIcon.transform.localScale = Vector3.one * 0.9f;
    }
    public void UpdateSkillUI()
    {
        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = skill.SkillIcon;
        itemIcon.transform.localScale = Vector3.one * 0.9f;
    }

    public void showInventory()
    {
        //슬롯 Type에 맞는 인벤토리로 정렬해서 열기
    }
}
