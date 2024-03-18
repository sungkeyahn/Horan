using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KindOfAct //데이터 테이블로 변경 하기 
{
    Attack=1,
}

public abstract class Act
{
    /*
     * 행동을 정의하는 객체 
     */

    protected int ID;
    protected List<int> AllowActs;
    //int[] BlockActs; //굳이 필요없다 


    public abstract void Init();
    public abstract void Execution();
    public abstract void Finish();

}
