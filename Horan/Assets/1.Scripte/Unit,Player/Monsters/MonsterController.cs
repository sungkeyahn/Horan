using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class MonsterController : UnitController
{
    public BTRunner Runner { get; protected set; }
    public Animator Anim { get; protected set; }
    public NavMeshAgent Nav { get; protected set; }
    public MonsterStat Stat { get; protected set; }

    protected bool isDead;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
        Stat = GetComponent<MonsterStat>();
        Nav = GetComponent<NavMeshAgent>();
    }
    protected virtual void Start()
    {
        SpawnPoint = transform.position;
        Managers.ContentsManager.SpawnUnit(MyName);
    }
    private void Update()
    {
        Runner.Operate();
        Runner.ServiceOperate();
    }
    public bool IsAnimationRunning(string stateName)
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

    protected float waitsecond;

    protected void StopUnit(bool isStop)
    {
        Runner.isActive = !isStop;
        Nav.isStopped = isStop;
        Nav.velocity = Vector3.zero;
    }
    protected void StopUnit(float second=-1)
    {
        if (second <= 0) return;
        waitsecond = second;
        StartCoroutine("STOP");
    }
    protected IEnumerator STOP()
    {
        StopUnit(true);
        yield return new WaitForSeconds(waitsecond);
        StopUnit(false);
        waitsecond = 0;
    }

    protected virtual void HitEffect()
    {
        StopUnit(true);
    }
    protected virtual void Dead()
    {
        Managers.ContentsManager.DeadUnit(MyName);
        isDead = true;
        StopUnit(true);
        StopAllCoroutines();
        StartCoroutine("DEAD");
    }
    protected IEnumerator DEAD()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    protected GameObject Target;
    protected Vector3 SpawnPoint;
    protected Vector3 DestPos;
    protected bool TargetSearch(float range, int targetLayer)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, range, targetLayer);
        for (int i = 0; i < cols.Length; i++)
        {
            Target = cols[i].gameObject;
            if (Target) //set target
                return true;
        }
        // search fail
        Target = null;
        return false;
    }
    protected float GetTargetDistance()
    {
        if (Target == null) return -1;
        return Vector3.Distance(transform.position, Target.transform.position);
    }
    protected bool RotateToTarget(GameObject target)
    {
        if (target)
        {
            Vector3 dir = target.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, dir) <= 45f / 2f) //몬스터 시야각 
                return true;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 5 * Time.deltaTime);
            return false;
        }
        return false;
    }
    protected bool RotateToVector(Vector3 pos,float RotSpeed=5f)
    {
        if (pos!=Vector3.zero)
        {
            Vector3 dir = pos - transform.position;
            if (Vector3.Angle(transform.forward, dir) <= 45f / 2f) //몬스터 시야각 
                return true;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, RotSpeed * Time.deltaTime);
            return false;
        }
        return false;
    }
}

/*
    public virtual void Wandering(Vector3 DestPos)
    { }
    public virtual void Chase(GameObject Target)
    { }
    public virtual void Attack(GameObject Target)
    { }
    public virtual void CombatWait(GameObject Target)
    { }*/



