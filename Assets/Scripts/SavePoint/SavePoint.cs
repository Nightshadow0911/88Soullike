using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private GameObject alert;
    [SerializeField] private GameObject awakeEffect;
    [SerializeField] private LayerMask playerLayer;
    private bool isAwake = false;
    
    private void Awake()
    {
        alert.SetActive(false);
        awakeEffect.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && alert.activeSelf)
        {
            if (!isAwake)
            {
                awakeEffect.SetActive(true);
                isAwake = true;
            }
            GameObject player = GameManager.instance.player;
            player.GetComponent<PlayerStatusHandler>().FullCondition();          
            player.GetComponent<LastPlayerController>().SetPosition(transform.position);          
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            alert.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            alert.SetActive(false);
        }
    }
}
