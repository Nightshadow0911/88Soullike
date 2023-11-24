using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask whatIsLedge;
    [SerializeField] private LastPlayerController player;

    private bool canDeteced;

    private void Update()
    {
        if (canDeteced)
        {
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius,whatIsLedge);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            canDeteced = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            canDeteced = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
