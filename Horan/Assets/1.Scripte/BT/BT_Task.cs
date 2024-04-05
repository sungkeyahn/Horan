using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Task : BT_Node
{
    public Func<NodeState> OnUpdate=null;
    public BT_Task(Func<NodeState> onUpdate)
    {
        OnUpdate = onUpdate;
    }
    public override NodeState Evaluate()
    {
        if (OnUpdate == null)
        {
            State = NodeState.Failure;
            return NodeState.Failure;
        }
        else
        {
            State = OnUpdate.Invoke();
            return State;
        }
    }
}
