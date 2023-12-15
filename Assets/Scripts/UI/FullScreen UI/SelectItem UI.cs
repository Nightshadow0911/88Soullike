using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemUI : MonoBehaviour
{
    public TMP_Text useTxt;
    public Button useBtn;
    public Button dropBtn;
    public Button deleteBtn;
    public Button dropAllBtn;
    public Button deleteAllBtn;

    private int slotNum;

    private Inventory inven;
    [SerializeField]private FInventoryUI fInvenUI;
    private FItemInformationUI fInformUI;
    //public Action anyAction;

    private void Start()
    {
        inven = Inventory.instance;
        //fInvenUI = FInventoryUI.instance;
        fInformUI = FItemInformationUI.instance;
        slotNum = 0;

        Init();
    }

    public void SetFunc(int slotNum)
    {
        if (fInvenUI.slots[slotNum].item.CurItem == null) return;

        this.slotNum = slotNum;

        if (fInvenUI.slots[slotNum].item.CurItem.IsStackable())
        {
            useTxt.text = "사용한다";
        } else
        {
            useTxt.text = "장착한다";
        }
    }

    public void Init()
    {
        useBtn.onClick.RemoveAllListeners();
        useBtn.onClick.AddListener(Confirm);

        deleteAllBtn.onClick.RemoveAllListeners();
        deleteAllBtn.onClick.AddListener(Dump);

        deleteBtn.interactable = false;
        dropBtn.interactable = false;
        dropAllBtn.interactable = false;

    }
    void Confirm()
    {
        if (fInvenUI.slots[slotNum] != null)
        {
            fInvenUI.slots[slotNum].ApplyUse();
        }

        Close();
    }

    void Close()
    {
        fInformUI.ClearInform();
        gameObject.SetActive(false);
    }

    void Dump()
    {
        inven.RemoveItem(slotNum);
/*        if (fInvenUI.slots[slotNum].item.CurItem.IsStackable()) // 소모품이면 하나씩 버리기인데
        {

        } else // 장비면 한번에 버리기
        {
            inven.RemoveItem(slotNum);
        }*/
        Debug.Log("덤프");
        Close();
    }
}



