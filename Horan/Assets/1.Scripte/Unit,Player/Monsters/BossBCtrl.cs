using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBCtrl : MonsterController
{
    AIAttackInfo AtkInfo_Default1;
    AIAttackInfo AtkInfo_Default2;
    AIAttackInfo AtkInfo_Default3; //패턴1 3타 공격

    AIAttackInfo AtkInfo_Dash1; //패턴 2 대쉬 1타 
    AIAttackInfo AtkInfo_Dash2; //추격용 대시 패턴 1
    AIAttackInfo AtkInfo_Jump;//패턴 2 차지 점프

    //AIAttackInfo[,] AttackPattern = new AIAttackInfo[3, 4];
    List<List<AIAttackInfo>> AttackPattern = new List<List<AIAttackInfo>>();
    int AtkCount;
    int SelectedPatternNum = -1;
    bool Attacking = false;

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
        AtkInfo_Default1 = new AIAttackInfo("ATTACK_DEFAULT1", 0, 0.25f, 4, 60, 0, 0, 0, false,Effect_NONE);
        AtkInfo_Default2 = new AIAttackInfo("ATTACK_DEFAULT2", 0, 0.25f, 4, 60,  0, 0, 0, false, Effect_NONE);
        AtkInfo_Default3 = new AIAttackInfo("ATTACK_DEFAULT3", 0, 0.25f, 4, 60, 0, 0, 0.5f, false, Effect_NONE);

        AtkInfo_Dash1 = new AIAttackInfo("ATTACK_DASH1", 0.55f, 0.25f, 10, 45,  2, 5, 0.75f, false, Effect_NONE);
        AtkInfo_Dash2 = new AIAttackInfo("ATTACK_DASH2", 0.55f, 0.25f, 10, 45, 2, 5, 0.75f, false, Effect_NONE);
        AtkInfo_Jump = new AIAttackInfo("ATTACK_JUMP", 0.15f, 0.75f, 7, 180,  2.5f, 2, 1, true, Effect_NONE);
        #endregion

        #region Pattern
        AttackPattern.Add(new List<AIAttackInfo> { AtkInfo_Default1, AtkInfo_Default2, AtkInfo_Default3, AtkInfo_Jump });
        AttackPattern.Add(new List<AIAttackInfo> { AtkInfo_Dash1 });
        AttackPattern.Add(new List<AIAttackInfo> { AtkInfo_Dash2 });
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
        Debug.Log($"SelectedPatternNum : {SelectedPatternNum}");
        Debug.Log($"AtkCount : {AtkCount}");

        if (SelectedPatternNum==-1)
        {
            SelectedPatternNum = Random.Range(0, 3);
            AtkCount = 0;
        }
        if (AttackPattern[SelectedPatternNum].Count <= AtkCount)
        {
            SelectedPatternNum = -1;
            return BT_Node.NodeState.Failure;
        }
        if (!Attacking && RotateToTarget(Target) && GetTargetDistance() <= AttackPattern[SelectedPatternNum][AtkCount].atkRange)
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
        if (!Nav.enabled)
            return BT_Node.NodeState.Failure;

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
        if (Target != null && Vector3.Distance(Target.transform.position, transform.position) < Stat.sensingrange)
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
        AIAttackInfo info = AttackPattern[SelectedPatternNum][AtkCount];
        AtkCount++;

        Anim.Play(info.animName);
        yield return new WaitForSeconds(info.animDelay);
        Nav.speed = info.moveSpeed;
        float jump = 0;
        if (info.isJump) jump = 1.5f;
        Vector3 movePos = transform.position + transform.forward * info.moveSpeed;
        if (info.moveSpeed != 0)
            StartCoroutine(AttackMove(movePos, info.moveDuration, jump));

        yield return new WaitForSeconds(info.damageTime);
        CheckAttackRange(info.atkRange, info.atkAngle);
        yield return new WaitForSeconds(info.waitSecond);

        StopUnit(.5f);
        Attacking = false;
    }
    IEnumerator AttackMove(Vector3 targetPosition, float moveDuration, float jumpHeight = 0)
    {
        // 점프 시작
        Nav.enabled = false; // NavMeshAgent 비활성화
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(targetPosition.x, targetPosition.y + jumpHeight, targetPosition.z);

        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * jumpHeight;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 점프 종료 후 목표 지점에 위치 설정
        transform.position = targetPosition;
        Nav.enabled = true; // NavMeshAgent 활성화
    }
}