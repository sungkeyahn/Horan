using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Decorator : BT_Node
{
    protected BT_Node Child;

    public Func<bool> OnDecorate = null;

    public BT_Decorator(BT_Node child, Func<bool> onDecorate)
    {
        if (child != null)
        {
            Child = child;
            OnDecorate = onDecorate;
        }
    }

    public override NodeState Evaluate()
    {
        if (Child == null)
        {
            State = NodeState.Failure;
            return NodeState.Failure;
        }

        bool Condition = true;
        if (OnDecorate != null)
            Condition = OnDecorate.Invoke();

        if (Condition)
            State = Child.Evaluate();
        else
            State = NodeState.Failure;


        return State;     
    }
}
