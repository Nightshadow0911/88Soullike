using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private Travel travel; // 빠른 이동 정보
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
                SaveMenuManager.instance.AddTravel(travel); // 빠른 이동 추가
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
