using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripre : MonoBehaviour
{
    /*����� �Է¿� ���� ���ε� �ϱ� 

ĳ���� ���� �� ���� ���� ���ֱ� 

����� ������ �Է� �����ؼ� �����ϱ� 

��,�� ���� ������ ���� �и�

��ð��� ����*/

    void OnSingleTouch()
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
    }
}
