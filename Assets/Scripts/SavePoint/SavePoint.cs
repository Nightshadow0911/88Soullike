using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private string pointName;
    [SerializeField] private GameObject travelPoint;
    [SerializeField] private GameObject alert;
    [SerializeField] private GameObject awakeEffect;
    [SerializeField] private LayerMask playerLayer;
    private bool isAwake = false;
    
    public static event Action<bool> TravelEvent;

    private void Awake()
    {
        alert.SetActive(false);
        awakeEffect.SetActive(false);
        travelPoint.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && alert.activeSelf)
        {
            if (!isAwake)
            {
                travelPoint.GetComponent<TravelPoint>().SetTravel(name, transform.position);
                awakeEffect.SetActive(true);
                TravelEvent?.Invoke(true);
                isAwake = true;
            }
            GameManager.instance.player.GetComponent<PlayerStatusHandler>().FullCondition();          
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            alert.SetActive(true);
            if (isAwake)
                TravelEvent?.Invoke(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            alert.SetActive(false);
            TravelEvent?.Invoke(false);
        }
    }
}
