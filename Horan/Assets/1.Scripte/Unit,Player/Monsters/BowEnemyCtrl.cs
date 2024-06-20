using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BowEnemyCtrl : MonsterController
{
    AIAttackInfo AtkInfo_Default;
    EffectInfo Effect_Atk;
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

        Effect_Atk = new EffectInfo("boss_01_projectiles",new Vector3(0.5f, 2.5f, 0f));
        AtkInfo_Default = new AIAttackInfo("ATTACK", 1.5f, 0.2f, 50, 15, 0, 0, 1,false, Effect_NONE);

        #region BT
        Runner = new BTRunner
        (
           new BT_Service(new BT_Selector(new List<BT_Node>()
           {
                new BT_Decorator(new BT_Selector(new List<BT_Node>()
                {      
                    new BT_Sequence(new List<BT_Node>()
                    {
                        new BT_Task(TryRun),
                        new BT_Task(Move)
                    }),
                    new BT_Sequence(new List<BT_Node>()
                    {                    
                        new BT_Task(Wait),
                        new BT_Task(TryAttak)                    
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
    bool isAtk;

    BT_Node.NodeState TryAttak()
    {
        if (RotateToTarget(Target, 5, 10))
        {
            if (!isAtk)
            {
                waitsecond = 1.5f;
                StartCoroutine(Attack()); 
            }
            return BT_Node.NodeState.Running;
        }
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState TryRun()
    {
        if (isAtk || 5f < Vector3.Distance(transform.position, Target.transform.position))
        {
            Nav.velocity = Vector3.zero;
            return BT_Node.NodeState.Failure;
        }
        if (RunPos == Vector3.zero)
            RunPos = transform.position + new Vector3((transform.position - Target.transform.position).x, 0, (transform.position - Target.transform.position).z)*5;
        if (!CheckNavigableArea(5f, RunPos) || Vector3.Distance(transform.position, RunPos) <= Nav.stoppingDistance)
        {
            Nav.velocity = Vector3.zero;
            waitsecond = 2f;

            RunPos = Vector3.zero;
            return BT_Node.NodeState.Failure;
        }

        DestPos = RunPos;
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState TryWandering()
    {
        if (WanderPos == Vector3.zero)
            WanderPos = SpawnPoint + new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
        NavMeshPath path = new();
        Nav.CalculatePath(WanderPos, path);

        if (Vector3.Distance(transform.position, WanderPos) <= Nav.stoppingDistance || path.status != NavMeshPathStatus.PathComplete)
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
        if (!IsAnimationRunning("WAIT")&& !isAtk)
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
    bool IsAttacking()
    {
        return isAtk;
    }
    #endregion

    bool CheckNavigableArea(float distance, Vector3 checkPosition)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(checkPosition, out hit, distance, NavMesh.AllAreas) && !NavMesh.Raycast(transform.position, hit.position, out hit, NavMesh.AllAreas))
                return true;   
        return false;
    }
    IEnumerator Attack()
    {
        isAtk = true;
        if (!IsAnimationRunning("ATTACK"))
            Anim.Play("ATTACK");
        
        yield return new WaitForSeconds(AtkInfo_Default.animDelay);
        //MoveArrowEffect();
        yield return new WaitForSeconds(AtkInfo_Default.damageTime);
        CheckAttackRange(AtkInfo_Default.atkRange, AtkInfo_Default.atkAngle);

        yield return new WaitForSeconds(0.5f);
        
        Anim.Play("WAIT");
        StopUnit(waitsecond);
        yield return new WaitForSeconds(waitsecond);
        
        isAtk = false;
    }

}
/*    void MoveArrowEffect()
    {
        if (Target!=null)
        {
            Vector3 direction = (Target.transform.position - transform.position).normalized; // 방향
            GameObject gameObject = Managers.PrefabManager.SpawnEffect(Effect_Atk.effectName, transform.position);
            gameObject.transform.rotation = Quaternion.LookRotation(direction); // 화살의 방향 설정
        }
    }
*/