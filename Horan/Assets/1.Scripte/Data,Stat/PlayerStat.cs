using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatIdentifier_Player
{ level = 1, maxhp, hp, sp }
public class PlayerStat : Stat, IDataBind, IDamageInteraction
{
    [SerializeField]
    public float maxhp;
    [SerializeField]
    public float hp;

    [SerializeField]
    public float maxsp;
    [SerializeField]
    public float sp;

    [SerializeField]
    public float attack;
    [SerializeField]
    public float speed;

    [SerializeField]
    public float critical;

    public bool isRegenable;
    float CurregenTime;
    const float spregenTime = 5;


    public Action OnHit;
    public bool isDamageable;



    void Start()
    {
        BindData();
        hp = maxhp;

        maxsp = 100;
        sp = maxsp;

        critical = 0.5f;

        speed = 5.5f;


        isRegenable = true;
        isDamageable = true;
    }

    public void BindData()
    {
        maxhp = Managers.DataLoder.playerStatDict[(int)StatIdentifier_Player.level].maxHp;
        attack = Managers.DataLoder.playerStatDict[(int)StatIdentifier_Player.level].attack;
    }

    public void TakeDamage(float damage)
    {
        if (0 < hp)
        {
            if (OnHit != null)
                OnHit.Invoke();
            if (isDamageable)
            {
                hp = Mathf.Clamp(hp - damage, -1, maxhp);
                if (OnStatChanged != null)
                    OnStatChanged.Invoke((int)StatIdentifier_Player.hp, (int)(hp / maxhp * 100));
            }
        }
    }

    public bool UseSP(float usedSP)
    {
        if (usedSP < sp)
        {
            sp = Mathf.Clamp(sp - usedSP, 0, maxsp);
            if (OnStatChanged != null)
                OnStatChanged.Invoke((int)StatIdentifier_Player.sp, (int)(sp / maxsp * 100));
            return true;
        }
        return false;
    }

    void Update()
    {
        if (isRegenable)
        {
            CurregenTime += Time.deltaTime;

            if (spregenTime <= CurregenTime) //Regen_SP
            {
                CurregenTime = 0;
                sp = Mathf.Clamp(sp + 5, sp, maxsp);
            }
        }
    }
}
