using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    [SerializeField] private GameObject UI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && UI.activeSelf)
        {
            SaveMenuManager.instance.DeActiveMenu();
        }
    }

    public void RequestActive()
    {
        SaveMenuManager.instance.ActiveMenu(UI);
    }
}
