using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]

public class SavePoint : MonoBehaviour
{
    [SerializeField] private Travel travel;
    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject alert;
    private bool awke = false;

    private void Awake()
    {
        travel.position = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && alert.activeSelf)
        {
            if (!awke)
            {
                SaveMenuManager.instance.AddTravel(travel);
                awke = true;
            }
            SaveMenuManager.instance.ActiveMenu(saveMenu);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            alert.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            alert.SetActive(false);
        }
    }
}
