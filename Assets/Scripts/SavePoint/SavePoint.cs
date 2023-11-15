using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private GameObject travelPoint;
    [SerializeField] private GameObject alert;
    private bool awke = false;
    
    public static event Action<bool> TravelEvent;

    private void Awake()
    {
        travelPoint.SetActive(false);
        alert.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && alert.activeSelf)
        {
            if (!awke)
            {
                travelPoint.GetComponent<TravelPoint>().SetTravel(name, transform.position);
                TravelEvent?.Invoke(true);
                awke = true;
            }
            CharacterStats stats = GameManager.Instance.playerStats;
            stats.characterHp = stats.MaxHP;
            stats.characterStamina = stats.MaxStemina;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            alert.SetActive(true);
            if (awke)
                TravelEvent?.Invoke(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            alert.SetActive(false);
            TravelEvent?.Invoke(false);
        }
    }
}
