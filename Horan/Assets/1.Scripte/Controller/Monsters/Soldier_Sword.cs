using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier_Sword : MonsterController
{
    //플레이어 거리 탐지  float distance = (player.transform.position - transform.position).magnitude;

    float WalkSpeed = 4.0f;
    float RunSpeed = 6.0f;
    float ATKRange = 4.0f;
    float SensRange = 15.0f;
    float WanderInterval = 3.0f;
    float WanderTime = 0;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        Stat = GetComponent<MonsterStat>();
        Stat.isRegenable = true;
        Stat.isDamageable = true;

        /*SenseRange = 8;
        AttackRange = 2;
        PlayerSerchInterval = 3;
        HPRatioCheckInterval = 1;
        WanderingInterval = 5;
        DangersHPRatio = 50;
        CurrentHPRatio = 100;*/


    }
}
