using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField]
    string itemname;

    [SerializeField]
    int amount;

    public void SetItem(string Itemname, int Amount) //몬스터에서 아이템 오브젝트를 생성했을때 호출할 함수
    {
        itemname = Itemname;
        amount = Amount;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //플레이어가 아니라 contentManager에 들어갈 내용??
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc.Loot(itemname, amount))
            {
                Destroy(gameObject);
            }
        }
    }

}
