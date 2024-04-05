using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Service : BT_Node
{
    protected BT_Node Child;

    public Action ServiceFun=null;

    MonsterGroup Group;
    
    float Interval;
    float time;

    public BT_Service(BT_Node child, MonsterGroup group, Action serviceFun, float interval=1)
    {
        if (child != null)
        {
            Child = child;
            Group = group;
            ServiceFun = serviceFun;
            Interval = interval;
            time = interval;
        }
    }
    public override NodeState Evaluate()
    {
        State = Child.Evaluate();

        if (Group && State == NodeState.Running)
        {
            Group.Runner.OnService -= OnService;
            Group.Runner.OnService += OnService;
        }

        return State;
    }
    void OnService()
    {
        State = Child.State;
        if (State != NodeState.Running)
        {
            Group.Runner.OnService -= OnService;
            return;
        }

        time += Time.deltaTime;
        if (Interval <= time)
        {
            if (ServiceFun != null)
                ServiceFun.Invoke();
            time = 0;
        }
    }
}


