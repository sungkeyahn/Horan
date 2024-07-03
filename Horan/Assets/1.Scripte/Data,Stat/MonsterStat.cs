using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat, IDataBind, IDamageInteraction
{
    const float spregenTime = 5;
    public bool isRegenable;
    public bool isDamageable;
    float CurregenTime;

    public float maxhp;
    public float maxsp;
    public float damage;
    public float walkspeed;
    public float runspeed;
    public float atkrange;
    public float sensingrange;

    [SerializeField]
    public float hp;
    [SerializeField]
    public float sp;

    void Start()
    {
        BindData();
        hp = maxhp;
        sp = maxsp;

        isRegenable = true;
        isDamageable = true;
    }

    public void BindData()
    {
        MonsterController ctrl=GetComponent<MonsterController>();
        if (ctrl == null) return;

        maxhp = Managers.DataLoder.DataCache_Monsters[ctrl.MyName].maxHp;
        maxsp = Managers.DataLoder.DataCache_Monsters[ctrl.MyName].maxSp;
        damage = Managers.DataLoder.DataCache_Monsters[ctrl.MyName].damage;
        walkspeed = Managers.DataLoder.DataCache_Monsters[ctrl.MyName].walkspeed;
        runspeed = Managers.DataLoder.DataCache_Monsters[ctrl.MyName].runspeed;
        atkrange = Managers.DataLoder.DataCache_Monsters[ctrl.MyName].atkrange;
        sensingrange = Managers.DataLoder.DataCache_Monsters[ctrl.MyName].sensingrange;
    }

    public bool TakeDamage(float damage)
    {
        if (0 < hp)
        {
            if (OnHit != null)
                OnHit.Invoke();

            if (isDamageable)
            {
                float pre = hp;
                hp = Mathf.Clamp(hp - damage, -1, maxhp);

                if (OnUnitTakeDamaged != null && pre > hp)
                    OnUnitTakeDamaged.Invoke();

                if (hp <= 0)
                    OnUnitDead.Invoke();
                
                if (OnStatChanged != null)
                    OnStatChanged.Invoke(StatIdentifier.Hp, hp , maxhp);

                return true;
            }
        }
        return false;
    }

    public bool UseSP(float usedSP)
    {
        if (usedSP < sp)
        {
            sp = Mathf.Clamp(sp - usedSP, 0, maxsp);
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.Sp, sp, maxsp);
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
