using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    protected MonsterStat onwerStat;
    public BoxCollider Area { get; private set; } //공격 범위 콜라이더
    private void Awake()
    {
        Area = GetComponent<BoxCollider>();
        Area.enabled = false;
        onwerStat = GetComponentInParent<MonsterStat>();
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitob = other.gameObject;
        if (hitob)
        {
            IDamageInteraction damageable = hitob.GetComponent<IDamageInteraction>();
            if (damageable != null)
            {
                float finaldamage = onwerStat.damage;
                if (damageable.TakeDamage(finaldamage))
                {
                  
                    //Managers.PrefabManager.SpawnEffect("Hit_01", transform, transform.localPosition - Vector3.up);
                    //Managers.PrefabManager.SpawnEffect("Hit_01", other.bounds.ClosestPoint(Area.bounds.center));
                    //Managers.PrefabManager.SpawnEffect("Hit_01", other.bounds.ClosestPoint(transform.position));
                }
            }
        }
    }

}
