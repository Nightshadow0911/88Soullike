using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField] private float speed = 5f;
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
        rigid.velocity = new Vector3(axisX, axisY, 0) * speed;
    }
}
