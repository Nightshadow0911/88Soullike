using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class AI : MonoBehaviour
    {
        private Node root;

        protected virtual void Awake()
        {
            if (root == null)
                root = CreateTree();
        }
        
        protected virtual void Update()
        {
            root.Evaluate();
        }

        protected abstract Node CreateTree();
    }
}


