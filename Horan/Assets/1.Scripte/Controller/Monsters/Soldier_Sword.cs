using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Soldier_Sword : MonsterController
{
    //플레이어 거리 탐지  float distance = (player.transform.position - transform.position).magnitude;

    private void Start()
    {
        AttackType = AttackType.Melee;
        Anim = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        Stat = GetComponent<MonsterStat>();
        Stat.isRegenable = true;
        Stat.isDamageable = true;
    }
}
