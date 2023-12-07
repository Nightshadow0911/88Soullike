using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FItemInformationUI : MonoBehaviour
{
    public Item selectedItem;
    [SerializeField] private GameObject[] itemPanel = new GameObject[2];

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



    public void Init()
    {
        if (selectedItem == null) return;
        if ((selectedItem.Type == ItemType.Weapon) || (selectedItem.Type == ItemType.Armor))
        {
            OpenPanel(0);
            itemImage.sprite = selectedItem.Sprite;
            itemName.text = $"{selectedItem.ItemName}";


            property.text = $"{selectedItem.WeaponProperty}";
            weight.text = $"{selectedItem.Weight}";
            attackSpeed.text = $"{selectedItem.AttackSpeed}";
            attackRange.text = $"{selectedItem.AttackRange}";

            itemPower.text = $"{selectedItem.Power}";
            PropertyAmount.text = $"{selectedItem.PropertyAmount}";

            for (int i = 0; i < selectedItem.Description.Count; i++)
            {
                description.text += $"{selectedItem.Description[i]}\n";
            }
        } else
        {
            OpenPanel(1);
            secondItemImage.sprite = selectedItem.Sprite;
            secondItemName.text = $"{selectedItem.ItemName}";


            amount.text = $"{selectedItem.Amount}";
            amountLimit.text = $"{selectedItem.CurItem.Amount}";
            secondItemPower.text = $"{selectedItem.Power}"; // 아이템 효과라고 생각하자?

            for (int i = 0; i < selectedItem.Description.Count; i++)
            {
                secondDescription.text += $"{selectedItem.Description[i]}\n";
            }
        }


    }
    void OpenPanel(int index)
    {
        foreach(GameObject panel in itemPanel)
        {
            panel.SetActive(false);
        }
        itemPanel[index].SetActive(true);

    }


}
