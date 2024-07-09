using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BreakableObject : MonoBehaviour, IDamageInteraction
{
    enum EBreakObType { Jar, Barrel,Box }
    [SerializeField]
    EBreakObType type;

    [SerializeField]
    GameObject prefab;
    [SerializeField]
    const float force=18f;
    [SerializeField]
    Vector3 offset = Vector3.zero;
    [SerializeField]
    int hp = 1;

    public bool TakeDamage(float Damage)
    {
        if (hp == 0) return false;

        hp -= 1;
        if (hp <= 0)
        {
            Break();
            SpawnDropObject();
        }
        return true;
    }
    void Break()
    {
        GameObject clone = Instantiate(prefab, transform.position, Quaternion.identity);
        if (clone)
        {
            Rigidbody[] rigid = clone.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rigid.Length; i++)
                rigid[i].AddExplosionForce(force, transform.position + offset, 10f);

            AudioSource Sound_Break = null;
            switch (type)
            {
                case EBreakObType.Jar:
                    Sound_Break = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_pot1"], clone.transform).GetComponent<AudioSource>();
                    break;
                case EBreakObType.Barrel:
                    Sound_Break = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_wood2"], clone.transform).GetComponent<AudioSource>();
                    break;
                case EBreakObType.Box:
                    Sound_Break = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_wood3"], clone.transform).GetComponent<AudioSource>();
                    break;
            }
            if (Sound_Break) Sound_Break.Play();
        }
        gameObject.SetActive(false);
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
