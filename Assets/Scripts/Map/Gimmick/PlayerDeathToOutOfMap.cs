using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathToOutOfMap : MonoBehaviour
{
    Vector3 boxSize = new Vector3(350f, 1f, 0f);

    private void Update()
    {
        death();
    }
    private void OnTriggerEnter(Collider other)
    {
        death();
    }

    private void death()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerStatusHandler>().currentHp = 0;
            }
        }
    }
    
}
