using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public struct AIAttackInfo
{
    public AIAttackInfo(string AnimName, float AnimDelay , float DamageTime,float AtkRange,float AtkAngle,float MoveDuration,float MoveSpeed,float WaitSecond,bool IsJump, EffectInfo EffectInfo)
    {
        animName = AnimName;
        animDelay= AnimDelay;
        damageTime = DamageTime;

        atkRange = AtkRange;
        atkAngle = AtkAngle;
        moveDuration = MoveDuration;
        moveSpeed = MoveSpeed;

        waitSecond = WaitSecond;
        isJump = IsJump;

        effectInfo = EffectInfo;
    }
    public string animName;
    public float animDelay;
    public float damageTime;

    public float atkRange;
    public float atkAngle;

    public float moveDuration;
    public float moveSpeed;

    public bool isJump;
    public float waitSecond;

    public EffectInfo effectInfo;
}
public struct EffectInfo
{
    public EffectInfo(string EffectName , Vector3 StartlocPos,float StartDelay=0f)
    {
        effectName = EffectName;
        startPos = StartlocPos;
        startDelay = StartDelay;
    }
    public string effectName;
    public Vector3 startPos;
    public float startDelay;
}
public  class MonsterController : UnitController
{
    public BTRunner Runner { get; protected set; }
    public Animator Anim { get; protected set; }
    public NavMeshAgent Nav { get; protected set; }
    public MonsterStat Stat { get; protected set; }

    protected bool isDead;
    protected float waitsecond;
    protected GameObject Target=null;
    protected Vector3 SpawnPoint;
    protected Vector3 DestPos;

    protected EffectInfo Effect_NONE = new EffectInfo("", Vector3.zero, 0f);



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

    protected void StopUnit(bool isStop)
    {
        Runner.isActive = !isStop;
        if (Nav.enabled)
        {
            Nav.isStopped = isStop;
            Nav.velocity = Vector3.zero;
        }
    }
    protected void StopUnit(float second=-1)
    {
        if (second <= 0) return;
        waitsecond = second;
        StartCoroutine(STOP());
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
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Hit1", transform));
        //StopUnit(true);
    }
    protected virtual void Dead()
    {
        isDead = true;

        Managers.ContentsManager.DeadUnit(MyName);
 
        StopUnit(true);
        StopAllCoroutines();
        StartCoroutine("DEAD");
    }
    protected IEnumerator DEAD()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

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
    protected bool RotateToTarget(GameObject target, float RotSpeed = 5f,float RotAngle=45f)
    {
        if (target)
        {
            Vector3 dir = target.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, dir) <= RotAngle / 2f) //몬스터 시야각 
                return true;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, RotSpeed * Time.deltaTime);
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

    float AttackDistance =0;
    float AngleRange =0;
    protected bool CheckAttackRange(float attackRange = 3,float atkAngle = 90)
    {
        if (Target == null) return false;
       
        AttackDistance = attackRange;
        AngleRange = atkAngle;
        float dotValue = Mathf.Cos(Mathf.Deg2Rad * (atkAngle * .5f));
        Vector3 direction = Target.transform.position - transform.position;
        if (direction.magnitude < attackRange)
        {
            if (Vector3.Dot(direction.normalized, transform.forward) > dotValue)
            {
                IDamageInteraction damage = Target.GetComponent<IDamageInteraction>();
                if (damage != null)
                {
                    if (damage.TakeDamage(Stat.damage))
                    {
                        if (Stat.damage < 5)
                           Managers.PrefabManager.SpawnEffect("Hit_01", Target.transform.position);
                        else
                            Managers.PrefabManager.SpawnEffect("Hit_04", Target.transform.position);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    protected IEnumerator SpwanEffect(EffectInfo info)
    {
        yield return new WaitForSeconds(info.startDelay);
        Managers.PrefabManager.SpawnEffect(info.effectName, transform, info.startPos);
        Debug.Log(info.effectName);
    }

    void OnDrawGizmos()
    {
        // 기즈모 색상 설정
        Gizmos.color = Color.red;

        // 공격자의 위치와 방향
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        // 공격 범위를 나타내는 원 그리기
        Gizmos.DrawWireSphere(position, AttackDistance);

        // 각도 범위를 나타내는 두 개의 선 그리기
        Quaternion leftRayRotation = Quaternion.Euler(0, -AngleRange / 2, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, AngleRange / 2, 0);

        Vector3 leftRayDirection = leftRayRotation * forward * AttackDistance;
        Vector3 rightRayDirection = rightRayRotation * forward * AttackDistance;

        Gizmos.DrawLine(position, position + leftRayDirection);
        Gizmos.DrawLine(position, position + rightRayDirection);

        // 타겟이 공격 범위 내에 있는지 확인
        if (Target!=null)
        {
            Vector3 direction = Target.transform.position - position;
            if (direction.magnitude < AttackDistance)
            {
                float dotValue = Mathf.Cos(Mathf.Deg2Rad * (AngleRange * 0.5f));
                if (Vector3.Dot(direction.normalized, forward) > dotValue)
                {
                    // 공격 범위 내에 있으면 초록색으로 표시
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(position, Target.transform.position);
                }
            }
        }
    }
}



