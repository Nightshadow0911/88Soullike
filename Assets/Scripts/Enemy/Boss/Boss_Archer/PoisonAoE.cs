using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonAoE : MonoBehaviour
{
    [SerializeField] private LayerMask target;
    [SerializeField] private float duration;
    private float currentTime;
    private BoxCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"), true);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > duration)
            Destroy(gameObject);
        
        Collider2D collision = Physics2D.OverlapBox(
            transform.position, coll.size, 0, target);
        if (collision != null)
            Debug.Log("poison hit!");
    }
}
