public class SequenceNode : Node
{
    public SequenceNode(string nodeName,int priority = 0) : base(priority, nodeName)
    {
    }

    public override NodeStatus Evaluate()
    {
        if(childIndex >= children.Count)
            ResetChildIndex();

        for (int i = 0; i < children.Count; i++)
        {
            switch (children[childIndex].Evaluate())
            {
                case NodeStatus.FAILURE:
                    ResetChildIndex();
                    return NodeStatus.FAILURE;
                case NodeStatus.SUCCESS:
                    childIndex++;
                    return childIndex >= children.Count ? NodeStatus.SUCCESS : NodeStatus.RUNNING;
                case NodeStatus.RUNNING:
                    return NodeStatus.RUNNING;
            }
        }
        return NodeStatus.DEFAULT;
    }
}



