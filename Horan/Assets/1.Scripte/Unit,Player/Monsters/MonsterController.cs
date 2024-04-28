using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AttackType { Melee, Range }
public class MonsterController : UnitController
{
    public AttackType AttackType { get; protected set; }
    public Animator Anim { get; protected set; }
    public MonsterStat Stat { get; protected set; }
    public NavMeshAgent Nav { get; protected set; }

    protected bool isDead;

    private void Awake()
    {
        Anim = GetComponent<Animator>();
        Stat = GetComponent<MonsterStat>();
        Nav = GetComponent<NavMeshAgent>();
    }
    protected void Start()
    {
        Managers.ContentsManager.WaveMonsterCounts += 1;
        Stat.OnUnitDead += Dead;
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
    protected bool RotateToAtkTarget(GameObject Target)
    {
        if (Target)
        {
            Vector3 dir = Target.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, dir) <= 45f / 2f) //몬스터 시야각 
            {
                return true;
            }
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 5 * Time.deltaTime);
            return false;
        }
        return false;
    }

    protected virtual void Dead()
    {
        Managers.ContentsManager.WaveMonsterCounts -= 1;
        isDead = true;
        Anim.Play("DEAD");
        DropItems();
        
        StopAllCoroutines();
        StartCoroutine("DEAD");
    }
    protected IEnumerator DEAD()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public virtual void Wandering(Vector3 DestPos)
    { }
    public virtual void Chase(GameObject Target)
    { }
    public virtual void Attack(GameObject Target)
    { }
    public virtual void CombatWait(GameObject Target)
    { }

}

/*    public void Wandering(BTRunner runner)
    {

        if (DestPos != Vector3.zero)
            StartCoroutine("WANDERING");
    }
    IEnumerator WANDERING()
    {
        if (IsAnimationRunning("ATTACK")) yield return null;

        Anim.CrossFade("MOVE", 0.1f);
        Vector3 pos = DestPos;
        Nav.SetDestination(pos);
        Nav.isStopped = false;
        Nav.speed = Stat.walkspeed;

        yield return new WaitWhile(()=> Nav.stoppingDistance <= Vector3.Distance(transform.position, pos));

        Anim.CrossFade("WAIT", 0.1f);
        Nav.isStopped = true;
        Nav.velocity = Vector3.zero;
        yield return new WaitForSeconds(1);

    }
    
    public void Chase(BTRunner runner)
    {
        if (IsAnimationRunning("ATTACK"))
            return;
        if (IsAnimationRunning("RUN")) return;

        Anim.Play("RUN");
        Nav.SetDestination(Target.transform.position);
        Nav.isStopped = false;
        Nav.speed = Stat.runspeed;
       
        //StartCoroutine("CHASE");
    }

    bool RotateToAtkTarget()
    {
        if (Target)
        {
            Vector3 dir = Target.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, dir) <= 45f / 2f) //몬스터 시야각 
            {
                return true;
            }
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 5 * Time.deltaTime);
            return false;
        }
        return false;
    }
    public void Attack(BTRunner runner)
    {
        if (RotateToAtkTarget())
            StartCoroutine("ATTACK");
    }
    IEnumerator ATTACK()
    {
        if (IsAnimationRunning("ATTACK"))
        {
            yield return null;
        }
  

        Anim.CrossFade("ATTACK", 0.1f);

        Weapon equippedWeapon = GetComponentInChildren<Weapon>();
        yield return new WaitForSeconds(0.3f); // 공격 활성화 equippedWeapon.AtkDelayTimes[atkCount]
        equippedWeapon.Area.enabled = true;
        yield return new WaitForSeconds(0.3f); // 공격 비활성화 
        equippedWeapon.Area.enabled = false;

        yield return new WaitForSeconds(Anim.GetCurrentAnimatorStateInfo(0).length); //curAnimStateInfo.length - (equippedWeapon.AtkDelayTimes[atkCount]+0.3f)

    }

    public void CombatWait(BTRunner runner)
    {
        StartCoroutine("COMBATWAIT");
    }
    IEnumerator COMBATWAIT() 
    {
        if (IsAnimationRunning("ATTACK")) yield return null;

        if (Vector3.Distance(transform.position, Target.transform.position) < Stat.atkrange * 2)
        {
            Anim.Play("WAIT");
            Nav.isStopped = true;
            yield return new WaitWhile(() => Stat.atkrange * 2 <= Vector3.Distance(transform.position, Target.transform.position));
        }
        else
        {
            Anim.Play("MOVE");
            Nav.isStopped = false;
            Nav.speed = Stat.walkspeed;
            Nav.SetDestination(Target.transform.position);
        }

    }*/

/*
    IEnumerator CHASE() 
    {
        if (IsAnimationRunning("ATTACK"))
            yield return null;

        isActing = true;

        Anim.CrossFade("RUN", 0.1f);
        Nav.SetDestination(Target.transform.position);
        Nav.isStopped = false;
        Nav.speed = Stat.runspeed;

        yield return new WaitForSeconds(0.5f);

        isActing = false;
    }
 * 
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




