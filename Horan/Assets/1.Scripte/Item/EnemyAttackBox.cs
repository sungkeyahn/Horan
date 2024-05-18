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
        Debug.Log(other.gameObject);
        if (hitob)
        {
            IDamageInteraction damageable = hitob.GetComponent<IDamageInteraction>();
            if (damageable != null)
            {
                float finaldamage = onwerStat.damage;
                damageable.TakeDamage(finaldamage);
            }
        }
    }

}
