using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BowEnemyCtrl : MonsterController
{
    void Awake()
    {
        MyName = "Soldier_Bow";
        Anim = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        Stat = GetComponent<MonsterStat>();
        
        //데미지 들어가는 구조 어떻게?
        //enemyattack 라는 클래스 에서 해당 몬스터가 할수 있는 공격 과 공격 범위에 대한 정보를 가지고 있게 하는것이?

        //발사체 구현-> 공격시 발사체를 생성해서 발사하는 코드가 필요함
        
        //범위공격 -> 특정 범위를 공격 타이밍에 체크해서 플레이어가 존재하면 데미지
        //이 방식으로 가자 어택 박스 삭제하고 
        
        //atkbox1 = GetComponentInChildren<EnemyAttackBox>();

        /*궁수는 일단 추격+순찰이 없고 
         * "도망가기"가 존재하는 걸로
         *일단 배치되어있는 곳에 대기하다가 엄청 큰범위 를 공격범위에 들어오면
         *공격 시작-> 가까히 다가오면 일정 시간마다 뒤로 도망
         *단 뒤로 도망갈때 속도를 빠르게 + 뒤에 장애물이 존재하는 지 판단해서 장애물이 존재하면 그냥 공격하는걸로
         */
    }
    protected override void Start()
    {
        base.Start();
        Stat.OnUnitTakeDamaged += HitEffect;
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
        Debug.Log(1);
        if (RotateToTarget(Target) && GetTargetDistance() < Stat.sensingrange)
        {
            if (!IsAnimationRunning("ATTACK"))
                Anim.Play("ATTACK");
            waitsecond = 1.5f;
            return BT_Node.NodeState.Success;
        }
        return BT_Node.NodeState.Failure;
    }
    BT_Node.NodeState TryRun()
    {
        //이 조건문 에서 뒤편 방향에 공간이 존재하는 지 확인 후 이동 가능을 결정
        if (RunPos == Vector3.zero)
            RunPos = transform.position + new Vector3((transform.position - Target.transform.position).x, 0, (transform.position - Target.transform.position).z)*5;
        bool isNavigable = CheckNavigableArea(5f, RunPos);
        if (!isNavigable || Vector3.Distance(transform.position, RunPos) <= Nav.stoppingDistance)
        {
            Debug.Log("RunFail");
            Nav.velocity = Vector3.zero;
            RunPos = Vector3.zero;

            waitsecond = 2f;
            return BT_Node.NodeState.Failure;
        }
        //RotateToVector(RunPos);
        Debug.Log(RunPos);
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

}
