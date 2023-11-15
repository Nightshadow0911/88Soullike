using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Boss : MonoBehaviour
{
    protected enum Distance
    {
        CloseRange,
        MediumRange,
        LongRange
    }
    
    protected Rigidbody2D rigid;
    [SerializeField] protected Transform target;
    [SerializeField] protected LayerMask targetLayer;
    protected SpriteRenderer sprite;
    
    [Header("Boss Stats")]
    [SerializeField] protected int power;
    [SerializeField] protected float speed;
    [SerializeField] protected float delay;
    [SerializeField] protected float baseGroggyMeter;
    [SerializeField] protected float groggyMeter;
    [SerializeField] protected float groggyTime;
    // 보스 속성 저항

    protected float currentTime = 0f;
    protected float lastAttackTime = float.MaxValue;
    protected bool isAttackReady = true;
    protected bool isGroggy = false;
    protected bool isDie = false;
    [SerializeField] protected GameObject dangerSign;

    protected Distance targetDistance;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    
    protected virtual void Update()
    {
        if (lastAttackTime < delay && isAttackReady)
            lastAttackTime += Time.deltaTime;
        else if (lastAttackTime > delay && isAttackReady)
        {
            StopAllCoroutines();
            isAttackReady = false;
            lastAttackTime = 0f;
            SetDistance(Vector2.Distance(transform.position, target.position));
            BossPattern(targetDistance);
        }
    }

    private void SetDistance(float distance)
    {
        distance = Mathf.Abs(distance);
        if (distance < 2f)
        {
            targetDistance = Distance.CloseRange;
        }
        else if (distance < 6f) 
        {
            targetDistance = Distance.MediumRange;
        }
        else
        {
            targetDistance = Distance.LongRange;
        }
    }

    protected abstract void BossPattern(Distance distance);
}
