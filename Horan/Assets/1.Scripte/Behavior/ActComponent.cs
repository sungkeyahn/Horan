using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActComponent : MonoBehaviour
{
    /*
     * 
     * 
     * 행위객체  + 행위컴포넌트 
     * 컴포넌트는 행동가능한 행위들을 가지
     * 이제 해당 컴포넌트를 가진 오브젝트들이 어떤 행위를 가지게 하는 방식 
     * 
     * 
     */
    GameObject Actor; // 행위자
    bool isRunning; // 행위 실행 여부 
    int[] ActionableActs; // 행위자가 실행 가능한 모든 행위들의 모음
    //위 변수에 들어갈 데이터는 따로 데이터테이블로 만들지 말고 유닛 객체의 데이터 테이블에 속성으로 추가해 주는 방향으로 

    Act CurAct; // 실행 중인 행위 
    int[] CurActionableAct; // 현재 행위를 덮어씌울수 잇는 행위들 
    //int[] CurinfeasibleAct; // 현재 실행 불가능한 행위 

    public bool Execution()
    {
        bool canExecution = CanStart();
        if (canExecution)
        {
            
        }

        return false;
    }
    public bool Finish()
    {
        return false;
    }
    public bool CanStart()
    {
        return false;
    }

}
