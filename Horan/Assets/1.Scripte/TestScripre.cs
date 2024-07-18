using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripre : MonoBehaviour
{
    /*    void OnSingleTouch()
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
    }*/

    private void Start()
    {
        //GetComponent<DissolveEffect>().DissolveEffectForUpdate(0.25f);

    }
    bool aaa;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            aaa = !aaa;
           // GetComponent<RimLightEfffect>().RimLightEffect(aaa);
        }
    }

}
