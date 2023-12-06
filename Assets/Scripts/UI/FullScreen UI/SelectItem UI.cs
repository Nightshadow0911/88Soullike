using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemUI : MonoBehaviour
{
    public Button useBtn;
    public Button dropBtn;
    public Button deleteBtn;
    public Button dropAllBtn;
    public Button deleteAllBtn;

    public Action anyAction;

    public void SetFunc(Action func)
    {

    }

    /*void Init()
    {
        useBtn.onClick.AddListener(SetFunc(anyAction));
    }*/
}
