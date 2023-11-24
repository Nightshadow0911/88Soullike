using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class Patrol : Node
{
    private BossAI bossAI;
    
    private Rigidbody2D rigid;
    private Transform boss;
    private Transform target;
    private float speed = 5f;

    public Patrol(BossAI ai, Transform boss, Transform target, Rigidbody2D rigid)
    {
        this.bossAI = ai;
        this.boss = boss;
        this.target = target;
        this.rigid = rigid;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("aa");
        float distance = target.position.x - boss.position.x;
        rigid.velocity = (Vector2.right * distance).normalized * speed;
        if (Mathf.Abs(distance) < 1f)
        {
            state = NodeState.FAILURE;
            return state;
        }
        bossAI.sprite.color = Color.green;
        state = NodeState.RUNNING;
        return state;
    }
}
