using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTRunner
{
    BT_Node Root;
    public Action OnService = null;
    public bool isActive;
    public BTRunner(BT_Node root)
    {
        Root = root;
        isActive = true;
    }
    public void Operate()
    {
        if (isActive)
            Root.Evaluate();
    }
    public void ServiceOperate()
    {
            if (OnService != null&& isActive)
                OnService.Invoke();
    }
}
