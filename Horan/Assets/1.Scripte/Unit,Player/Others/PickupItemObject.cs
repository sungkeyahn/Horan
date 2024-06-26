using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemObject : MonoBehaviour
{
    public int type=-1;
    float val=20;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            PlayerStat stat = other.GetComponent<PlayerStat>();

            switch (type)
            {
                case 0:
                    Managers.ContentsManager.dropgold += (int)val;
                    break;
                case 1:
                    stat.Exp += val;
                    break;
                default:
                    stat.Hp += val;
                    break;
            }
            Destroy(gameObject);
        }        
    }
}
