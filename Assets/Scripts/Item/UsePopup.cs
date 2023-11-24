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
    [SerializeField] private Button btnDump;

    public int slotnum;
    private InventoryUI invenUI;

    // Start is called before the first frame update
    void OnEnable()
    {
        invenUI = InventoryUI.instance;
        //btnBack.onClick.AddListener(Close);
        btnCancel.onClick.AddListener(Close);
        btnConform.onClick.AddListener(Confirm);
        btnDump.onClick.AddListener(Dump);
    
    }

    private void OnDisable()
    {
        btnCancel.onClick.RemoveAllListeners();
        btnConform.onClick.RemoveAllListeners();
        btnDump.onClick.RemoveAllListeners();
    }

    public void SetPopup(string title, string content, int slotnum)
    {
        txtTitle.text = "";
        if (invenUI.slots[slotnum].item.CurItem.IsStackable())
        {
            txtTitle.text = title;
            txtContent.text = $"{content}\n사용하시겠습니까?";
            confirmTxt.text = "사용한다";

        } else // 장착 가능 아이템
        {
            txtTitle.text = title;
            txtContent.text = $"{content}\n장착하시겠습니까?";
            confirmTxt.text = "장착한다";

        }
        this.slotnum = slotnum;

    }

    void Confirm()
    {
        if (invenUI.slots[slotnum] != null)
        {
            invenUI.slots[slotnum].ApplyUse();
        }

        Close();
    }

    void Close()
    {
        gameObject.SetActive(false);
    }

    void Dump()
    {
        Inventory.instance.RemoveItem(slotnum);
        Debug.Log("덤프");
        Close();
    }
}
