using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageInteraction
{
    public void TakeDamage(float Damage);
}

public enum StatIdentifier
{ Level = 1, MaxHp, Hp, MaxSp, Sp, TotalExp, Exp }

public abstract class Stat : MonoBehaviour
{
    public Action<StatIdentifier, float, float> OnStatChanged; //스텟 식별자, 변화량 백분률
    public Action OnUnitDead;
    public Action OnUnitTakeDamaged;
}