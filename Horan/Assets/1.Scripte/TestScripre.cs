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
                case TouchPhase.Began: //��ġ ���� 
                    break;
                case TouchPhase.Moved: //��ġ + �̵� ��  (�� ������)
                    break;
                case TouchPhase.Stationary: //��ġ ���ڸ� ���� (�� ������)
                    break;
                case TouchPhase.Ended: //��ġ ���� 
                    break;
                case TouchPhase.Canceled: // ��ġ ���� (����)
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
