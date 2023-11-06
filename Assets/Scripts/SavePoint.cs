using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//[System.Serializable]

public class SavePoint : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private int travelNumber;
    private Rigidbody2D rigid;
    //private ui f 누르기 ~

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //ui .setActive true
        }   
    }
}
