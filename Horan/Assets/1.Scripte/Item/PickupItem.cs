using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //�ش� ��ũ��Ʈ �� ������ ������Ʈ�� �ƹ��������� �� ���� �������� �κи� ����ϵ��� 


    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            {
                Destroy(gameObject);
            }
        }
    }

}
