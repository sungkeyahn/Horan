using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KindOfAct //데이터 테이블로 변경 하기 
{
    Move=1,Attack,Dash,Guard,DashAttack, Counter
}

public class Act
{
    public int ID { get; protected set; } //식별 번호
    public List<int> AllowActsID = new List<int>();
    public bool isRunning { get; protected set; } // 행위 실행 여부 
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

    public void Execution() //실행
    {
        //Debug.Log("ActExecution");
        action.Invoke();
        isRunning = true;
    }
    public void Cancel() //취소  //Cancel()에서 적절히 Finish()의 동작을 구현하거나 호출 하거나
    {
        //Debug.Log("ActCancel");
        isRunning = false;
    }
    public void Finish() //종료
    {
        //Debug.Log("ActFinish");
        isRunning = false;
    }
}
