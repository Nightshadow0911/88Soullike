using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;



public class EnemyCharacter : EnemyPattern
{
    protected enum State
    {
        SUCCESS,
        RUNNING,
        FAILURE
    }
    
    [Header("Base Setting")]
    [SerializeField] protected EnemyStats baseStats;
    protected EnemyStats currentStats;
    [SerializeField] protected Transform targetTransform;
    protected Rigidbody2D rigid;
    protected SoundManager soundManager;

    private float currentTime = float.MaxValue;
    protected State state;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        SetStats();
    }

    protected virtual void Start()
    {
        soundManager = SoundManager.instance;
    }

    protected virtual void Update()
    {
        if (state == State.FAILURE)
            currentTime += currentStats.delay - .5f;
        if (state != State.RUNNING)
            currentTime += Time.deltaTime;
        Debug.Log(state);
        Debug.Log(currentStats.delay);
        Debug.Log((int)currentTime);
        if (currentTime > currentStats.delay)
        {
            ActionPattern();
        }
    }
    
    private void ActionPattern()
    {
        StopAllCoroutines();
        SetDistance(targetTransform.position);
        StartCoroutine(GetPattern()());
    }

    protected virtual void RunningPattern()
    {
        state = State.RUNNING;
        currentTime = 0f;
        Rotate();
    }
    
    protected void Rotate()
    {
        transform.rotation = targetTransform.position.x < transform.position.x
            ? Quaternion.Euler(0, 180, 0)
            : Quaternion.Euler(0, 0, 0);
    }
    
    private void SetStats()
    {
        currentStats = ScriptableObject.CreateInstance<EnemyStats>();
        currentStats.speed = baseStats.speed;
        currentStats.damage = baseStats.damage;
        currentStats.delay = baseStats.delay;
        currentStats.groggyMeter = baseStats.groggyMeter;
        currentStats.groggyTime = 0f;
        currentStats.target = baseStats.target;
    }
    
    private void AllStop()
    {
        StopAllCoroutines();
        state = State.FAILURE;
        rigid.velocity = Vector2.zero;
    }
}
