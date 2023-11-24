using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        private List<Node> childrenNodes = new List<Node>();

        public Sequence(List<Node> children)
        {
            foreach (Node child in children)
            {
                childrenNodes.Add(child);
            }
        }
        
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in childrenNodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
     }
}