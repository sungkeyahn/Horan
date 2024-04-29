using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat : Stat, IDataBind, IDamageInteraction
{
    const float SpRegenTime = 5;
    float CurSpRegenTime;

    public Action OnHit;

    public bool isDamageable;
    public bool isRegenable;

    int _level;
    public int Level { get { return _level; }
        set
        {
            int pre = _level;
            _level = value;
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.Level, pre, _level);
        }
    }

    float _maxhp;
    public float MaxHp { get { return _maxhp; }
        set
        {
            float pre = _maxhp;
            _maxhp = value;
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.MaxHp, pre, _maxhp);
        }
    }
    float _hp;
    public float Hp { get { return _hp; }
        set
        {
            float pre = _hp;
            _hp = value;
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.Hp, pre, _hp);
        }
    }

    float _maxsp;
    public float MaxSp { get { return _maxsp; }
        set
        {
            float pre = _maxsp;
            _maxsp = value;
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.MaxSp, pre, _maxsp);
        }
    }
    float _sp;
    public float Sp { get { return _sp; }
        set
        {
            float pre = _sp;
            _sp = value;
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.Sp, pre, _sp);
        }
    }

    float _attack;
    public float Attack { get { return _attack; } set { _attack = value; } }
    float _speed;
    public float Speed { get { return _speed; } set { _speed = value; } }

    float _critical;
    public float Critical { get { return _critical; } set { _critical = value; } }

    float _totalexp;
    public float TotalExp { get { return _totalexp; }
        set
        {
            float pre = _totalexp;
            _totalexp = value;
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.TotalExp, pre, _totalexp);
        }
    }
    float _exp;
    public float Exp { get { return _exp; }
        set
        {
            float pre = _exp;
            _exp = value;
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.Exp, pre, _exp);
        }
    }



    
    private void Awake()
    {
        //아래 2줄 코드는 세이브 데이터 가 정의되면 데이터 로드 받아서 넣을 예정
        Level = 1;
        Exp = 0; 

        BindData();
    }
    void Start()
    {
        Hp = MaxHp;
        Sp = MaxSp;

        Critical = 0.5f;
        Speed = 5.5f;

        isRegenable = true;
        isDamageable = true;
    }
    public void BindData()
    {
        MaxHp = Managers.DataLoder.playerStatDict[Level].maxHp;
        MaxSp = Managers.DataLoder.playerStatDict[Level].maxSp;
        Attack = Managers.DataLoder.playerStatDict[Level].attack;
        TotalExp = Managers.DataLoder.playerStatDict[Level].totalExp;
    }

    public void TakeDamage(float damage)
    {
        if (0 < Hp)
        {
            if (OnHit != null)
                OnHit.Invoke();
            if (isDamageable)
            {
                Hp = Mathf.Clamp(Hp - damage, -1, MaxHp);
              
            }
        }
    }

    public bool UseSP(float usedSP)
    {
        if (usedSP < Sp)
        {
            Sp = Mathf.Clamp(Sp - usedSP, 0, MaxSp);
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.Sp, Sp ,MaxSp);
            return true;
        }
        return false;
    }

    void Update()
    {
        if (isRegenable)
        {
            CurSpRegenTime += Time.deltaTime;

            if (SpRegenTime <= CurSpRegenTime) //Regen_SP
            {
                CurSpRegenTime = 0;
                Sp = Mathf.Clamp(Sp + 5, Sp, MaxSp);
            }
        }
    }
}
