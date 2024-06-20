using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripre : MonoBehaviour
{
    /*모바일 입력에 따라 바인딩 하기 

캐릭터 공격 시 각도 보정 해주기 

약공격 강공격 입력 구분해서 공격하기 

약,강 공격 데미지 공식 분리

대시공격 변경*/

    void OnSingleTouch()
    {
        if (Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began: //터치 시작 
                    break;
                case TouchPhase.Moved: //터치 + 이동 중  (매 프레임)
                    break;
                case TouchPhase.Stationary: //터치 제자리 유지 (매 프레임)
                    break;
                case TouchPhase.Ended: //터치 종료 
                    break;
                case TouchPhase.Canceled: // 터치 종료 (강제)
                    break;
            }

        }
    }
}
