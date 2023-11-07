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
    [SerializeField] private GameObject menuUI;
    private bool awke = false;

    private void Update()
    {
        if (!alertUI.activeSelf)
            return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            // if (!awke)
            // {
            //     awke = true;
                TravelManager.instance.AddTravel(name, travelNumber, transform.position);
                //연출
            //}
            menuUI.SetActive(true);
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
            menuUI.SetActive(false);
        }
    }
}
