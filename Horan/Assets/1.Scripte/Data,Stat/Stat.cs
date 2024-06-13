using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageInteraction
{
    public bool TakeDamage(float Damage);
}

public enum StatIdentifier
{ Level = 1, MaxHp, Hp, MaxSp, Sp, TotalExp, Exp }

public abstract class Stat : MonoBehaviour
{
    public Action<StatIdentifier, float, float> OnStatChanged; //���� �ĺ���, ��ȭ�� ��з�
    public Action OnUnitDead;
    public Action OnUnitTakeDamaged;
    public Action OnHit;
}