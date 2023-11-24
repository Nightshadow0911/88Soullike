using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyCharacter : EnemyPattern
{
    [Header("Base Setting")]
    [SerializeField] protected EnemyStats baseStats;
    protected EnemyStats currentStats;
    [SerializeField] protected Transform targetTransform;
    protected Rigidbody2D rigid;
    protected SoundManager soundManager;
    private DangerSign danger;

    private IEnumerator pattern;
        
    private float currentTime = float.MaxValue;
    protected bool isGroggy;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        danger = GetComponent<DangerSign>();
        SetStats();
    }

    protected virtual void Start()
    {
        soundManager = SoundManager.instance;
    }

    protected virtual void Update()
    {
        if (state == PatternState.FAILURE)
            currentTime += currentStats.delay;
        if (state != PatternState.RUNNING)
            currentTime += Time.deltaTime;
        Debug.Log(state);
        Debug.Log((int)currentTime);
        if (currentTime > currentStats.delay) //2
        {
            ActionPattern();
        }
    }
    
    private void ActionPattern()
    {
        StopAllCoroutines();
        SetDistance(targetTransform.position);
        pattern = GetPattern();
        StartCoroutine(pattern);
        Debug.Log("패턴실행");
        Debug.Log("pattern");
    }

    // private void OnGroggy()
    // {
    //     AllStop();
    //     GroggyAnimation(isGroggy);
    //     currentStats.groggyTime += Time.deltaTime;
    //     
    //     if (currentStats.groggyTime > baseStats.groggyTime)
    //     {
    //         GroggyAnimation(isGroggy);
    //         currentStats.groggyMeter = baseStats.groggyMeter;
    //         currentTime = 0f;
    //     }
    // }
    protected void RunningPattern()
    {
        Debug.Log("런");
        state = PatternState.RUNNING;
        currentTime = 0f;
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
        danger.OffDangerSign();
        rigid.velocity = Vector2.zero;
    }
}
