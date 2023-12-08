using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FItemInformationUI : MonoBehaviour
{
    public static FItemInformationUI instance;

    public Item selectedItem;
    public Skill selectedSkill;
    [SerializeField] private GameObject[] itemPanel = new GameObject[2];
    [SerializeField] private GameObject centerPanel;

    [Header("장비 아이템")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text property;
    [SerializeField] private TMP_Text weight;
    [SerializeField] private TMP_Text attackSpeed;
    [SerializeField] private TMP_Text attackRange;
    [SerializeField] private TMP_Text itemPower;
    [SerializeField] private TMP_Text PropertyAmount;
    [SerializeField] private TMP_Text description;

    [Header("일반 아이템")]
    [SerializeField] private Image secondItemImage;
    [SerializeField] private TMP_Text secondItemName;
    [SerializeField] private TMP_Text amount;
    [SerializeField] private TMP_Text amountLimit;
    [SerializeField] private TMP_Text secondItemPower;
    [SerializeField] private TMP_Text secondDescription;

    [SerializeField] private GameObject attackSpeedTxt;
    [SerializeField] private GameObject attackRangeTxt;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }


    public void Init()
    {
        Debug.Log(selectedItem.CurItem.ItemName);
        //if (selectedItem.CurItem == null) return;
        if ((selectedItem.Type == ItemType.Weapon) || (selectedItem.Type == ItemType.Armor))
        {
            Debug.Log("ff"+selectedItem.ItemName);

            OpenPanel(0);
            Debug.Log(selectedItem.ItemName);

            itemImage.sprite = selectedItem.Sprite;
            itemName.text = $"{selectedItem.ItemName}";


            property.text = $"{selectedItem.WeaponProperty}";
            weight.text = $"{selectedItem.Weight}";

            if(selectedItem.Type == ItemType.Weapon)
            {
                attackSpeedTxt.SetActive(true);
                attackRangeTxt.SetActive(true);
                attackSpeed.text = $"{selectedItem.AttackSpeed}";
                attackRange.text = $"{selectedItem.AttackRange}";
            } else
            {
                attackSpeedTxt.SetActive(false);
                attackRangeTxt.SetActive(false);
                attackSpeed.text = $"";
                attackRange.text = $"";
            }

            itemPower.text = $"{selectedItem.Power}";
            PropertyAmount.text = $"{selectedItem.PropertyAmount}";

            description.text = "";
            for (int i = 0; i < selectedItem.Description.Count; i++)
            {
                description.text += $"{selectedItem.Description[i]}\n";
            }
        } else
        {
            Debug.Log("dd"+selectedItem.ItemName);

            OpenPanel(1);
            Debug.Log(selectedItem.ItemName);
            secondItemImage.sprite = selectedItem.Sprite;
            secondItemName.text = $"{selectedItem.ItemName}";

            amount.text = "소지 수";
            amountLimit.text = $"{selectedItem.Amount} / {selectedItem.CurItem.Amount}";
            

            secondDescription.text = "";
            for (int i = 0; i < selectedItem.Description.Count; i++)
            {
                secondDescription.text += $"{selectedItem.Description[i]}\n";
            }
        }
    }

    public void InitSkill()
    {
        OpenPanel(1);
        secondItemImage.sprite = selectedSkill.SkillIcon;
        secondItemName.text = $"{selectedSkill.SkillName}";

        amount.text = "";
        amountLimit.text = "";
        //secondItemPower.text = $"{selectedSkill.Power}"; // 아이템 효과라고 생각하자?

        secondDescription.text = "";
        for (int i = 0; i < selectedSkill.description.Count; i++)
        {
            secondDescription.text += $"{selectedSkill.description[i]}\n";
        }

    }
    void OpenPanel(int index)
    {
        foreach(GameObject panel in itemPanel)
        {
            panel.SetActive(false);
        }
        itemPanel[index].SetActive(true);
        centerPanel.SetActive(false);

    }
    public void ClearInform()
    {
        /*        foreach (GameObject panel in itemPanel)
                {
                    panel.SetActive(false);
                }*/
       // centerPanel.SetActive(true);
    }


}
