using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField]
    string itemname;

    [SerializeField]
    int amount;

    public void SetItem(string Itemname, int Amount) //���Ϳ��� ������ ������Ʈ�� ���������� ȣ���� �Լ�
    {
        itemname = Itemname;
        amount = Amount;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //�÷��̾ �ƴ϶� contentManager�� �� ����??
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc.Loot(itemname, amount))
            {
                Destroy(gameObject);
            }
        }
    }

}
