using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]

public class SavePoint : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private int travelNumber;
    [SerializeField] private GameObject alertUI;
    private bool awke = false;

    private void Update()
    {
        if (!awke)
            SaveMenuManager.instance.AddTravel(name, travelNumber, transform.position);
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 세이브매니저. 메뉴 오픈
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            alertUI.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            alertUI.SetActive(false);
        }
    }
}
