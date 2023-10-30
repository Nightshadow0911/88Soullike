using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRangedHit : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector2 direction;
    private float speed;
    private float duration;
    private float currenTime;
    private float spread;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        currenTime += Time.deltaTime;
        if (currenTime > duration)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rigid.velocity = direction * speed;
    }

    private void InitializeAttack(Vector2 direction, float speed, float duration)
    {
        this.direction = direction;
        this.speed = speed;
        this.duration = duration;
    }
}
