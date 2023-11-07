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
        txtTitle.text = title;
        txtContent.text = content;

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
