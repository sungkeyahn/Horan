using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Soldier_Sword : MonoBehaviour
{
    /*
    public  void Wandering(Vector3 DestPos)
    {
        if (isDead) return;
        if (IsAnimationRunning("ATTACK")) return;
        if (DestPos != Vector3.zero)
        {
            Vector3 pos = DestPos;
            if (Vector3.Distance(transform.position, pos) < Nav.stoppingDistance)
            {
                Anim.Play("WAIT");
                Nav.isStopped = true;
                Nav.velocity = Vector3.zero;
                return;
            }
            Anim.Play("MOVE");
            Nav.SetDestination(pos);
            Nav.isStopped = false;
            Nav.speed = Stat.walkspeed;
        }
    }
    
    public  void Chase(GameObject Target)
    {
        if (isDead) return;
        if (IsAnimationRunning("ATTACK")) return;
        if (IsAnimationRunning("RUN")) return;
        if (Vector3.Distance(transform.position, Target.transform.position) < Stat.atkrange) return;

        Anim.Play("RUN");
        Nav.SetDestination(Target.transform.position);
        Nav.isStopped = false;
        Nav.speed = Stat.runspeed;
    }
    
    public  void Attack(GameObject Target)
    {
        if (isDead) return;
        if (IsAnimationRunning("ATTACK")) return;
        if (!RotateToAtkTarget(Target)) return;

        //���⼭ ���� or ���� ���� �ϳ��� ������ ���� �ϵ��� �����ϱ� 
        Anim.Play("ATTACK");

        Weapon equippedWeapon = GetComponentInChildren<Weapon>();
        equippedWeapon.Area.enabled = true;
        equippedWeapon.Area.enabled = false;
    }

    public  void CombatWait(GameObject Target)
    {
        if (isDead) return;
        if (IsAnimationRunning("ATTACK")) return;

        if (Vector3.Distance(transform.position, Target.transform.position) < Stat.atkrange * 2)
        {
            Anim.Play("WAIT");
            Nav.isStopped = true;
        }
        else
        {
            Anim.Play("MOVE");
            Nav.isStopped = false;
            Nav.speed = Stat.walkspeed;
            Nav.SetDestination(Target.transform.position);
        }
    }

    void Guard()
    {
        if (Stat.sp <= 0) return;
        Anim.Play("GUARD");
        Stat.isDamageable = false;
    }
    */

}
