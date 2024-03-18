using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //데이터 테이블 참조 예정
    public int damage;
    public string[] AttackNames;
    public float[] AtkDelayTime;

    GameObject Owner;
    public BoxCollider Area { get; private set; }



    private void Start()
    {
        //무기 데이터도 테이블에서 불러오는 방식으로 변경하기 
        damage = 10;
        Area = GetComponent<BoxCollider>();
        Owner = GetComponentInParent<Stat>().gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitob = other.gameObject;
        if (hitob)
        {
            IDamageInteraction damageable= hitob.GetComponent<IDamageInteraction>();
            if(damageable!=null)
                damageable.TakeDamage(damage);
        }
    }

}
