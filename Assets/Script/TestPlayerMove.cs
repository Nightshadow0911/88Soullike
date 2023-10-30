using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : MonoBehaviour
{
    private Rigidbody2D rigid;
    public float speed;
    private float axisX, axisY;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        axisX = Input.GetAxisRaw("Horizontal");
        axisY = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(axisX, axisY);
        rigid.velocity = direction * speed;
    }
}
