using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterController : UnitController
{
    public Animator Anim { get; protected set; }

    public MonsterStat Stat { get; protected set; }

    public BTRunner Runner { get; protected set; }
    
    public NavMeshAgent Nav { get; protected set; }

    private void Update()
    {
        if (Runner != null)
        {
            Runner.Operate();
            Runner.ServiceOperate();
        }
    }

    protected bool IsAnimationRunning(string stateName)
    {
        if (Anim != null)
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            {
                var normalizedTime = Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

                return normalizedTime != 0 && normalizedTime < 1f;
            }
        }
        return false;
    }

    protected void StopUnit(bool isStop)
    {
        Runner.isActive = !isStop;
        Nav.isStopped = isStop;
    }

    /*
    Vector3 DestPos;
    DestPos = transform.position + new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
    Nav.SetDestination(DestPos);
    Nav.speed = RunSpeed;
    Nav.speed = WalkSpeed;
    Nav.speed = 0;
    Anim.Play("MOVE");
    Anim.Play("RUN");
    Anim.Play("WAIT");
    Anim.Play("ATTACK");
    
    bool isAttacking;
    IEnumerator ATTACK()
    {
        BoxCollider Area = GetComponentInChildren<Weapon>().gameObject.GetComponent<BoxCollider>();
        isAttacking = true;
        var curAnimStateInfo = Anim.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(0.2f); // 공격 활성화
        Area.enabled = true;
        yield return new WaitForSeconds(0.8f); // 공격 비활성화
        Area.enabled = false;

        yield return new WaitForSeconds(curAnimStateInfo.length - 1f);   // 애니메이션 시간 동안 대기
        isAttacking = false;
    }
     */




}
