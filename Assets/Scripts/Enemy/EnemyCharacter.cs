using System;
using System.Collections;
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
    [SerializeField] private EnemyStat baseStats; protected EnemyStat GetBaseStats() => baseStats;
    protected EnemyStat currentStats;
    [SerializeField] protected Transform targetTransform;
    protected Rigidbody2D rigid;
    protected EnemyAnimationController animationController;
    protected SoundManager soundManager;

    private Coroutine currentPattern;
    private float currentTime = float.MaxValue;
    protected State state;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animationController = GetComponent<EnemyAnimationController>();
        SetStats();
    }

    protected virtual void Start()
    {
        soundManager = SoundManager.instance;
    }

    protected virtual void Update()
    {
        if (state == State.FAILURE)
            ActionPattern();
        if (state != State.RUNNING)
            currentTime += Time.deltaTime;
        if (currentTime > currentStats.delay)
        {
            ActionPattern();
        }
    }
    
    private void ActionPattern()
    {
        currentTime = 0f;
        if (currentPattern != null)
            StopCoroutine(currentPattern);
        SetDistance(targetTransform.position);
        currentPattern = StartCoroutine(GetPattern()());
    }

    protected virtual void RunningPattern()
    {
        state = State.RUNNING;
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
        currentStats = ScriptableObject.CreateInstance<EnemyStat>();
        currentStats.characterMaxHP = baseStats.characterMaxHP;
        currentStats.characterDamage = baseStats.characterDamage;
        currentStats.characterDefense = baseStats.characterDefense;
        currentStats.propertyDamage = baseStats.propertyDamage;
        currentStats.propertyDefense = baseStats.propertyDefense;
        currentStats.speed = baseStats.speed;
        currentStats.delay = baseStats.delay;
        currentStats.target = baseStats.target;
    }

    private void AllStop()
    {
        StopAllCoroutines();
        state = State.FAILURE;
        rigid.velocity = Vector2.zero;
    }
}
