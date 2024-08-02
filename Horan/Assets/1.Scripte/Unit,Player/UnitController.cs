using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitController : MonoBehaviour
{
    public string MyName;
    protected AudioSource Sound_Hit;

    protected ActComponent Act;
    protected MoveComponent move; //-> Nav메시 컴포넌트를사용한 움직임 vs 리지드 바디를 사용한 움직임 구분하기 
    protected InputComponent input;
    //Input컴포넌트도 HUD에서 처리하는 방식 말고 해당 
}
