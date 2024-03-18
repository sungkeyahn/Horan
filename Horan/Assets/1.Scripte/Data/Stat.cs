using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDamageInteraction
{
    public void TakeDamage(float Damage);
}
public enum StatIdentifier_Player
{ level=1, maxhp, hp, sp }

public abstract class Stat : MonoBehaviour
{
    //public Dictionary<int, float> Stats=new Dictionary<int, float>(); //스텟 컨테이너
    public Action<int,int> OnStatChanged; //스텟 식별자, 변화량 백분률
}