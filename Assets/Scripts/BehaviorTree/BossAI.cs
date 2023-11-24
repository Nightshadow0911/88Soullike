using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class BossAI : AI
{
    [SerializeField] private Transform target;
    private Rigidbody2D rigid;
    
    public SpriteRenderer sprite;

    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        base.Awake();
    }
    
    protected override Node CreateTree()
    {
        Node root = new Selector(new List<Node>
            {
                new Patrol(this, transform, target, rigid),
                new TestNode1(1, this),
                new TestNode2(2, this), 
                new TestNode3(3, this)
            }
        );
        return root;
    }
}
