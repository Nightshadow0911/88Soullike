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
    
