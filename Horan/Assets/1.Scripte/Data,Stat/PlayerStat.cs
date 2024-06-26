using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat : Stat, IDamageInteraction
{
    public bool isDamageable=false;
    public bool isRegenable=false;

    float CurHpRegenTime;
    float HpRegenTime = 5;
    public float HpRegenAmount = 0.5f;

    float CurSpRegenTime;
    float SpRegenTime = 5;
    public float SpRegenAmount = 5;

    [SerializeField]
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
    [SerializeField]
    float _hp;
    public float Hp { get { return _hp; }
        set
        {
            float pre = _hp;
            _hp = value;
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.Hp, pre, _hp);
            if (OnUnitDead!=null && _hp<=0)
                OnUnitDead.Invoke();
        }
    }
    [SerializeField]
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
    [SerializeField]
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
    [SerializeField]
    float _attack;
    public float Attack { get { return _attack; } set { _attack = value; } }
    [SerializeField]
    float _speed;
    public float Speed { get { return _speed; } set { _speed = value; } }
    [SerializeField]
    float _critical;
    public float Critical { get { return _critical; } set { _critical = value; } }
    [SerializeField]
    int _level;
    public int Level
    {
        get { return _level; }
        set
        {
            int pre = _level;
            _level = value;
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.Level, pre, _level);
        }
    }
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

            while (TotalExp<=_exp)
            {
                Level += 1;
                _exp -= TotalExp;
            }
            if (OnStatChanged != null)
                OnStatChanged.Invoke(StatIdentifier.Exp, pre, _exp);
        }
    }

    public enum ECharacterAtkType {FAtk,SAtk,DashAtk,CounterAtk}
    public ECharacterAtkType atkType;

    void Start() 
    {
        isRegenable = true;
        isDamageable = true;
    }
    public void StatInit(int level,float exp , float hp)
    {
        _level = level;
        //맵 이동시 전달이 필요 없는 캐릭터 기본 값
        TotalExp = 100;
        MaxHp = 100;
        MaxSp = 100;
        Attack = 20;
        Critical = 0.5f;
        Speed = 6.5f;
        HpRegenTime = 5;
        HpRegenAmount = 5f;
        SpRegenTime = 5;
        SpRegenAmount = 5;

        Sp = MaxSp;

        //맵 이동시 전달 되어야 하는 
        if (exp != -1) Exp = exp;
        else Exp = 0;
        if (hp != -1) Hp = hp;
        else Hp = MaxHp;
    }
    public bool TakeDamage(float damage)
    {
        if (0 < Hp)
        {
            if (OnHit != null)
                OnHit.Invoke();
            if (isDamageable)
            {
                if (OnUnitTakeDamaged != null)
                    OnUnitTakeDamaged.Invoke();
                Hp = Mathf.Clamp(Hp - damage, -1, MaxHp);
                return true;
            }
        }
        return false;
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
            CurHpRegenTime += Time.deltaTime; //Regen_HP
            if (HpRegenTime <= CurHpRegenTime)
            {
                CurHpRegenTime = 0;
                Hp = Mathf.Clamp(Hp + HpRegenAmount, Hp, MaxHp);
            }
            CurSpRegenTime += Time.deltaTime; //Regen_SP
            if (SpRegenTime <= CurSpRegenTime) 
            {
                CurSpRegenTime = 0;
                Sp = Mathf.Clamp(Sp + SpRegenAmount, Sp, MaxSp);
            }
        }
    }
}
