using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ItemDrop 
{
    void DropItems();
}

public  class UnitController : MonoBehaviour, ItemDrop
{
    public string MyName;
    public void DropItems()
    {
        int dropitemcount = Managers.DataLoder.DataCache_Monsters[MyName].dropitems.Count;
        for (int i = 0; i < dropitemcount; i++)
        {
            int rand = Random.Range(0, 99);
            if (rand < Managers.DataLoder.DataCache_Monsters[MyName].dropitems[i].probability)
            {
                string name = Managers.DataLoder.DataCache_Monsters[MyName].dropitems[i].name;
                int amount = Managers.DataLoder.DataCache_Monsters[MyName].dropitems[i].amount;

                GameObject prefab = Resources.Load<GameObject>($"DropItem/{name}");
                if (prefab)
                {
                    GameObject ob = Instantiate(prefab);
                    ob.transform.position = transform.position + Vector3.up*3;
                    ob.GetComponent<PickupItem>().SetItem(name, amount);
                }
            }
        }
    }
}
