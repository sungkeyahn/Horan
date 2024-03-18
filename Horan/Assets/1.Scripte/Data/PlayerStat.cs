using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat : Stat, IDataBind, IDamageInteraction
{
    [SerializeField]
    public float maxhp;
    [SerializeField]
    public float hp;
    [SerializeField]
    public float sp;
    [SerializeField]
    public float attack;
    [SerializeField]
    public float speed;

    public Action OnHit;
    public bool isDamageable;

    void Start()
    {
        BindData();
        hp = maxhp;
        speed = 5.5f;
        isDamageable = true;
    }

    public void BindData()
    {
        maxhp = Managers.DataLoder.playerStatDict[(int)StatIdentifier_Player.level].maxHp;
        attack = Managers.DataLoder.playerStatDict[(int)StatIdentifier_Player.level].attack;
    }
    public void TakeDamage(float Damage)
    {
        if (0 < hp)
        {
            if (OnHit != null)
                OnHit.Invoke();
            if (isDamageable)
            {
                hp = Mathf.Clamp(hp - Damage, -1, maxhp);
                if (OnStatChanged != null)
                    OnStatChanged.Invoke((int)StatIdentifier_Player.hp, (int)(hp / maxhp * 100));
            }
        }
    }
}
