using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActive : MonoBehaviour
{
    public SpriteRenderer button;

    private void Start()
    {
        button.enabled = false;
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("11" + other);
        if(other.CompareTag("Player"))
            button.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("22" + other);
        if(other.CompareTag("Player"))
            button.enabled = false;
    }
}
