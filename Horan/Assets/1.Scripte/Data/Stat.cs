using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageInteraction
{
    public void TakeDamage(float Damage);
}

public abstract class Stat : MonoBehaviour
{
    public Action<int,int> OnStatChanged; //스텟 식별자, 변화량 백분률
}