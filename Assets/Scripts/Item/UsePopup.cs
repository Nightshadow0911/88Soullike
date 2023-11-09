using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsePopup : MonoBehaviour
{
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtContent;
    [SerializeField] private TMP_Text confirmTxt;

    //[SerializeField] private Button btnBack;
    [SerializeField] private Button btnConform;
    [SerializeField] private Button btnCancel;

    public int slotnum;

    // Start is called before the first frame update
    void Start()
    {
        //btnBack.onClick.AddListener(Close);
        btnCancel.onClick.AddListener(Close);
        btnConform.onClick.AddListener(Confirm);
    }

    public void SetPopup(string title, string content, int slotnum)
    {
        if (InventoryUI.instance.slots[slotnum].item.curItem.IsStackable())
        {
            txtTitle.text = title;
            txtContent.text = $"[{content}]\n사용하시겠습니까?";
            confirmTxt.text = "사용한다";

        } else // 장착 가능 아이템
        {
            txtTitle.text = title;
            txtContent.text = $"[{content}]\n장착하시겠습니까?";
            confirmTxt.text = "장착한다";

        }
        this.slotnum = slotnum;

    }

    void Confirm()
    {
        if (InventoryUI.instance.slots[slotnum] != null)
        {
            InventoryUI.instance.slots[slotnum].ApplyUse();
        }

        Close();
    }

    void Close()
    {
        gameObject.SetActive(false);
    }
}
