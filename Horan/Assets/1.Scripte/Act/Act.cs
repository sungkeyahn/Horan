using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECharacterAct //������ ���̺�� ���� �ϱ� 
{
    Move=1,FAttack,SAttack,Dash,Guard,DashAttack, Counter
}

public class Act
{
    public int ID { get; protected set; } //�ĺ� ��ȣ
    public List<int> AllowActsID = new List<int>();
    public bool isRunning { get; protected set; } // ���� ���� ���� 
    public Action action;

    public Act(int id, Action actFun)
    {
        ID = id;
        action = actFun;
    }

    public void AddAllowActID(int id)
    {
        if (!AllowActsID.Contains(id))
            AllowActsID.Add(id);
    }
    public void RemoveAllowActID(int id)
    {
        if (AllowActsID.Contains(id))
            AllowActsID.Remove(id);
    }

    public void Execution() //����
    {
        //Debug.Log("ActExecution");
        action.Invoke();
        isRunning = true;
    }
    public void Cancel() //���  //Cancel()���� ������ Finish()�� ������ �����ϰų� ȣ�� �ϰų�
    {
        //Debug.Log("ActCancel");
        isRunning = false;
    }
    public void Finish() //����
    {
        //Debug.Log("ActFinish");
        isRunning = false;
    }
}
