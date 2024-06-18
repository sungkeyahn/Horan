using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordEnemyCtrl : MonsterController
{
    const float SenseRange = 8;
    const float AttackRange = 2;
    const float PlayerSerchInterval = 1;
    const float HPRatioCheckInterval = 1;
    const float WanderingInterval = 5;
    const float DangersHPRatio = 50;
    EnemyAttackBox atkbox1;

    void Awake()
    {
        MyName = "Soldier_Sword";
        Anim = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        Stat = GetComponent<MonsterStat>();
        atkbox1 = GetComponentInChildren<EnemyAttackBox>();
    }
    protected override void Start()
    {
        base.Start();
        Stat.OnHit += HitEffect;
        Stat.OnUnitDead += Dead;

        #region BT
        Runner = new BTRunner
        (
           new BT_Service(new BT_Selector(new List<BT_Node>()
           {  
                new BT_Decorator(new BT_Selector(new List<BT_Node>()
                {
                    new BT_Decorator(new BT_Sequence(new List<BT_Node>()
                    {
                        new BT_Task(TryAttak),
                        new BT_Task(Wait)
                    }),IsAttackable),                   
                    new BT_Sequence(new List<BT_Node>()
                    {
                        new BT_Task(TryChase),
                        new BT_Task(Move)
                    })
                }),HasTarget),
                new BT_Selector(new List<BT_Node>()
                {
                    new BT_Sequence(new List<BT_Node>()
                    {                     
                        new BT_Task(TryWandering),
                        new BT_Task(Move),
                    }),
                    new BT_Task(Wait)
                })
           }),gameObject,FindEnemy,0.3f)
        );
        #endregion
    }
    protected override void HitEffect()
    {
        base.HitEffect();
        Stat.isDamageable = true;
    }
    protected override void Dead()
    {
        base.Dead();
        Anim.Play("DEAD");
    }


    #region Task   
    Vector3 WanderPos;
    BT_Node.NodeState TryAttak()
    {
        if (RotateToTarget(Target) && GetTargetDistance()<Stat.atkrange/2)
        {
            if (!IsAnimationRunning("ATTACK"))
                Anim.Play("ATTACK");
            waitsecond = 2;
            return BT_Node.NodeState.Success;
        }
        return BT_Node.NodeState.Failure;
    }
    BT_Node.NodeState TryChase()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) <= Nav.stoppingDistance)
        {
            Nav.speed = 0;
            Nav.velocity =Vector3.zero;
            return BT_Node.NodeState.Failure;
        }
        DestPos = Target.transform.position;

        
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState TryWandering()
    {
        if (WanderPos == Vector3.zero)
            WanderPos = SpawnPoint + new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
       
        NavMeshPath path = new();
        Nav.CalculatePath(WanderPos, path);
       // NavMeshHit hit; !NavMesh.SamplePosition(WanderPos, out hit, 1.0f, NavMesh.AllAreas)
        if (Vector3.Distance(transform.position, WanderPos) <= Nav.stoppingDistance || path.status != NavMeshPathStatus.PathComplete)
        {
            Nav.speed = 0;
            Nav.velocity = Vector3.zero;
            waitsecond = 3; 


            WanderPos = Vector3.zero;
            return BT_Node.NodeState.Failure;
        }

        DestPos = WanderPos;

        return BT_Node.NodeState.Success;
    }

    BT_Node.NodeState Wait()
    {
        if (!IsAnimationRunning("WAIT"))
            Anim.SetInteger("AnimState", 0);
        StopUnit(waitsecond);
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Move()
    {
        if (Nav.stoppingDistance < Vector3.Distance(transform.position, DestPos))
        {
            if (!IsAnimationRunning("MOVE"))
                Anim.SetInteger("AnimState", 1);
            Nav.isStopped = false;
            Nav.speed = Stat.walkspeed;
            Nav.SetDestination(DestPos);
        }
        return BT_Node.NodeState.Success; 
    }
    #endregion

    #region Service
    void FindEnemy()
    {
        TargetSearch(Stat.sensingrange, LayerMask.GetMask("Player"));
    }
    #endregion

    #region Deco
    bool HasTarget()
    {
        if (Target)
            return true;
        else
            return false;
    }
    bool IsAttackable()
    {
        if (GetTargetDistance() <= Stat.atkrange)
            return true;
        else
            return false;
    }
    #endregion

    void AttackStart()
    {
        atkbox1.Area.enabled = true;

    }
    void AttackEnd()
    {
        atkbox1.Area.enabled = false; 

    }
}
