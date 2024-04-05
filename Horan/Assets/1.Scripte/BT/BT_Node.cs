using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BT_Node
{
    public enum NodeState 
    {
        Success, Failure, Running
    }
    public NodeState State {  get; protected set; }
    public abstract NodeState Evaluate();
}
