using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageInteraction
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    const float force=18f;
    [SerializeField]
    Vector3 offset = Vector3.zero;
    [SerializeField]
    int hp = 1;

    public void Break()
    {
        GameObject clone = Instantiate(prefab,transform.position,Quaternion.identity);
        Rigidbody[] rigid = clone.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rigid.Length;  i++)
        {
            rigid[i].AddExplosionForce(force, transform.position + offset, 10f);
        }
        gameObject.SetActive(false);
    }
    public bool TakeDamage(float Damage)
    {
        hp -= 1;
        if (hp <= 0)
        {
            Break();
            SpawnDropObject();
        }
        return true;
    }
    void SpawnDropObject() 
    {   
       int num = UnityEngine.Random.Range(0,3);
        GameObject drop;
        switch (num)
        {
            case 0:
                drop=Instantiate(Resources.Load("Object/DropObjects/Exp"), transform.position, Quaternion.identity) as GameObject;
                break;
            case 1:
                drop= Instantiate(Resources.Load("Object/DropObjects/Gold"), transform.position, Quaternion.identity) as GameObject;
                break;
            default:
                drop= Instantiate(Resources.Load("Object/DropObjects/Heal"), transform.position, Quaternion.identity) as GameObject;
                break;
        }
        drop.GetComponent<PickupItemObject>().type = num;
    }
}
