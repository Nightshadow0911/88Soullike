using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    public int damage = 10;

    void Start()
    {

    }

    void Update()
    {
        transform.right = GetComponent<Rigidbody2D>().velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //데미지 공식 추가
            
        {
            Destroy(gameObject);
            Debug.Log("플레이어 활 맞음");
        }
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}