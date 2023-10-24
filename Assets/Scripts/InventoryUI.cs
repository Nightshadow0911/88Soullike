using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    bool activeInventory = false;


    private void Start()
    {
        inventoryPanel.SetActive(activeInventory);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) // 이후 new Input System 사용
        {
            activeInventory = !activeInventory;
            inventoryPanel.SetActive(activeInventory);
        }
    }
}
