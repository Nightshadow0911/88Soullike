using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree
{
    public class TestNode1 : Node
    {
        private int index;
        private BossAI bossAi;
        private float currentTime = 0f;
        public TestNode1(int num, BossAI ai)
        {
            index = num;
            bossAi = ai;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("bb");
            bossAi.sprite.color = Color.red;
            currentTime += Time.deltaTime;
            if (currentTime > 1f)
            {
                currentTime = 0f;
                return NodeState.FAILURE;
            }
            return NodeState.RUNNING;
        }
    }
}

