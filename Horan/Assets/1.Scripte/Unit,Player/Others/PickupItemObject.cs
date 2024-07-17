using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemObject : MonoBehaviour
{
    public int type=-1;
    float val=20;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            PlayerStat stat = other.GetComponent<PlayerStat>();
            int rand = UnityEngine.Random.Range(1, 4);
            Debug.Log(rand);
            Instantiate(Managers.DataLoder.DataCache_Sound[$"Sound_item_{rand}"]).GetComponent<AudioSource>().Play();
            switch (type)
            {
                case 0:
                    Instantiate(Managers.DataLoder.DataCache_Effect["money_up_effect"]).transform.position = transform.position; ;
                    Managers.ContentsManager.dropgold += (int)val;
                    break;
                case 1:
                    Instantiate(Managers.DataLoder.DataCache_Effect["item_up_effect"]).transform.position = transform.position; ;
                    stat.Exp += val;
                    break;
                default:
                    Instantiate(Managers.DataLoder.DataCache_Effect["hp_up_effect"]).transform.position=transform.position;
                    stat.Hp += val;
                    break;
            }
            Destroy(gameObject);
        }        
    }
}
