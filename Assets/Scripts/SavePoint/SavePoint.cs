using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] private GameObject alert;
    [SerializeField] private GameObject awakeEffect;
    [SerializeField] private Travel travel;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip soundData;

    private Animator anim;
    private bool isAwake = false;
    
    private void Awake()
    {
        alert.SetActive(false);
        awakeEffect.SetActive(false);
        travel.position = transform.position;
        anim = awakeEffect.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && alert.activeSelf)
        {
            if (!isAwake)
            {
                awakeEffect.SetActive(true);
                anim.SetTrigger("Save");
                SoundManager.instance.PlayClip(soundData);
                FTravelUI.instance.SetTravel(travel);
                FTravelUI.instance.OnSavePoint?.Invoke(true);
                isAwake = true;
            }
            anim.SetTrigger("Save");
            SoundManager.instance.PlayClip(soundData);
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
            FTravelUI.instance.OnSavePoint?.Invoke(isAwake);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            alert.SetActive(false);
            FTravelUI.instance.OnSavePoint?.Invoke(false);
        }
    }
}
