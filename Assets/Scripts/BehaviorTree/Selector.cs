using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree
{
    public class Selector : Node
    {
        private List<Node> childrenNodes = new List<Node>();

        public Selector(List<Node> children)
        {
            foreach (Node child in children)
            {
                childrenNodes.Add(child);
            }
        }

        public override NodeState Evaluate()
        {
            foreach (Node node in childrenNodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    case NodeState.FAILURE:
                        continue;
                    default:
                        continue;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}