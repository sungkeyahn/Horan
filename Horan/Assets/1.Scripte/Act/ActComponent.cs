using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActComponent : MonoBehaviour
{
    Dictionary<int,Act> ActionableActs=new Dictionary<int, Act>(); // �����ڰ� ���� ������ �ൿ��
    public Act CurAct { get; private set; } // ���� ���� ���� ����
    
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
        if (ActionableActs.TryGetValue(ActID, out action)) // ������ �ൿ�� ������ �ִ°�?
        {
            if (CurAct == null || !CurAct.isRunning)
            {
                CurAct = action;
                return true;

            }
            else if (CurAct.AllowActsID.Contains(action.ID)) //�������� �ൿ�� ���ε��� �ൿ�� ����ϴ°�?
            {
                CurAct.Cancel();
                CurAct = action;
                return true;
            }
        }
        return false;
    }
}
