using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAoE : MonoBehaviour
{
    [SerializeField] private LayerMask target;
    [SerializeField] private float duration;
    private float currentTime;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > duration)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            Debug.Log("poison hit!");
    }
}
