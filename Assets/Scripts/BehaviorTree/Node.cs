using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }
    
    public abstract class Node
    {
        protected NodeState state;

        public abstract NodeState Evaluate();
    }
}
    
