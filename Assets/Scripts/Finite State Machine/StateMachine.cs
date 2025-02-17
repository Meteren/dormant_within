using System;
using System.Collections.Generic;

public class StateMachine 
{
    private Dictionary<Type, StateNode> stateNodes = new();
    private IState currentState;
    public IState CurrentState { get { return currentState; } set { currentState = value; } }

    public void Update()
    {
        if(TryGetTransition(out Transition transition))
        {
            ChangeStateTo(transition.to);
        }

        currentState.Update();

    }

    private void ChangeStateTo(IState to)
    {
        currentState.OnExit();
        currentState = to;
        currentState.OnStart();
    }

    private bool TryGetTransition(out Transition transition)
    {
        foreach(var transit in stateNodes[currentState.GetType()].transitions)
        {
            if(transit != null)
            {
                if (transit.condition.Evaluate())
                {
                    transition = transit;
                    return true;
                }
            }
        }

        transition = default;
        return false;
    }
    
    public void AddNode(IState from, IState to, IPredicate condition)
    {
        AddOrGetStateNode(from).AddTransition(AddOrGetStateNode(to).nodeState, condition);
    }

    private StateNode AddOrGetStateNode(IState state)
    {
        if (stateNodes.TryGetValue(state.GetType(),out StateNode node))
        {
            return node;
        }
        
        StateNode stateNode = new StateNode(state);
        stateNodes[state.GetType()] = stateNode;
        return stateNode; 
        
    }

    public class StateNode
    {
        public List<Transition> transitions = new List<Transition>();
        public IState nodeState;

        public StateNode(IState nodeState)
        {
            this.nodeState = nodeState;
        }

        public void AddTransition(IState to, IPredicate predicate)
        {
            transitions.Add(new Transition(predicate, to));
        }

    }
}
