using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    public GameObject target;
    private bool magnetTime;

    private void Update()
    {
        if(magnetTime) 
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.03f); // 점점 빨라지게도 되려나..
    }

    private void OnEnable()
    {
        target = null;
        magnetTime = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("MagnetField"))
        {
            target = collision.gameObject;
            magnetTime = true;
        }
        if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

    }
}
