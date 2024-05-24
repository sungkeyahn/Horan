using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BowEnemyCtrl : MonsterController
{
    AIAttackInfo AtkInfo_Default;
    void Awake()
    {
        MyName = "Soldier_Bow";
        Anim = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        Stat = GetComponent<MonsterStat>();
    }
    protected override void Start()
    {
        base.Start();
        Stat.OnHit += HitEffect;
        Stat.OnUnitDead += Dead;

        AtkInfo_Default = new AIAttackInfo("ATTACK", 1.5f, 2f, 50, 30, 0.15f, 0, 0, false);

        #region BT
        Runner = new BTRunner
        (
           new BT_Service(new BT_Selector(new List<BT_Node>()
           {
                new BT_Decorator(new BT_Selector(new List<BT_Node>()
                {
                    new BT_Decorator(new BT_Sequence(new List<BT_Node>()
                    {
                        new BT_Selector(new List<BT_Node>()
                        {
                           new BT_Decorator(new BT_Sequence(new List<BT_Node>()
                           {                 
                               new BT_Task(TryRun),
                               new BT_Task(Move)
                           }),IsClose),
                           new BT_Sequence(new List<BT_Node>()
                           {                           
                               new BT_Task(TryAttak),
                               new BT_Task(Wait) 
                           })
                        })
                    }),IsAttackable)
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
           }), gameObject, FindEnemy, .5f)
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

    Vector3 WanderPos;
    Vector3 RunPos;

    BT_Node.NodeState TryAttak()
    {
        if (RotateToTarget(Target,5,30) && GetTargetDistance() < Stat.sensingrange)
        {
            if (!IsAnimationRunning("ATTACK"))
            {
                StartCoroutine("Attack");
                //Anim.Play("ATTACK");
            }
            waitsecond = 1.5f;
            return BT_Node.NodeState.Success;
        }
        return BT_Node.NodeState.Failure;
    }
    BT_Node.NodeState TryRun()
    {
        //이 조건 에서 뒤편 방향에 공간이 존재하는 지 확인 후 이동 가능을 결정
        if (RunPos == Vector3.zero)
            RunPos = transform.position + new Vector3((transform.position - Target.transform.position).x, 0, (transform.position - Target.transform.position).z)*5;
        bool isNavigable = CheckNavigableArea(5f, RunPos);
        if (!isNavigable || Vector3.Distance(transform.position, RunPos) <= Nav.stoppingDistance)
        {
            Nav.velocity = Vector3.zero;
            RunPos = Vector3.zero;

            waitsecond = 2f;
            return BT_Node.NodeState.Failure;
        }
        DestPos = RunPos;
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState TryWandering()
    {
        if (WanderPos == Vector3.zero)
            WanderPos = SpawnPoint + new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));

        if (Vector3.Distance(transform.position, WanderPos) <= Nav.stoppingDistance)
        {
            Nav.velocity = Vector3.zero;
            waitsecond = 1;

            WanderPos = Vector3.zero;
            return BT_Node.NodeState.Failure;
        }

        DestPos = WanderPos;
        return BT_Node.NodeState.Success;
    }

    BT_Node.NodeState Wait()
    {
        if (!IsAnimationRunning("WAIT"))
            Anim.Play("WAIT");
        StopUnit(waitsecond);
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Move()
    {
        if (Nav.stoppingDistance < Vector3.Distance(transform.position, DestPos))
        {
            if (!IsAnimationRunning("MOVE"))
                Anim.Play("MOVE");
            Nav.speed = Stat.runspeed;
            Nav.isStopped = false;
            Nav.SetDestination(DestPos); 
            return BT_Node.NodeState.Success;
        }
        return BT_Node.NodeState.Failure;
    }

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
        if (GetTargetDistance() <= Stat.sensingrange)
            return true;
        else
            return false;
    }
    bool IsClose()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) <= Stat.atkrange)
            return true;
        else
            return false;
    }
    #endregion

    bool CheckNavigableArea(float distance, Vector3 checkPosition)
    {
        // NavMesh 영역 내에서의 위치를 샘플링
        NavMeshHit hit;
        if (NavMesh.SamplePosition(checkPosition, out hit, distance, NavMesh.AllAreas))
        {
            if (!NavMesh.Raycast(transform.position, hit.position, out hit, NavMesh.AllAreas))
            {
                return true;
            }
        }
        return false;
    }
    IEnumerator Attack()
    {
        yield return null;
        Anim.Play("ATTACK");
        yield return new WaitForSeconds(AtkInfo_Default.animdelay);
        yield return new WaitForSeconds(AtkInfo_Default.atktime);
        CheckAttackRange(AtkInfo_Default.range, AtkInfo_Default.angle);
    }

}
