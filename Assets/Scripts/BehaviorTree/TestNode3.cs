using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree
{
    public class TestNode3 : Node
    {
        private int index;
        private BossAI bossAi;
        public TestNode3(int num, BossAI ai)
        {
            index = num;
            bossAi = ai;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("dd");
            bossAi.sprite.color = Color.blue;
            return NodeState.SUCCESS;
        }
    }
}

