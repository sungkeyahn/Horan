using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //데이터 테이블 참조 예정
    public int damage;
    public string[] AttackAnimNames; // 해당 무기로 수행할 수 있는 모든 애니메이션 
    public float[] AtkDelayTimes; // 공격 애니메이션 이 실행될때 딜레이 시간(애니메이션 에 따라 달라짐으로 체크 필요) 
    public BoxCollider Area { get; private set; } //공격 범위 콜라이더


    private void Start()
    {
        //무기 데이터도 테이블에서 불러오는 방식으로 변경하기 
        damage = 10;
        Area = GetComponent<BoxCollider>();
        Area.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        print(string.Format("{0} 오브젝트가 {1} 와 충돌했습니다.", gameObject.name, other.gameObject.name));

        Stat onwerStat = GetComponentInParent<Stat>(); //구조 개선 고민
        int critical = UnityEngine.Random.Range(0, 100);
        GameObject hitob = other.gameObject;
        if (hitob)
        {
            IDamageInteraction damageable = hitob.GetComponent<IDamageInteraction>();
            if (damageable != null)
            {
                if (critical < 50)
                {
                    damageable.TakeDamage(damage * 1.5f);
                }
                else
                {
                    damageable.TakeDamage(damage);
                }

            }
        }
    }
}
