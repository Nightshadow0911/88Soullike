using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree
{
    public class RandomSelector : Node
    {
        private List<Node> childrenNodes = new List<Node>();
        private Stack<Node> randomChildren = new Stack<Node>();
        
        public RandomSelector(List<Node> children)
        {
            foreach (Node child in children)
            {
                childrenNodes.Add(child);
            }
        }
        
        public override NodeState Evaluate()
        {
            if (randomChildren.Count == 0)
                ShuffleChildren();

            while (randomChildren.Count > 0)
            {
                switch (randomChildren.Peek().Evaluate())
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
                        state = NodeState.FAILURE;
                        return state;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }

        public void ShuffleChildren()
        {
            randomChildren.Clear();
            for (int i = 0; i < childrenNodes.Count; i++)
            {
                int ran = Random.Range(0, childrenNodes.Count);
                Node randomNode = childrenNodes[ran];
                randomChildren.Push(randomNode);
                childrenNodes[ran] = childrenNodes[i];
                childrenNodes[i] = randomNode;
            }
        }
    }
}