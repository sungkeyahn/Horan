using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class BossACtrl : MonsterController
{
    AIAttackInfo AtkInfo_Dash;
    AIAttackInfo AtkInfo_Jump;

    AIAttackInfo[,] AttackPattern = new AIAttackInfo[2, 3];

    int AtkCount;
    int SelectedPatternNum = -1;
    bool Attacking=false;

    void Awake()
    {
        MyName = "Boss_A";
        Anim = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        Stat = GetComponent<MonsterStat>();
    }
    protected override void Start()
    {
        base.Start(); 
        Stat.OnHit += HitEffect;
        Stat.OnUnitDead += Dead;

        #region ATTACKInfo
        AtkInfo_Dash = new AIAttackInfo("ATTACK_DASH", 0.5f, 2, 5, 90,0.25f, 20, 20, false);
        AtkInfo_Jump = new AIAttackInfo("ATTACK_JUMP", 0.1f, 2, 10, 360, 0.75f, 5, 10, true);
        #endregion

        #region Pattern
        AttackPattern[0,0] = AtkInfo_Dash;
        AttackPattern[0,1] = AtkInfo_Dash;
        AttackPattern[1,0] = AtkInfo_Dash;
        AttackPattern[1,1] = AtkInfo_Jump;
        #endregion

        #region BT
        Runner = new BTRunner
        (
           new BT_Selector(new List<BT_Node>()
           {
               new BT_Decorator(new BT_Selector(new List<BT_Node>()
               {
                    new BT_Decorator(new BT_Task(TryAttak),Attackable),
                    new BT_Decorator( new BT_Selector(new List<BT_Node>()
                    {
                        new BT_Task(Wait),
                        new BT_Sequence(new List<BT_Node>()
                        {
                            new BT_Task(TryChase),
                            new BT_Task(Move)
                        })
                    }),PatternActiving)
               }),HasTarget),
               new BT_Task(FindEnemy)
           })
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

    BT_Node.NodeState TryAttak()
    {
        //다음 패턴 선택 + 공격 초기화
        if (SelectedPatternNum == -1)
        {
            SelectedPatternNum = Random.Range(0, 2);
            AtkCount = 0;
        }

        if (!Attacking&& RotateToTarget(Target))
        {
            StartCoroutine("Attack");
            waitsecond = -1;
            return BT_Node.NodeState.Success;
        }
        return BT_Node.NodeState.Failure;
    }
    BT_Node.NodeState TryChase()
    {
       
        if (AnimPlay("MOVE"))
        {
            Nav.speed = Stat.walkspeed;
            DestPos = Target.transform.position;
            return BT_Node.NodeState.Success;
        }
        return BT_Node.NodeState.Failure;
    }
    BT_Node.NodeState Wait()
    {
        if (0 < waitsecond)
        {
            if (AnimPlay("WAIT"))
            {
                StopUnit(waitsecond);
                return BT_Node.NodeState.Success;
            }
            return BT_Node.NodeState.Running;
        }
        return BT_Node.NodeState.Failure;
    }
    BT_Node.NodeState Move()
    {
        if (Vector3.Distance(transform.position, DestPos) <= Nav.stoppingDistance)
        {
            Nav.speed = 0;
            Nav.velocity = Vector3.zero;
            return BT_Node.NodeState.Failure;
        }
        Nav.isStopped = false;
        Nav.SetDestination(DestPos);
        return BT_Node.NodeState.Success;
    }
    
    BT_Node.NodeState FindEnemy()
    {
        if (Target!=null && Vector3.Distance(Target.transform.position,transform.position)< Stat.sensingrange)
            return BT_Node.NodeState.Running;
        if (TargetSearch(Stat.sensingrange, LayerMask.GetMask("Player")))
            return BT_Node.NodeState.Success;
            return BT_Node.NodeState.Failure;
    }


    #region Deco
    bool HasTarget()
    {
        if (Target)
            return true;
        else
            return false;
    }
    bool Attackable()
    {
        if (GetTargetDistance() <= Stat.atkrange)
            return true;
        else
            return false;
    }
    bool PatternActiving()
    {
        if (Attacking)
            return false; 
        return true;
    }
    #endregion

    bool AnimPlay(string palyAnimName)
    {
        if (!string.IsNullOrEmpty(palyAnimName) && !IsAnimationRunning(palyAnimName))
        {
            Anim.Play(palyAnimName);
            return true;
        }
        return false;
    }
    IEnumerator Attack()
    {
        Attacking = true;

        AIAttackInfo info = AttackPattern[SelectedPatternNum, AtkCount];

        AtkCount++;

        Anim.Play(info.palyAnimName);
        yield return new WaitForSeconds(info.animdelay);
        Nav.speed = info.movespeed;
        DestPos = transform.position + transform.forward* info.movedistance;
        if (info.navoff) StartCoroutine(AttackMove(DestPos));
        
        yield return new WaitForSeconds(info.atktime);
        CheckAttackRange(info.range, info.angle);
        yield return new WaitForSeconds(.5f);


        if (2 <= AtkCount)
            SelectedPatternNum = -1;

        StopUnit(.5f);
        Attacking = false;
    }
    IEnumerator AttackMove(Vector3 targetPosition)
    {
        // 점프 시작
        float jumpDuration = 0.5f;
        float jumpHeight = 1.5f;
        Nav.enabled = false; // NavMeshAgent 비활성화
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(targetPosition.x, targetPosition.y + jumpHeight, targetPosition.z);

        float elapsedTime = 0;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * jumpHeight;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 점프 종료 후 목표 지점에 위치 설정
        transform.position = targetPosition;
        Nav.enabled = true; // NavMeshAgent 활성화
    }
    /*    void Dash()
    {
        Nav.isStopped = false;
        Nav.SetDestination(DestPos);
    }*/

}
