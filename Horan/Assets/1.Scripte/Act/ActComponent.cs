using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActComponent : MonoBehaviour
{
    Dictionary<int,Act> ActionableActs=new Dictionary<int, Act>(); // 행위자가 실행 가능한 행동들
    public Act CurAct { get; private set; } // 실행 중인 행위 정보
    
    public void AddAct(Act act)
    {
        if (act != null)
        {
            if (!ActionableActs.ContainsKey(act.ID))
                ActionableActs.Add(act.ID, act);
            else
                Debug.Log("This ACT is Aready Contain ActDict");
        }
    }
    public void RemoveAct(Act act)
    {
        if (act != null)
        {
            if (!ActionableActs.ContainsKey(act.ID))
                ActionableActs.Remove(act.ID);
            else
                Debug.Log("This ACT is Not Contain ActDict");
        }
    }

    public void Execution(int ActID)
    {
        if (CanStart(ActID))
        {
            CurAct.Execution();
        }
    }
    public void Finish(int ActID)
    {
        Act action = null;
        if (ActionableActs.TryGetValue(ActID, out action))
        {
            if (CurAct == action)
                CurAct.Finish();
        }
    }

    bool CanStart(int ActID)
    {
        Act action = null;
        if (ActionableActs.TryGetValue(ActID, out action)) // 실행할 행동을 가지고 있는가?
        {
            if (CurAct == null || !CurAct.isRunning)
            {
                CurAct = action;
                return true;

            }
            else if (CurAct.AllowActsID.Contains(action.ID)) //실행중인 행동이 새로들어온 행동을 허용하는가?
            {
                CurAct.Cancel();
                CurAct = action;
                return true;
            }
        }
        return false;
    }
}
