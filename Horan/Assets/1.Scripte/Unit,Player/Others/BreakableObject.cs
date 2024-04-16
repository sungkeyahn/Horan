using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : UnitController
{
    GameObject prefab;
    const float force=8f;
    Vector3 offset=Vector3.zero;

    public void Break()
    {
        GameObject clone = Instantiate(prefab,transform.position,Quaternion.identity);
        Rigidbody rigid = clone.GetComponentInChildren<Rigidbody>();
        //for (int i = 0; i < rigid.AddExplosionForce(force,transform.position+offset,10f); i++)
        //{

       // }
        gameObject.SetActive(false);
    }

}
