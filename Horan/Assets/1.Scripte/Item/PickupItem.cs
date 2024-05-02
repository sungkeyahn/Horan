using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    //해당 스크립트 가 부착될 오브젝트는 아무런데이토 도 없고 연출적인 부분만 담당하도록 


    
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
