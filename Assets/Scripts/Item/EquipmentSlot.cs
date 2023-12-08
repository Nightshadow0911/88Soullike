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
    [SerializeField] private FItemInformationUI fInformManager;

    private void Start()
    {
        fInformManager = FItemInformationUI.instance;
    }
    public void UpdateSlotUI()
    {
        //itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = item.Sprite;
        itemIcon.transform.localScale = Vector3.one * 0.9f;
    }
    public void UpdateSkillUI()
    {
        //itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = skill.SkillIcon;
        itemIcon.transform.localScale = Vector3.one * 0.9f;
    }

    public void showInventory()
    {
        //���� Type�� �´� �κ��丮�� �����ؼ� ����
    }
    public void ShowInformation() // Onclick �̺�Ʈ
    {
        if (item == null) return;

        if(slotType == ItemType.Skill)
        {
            fInformManager.selectedSkill = skill;
            fInformManager.selectedSkill.Init();
            fInformManager.InitSkill();
        }
        else
        {
            fInformManager.selectedItem = item;
            fInformManager.selectedItem.Init();
            fInformManager.Init();
        }


    }
}
