using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterController : UnitController
{
    Animator Anim;
    
    PlayerStat stat;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        stat = GetComponent<PlayerStat>();
        State = MonsterState.DEFAULT;
        CurAction = MonsterAcion.WAIT;
        //stat.OnHit += Hit;
    }
    void Hit()
    {
        StopCoroutine("HIT");
        StartCoroutine("HIT");
    }
    IEnumerator HIT()
    {
        Anim.Play("HIT", -1, 0);
        yield return new WaitForSeconds(0.1f); // 피격 경직 이전 피격 애니메이션 재생  
        Anim.speed = 0f;
        yield return new WaitForSeconds(0.3f); // 피격 경직 시작   
        Anim.speed = 1f;
    }

    float GetPlayerDistance() //플레이어 거리 탐지 
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return -1;

        float distance = (player.transform.position - transform.position).magnitude;

        return distance;
    }
    public enum MonsterState
    {
        DIE,
        DEFAULT,
        ANGER,//해당 상태에서 플레이어와의 거리에 따라 ,공격 범위에 따라 다른 행동을 호출
    }
    public MonsterState STATE
    {
        get { return State; }
        set
        {
            State = value;
        }
    }
    public Action OnStateChanged;

    public enum MonsterAcion
    {
        WAIT,
        MOVE,
        CHASE,
        ATTACK,
        HIT,
    }
    public MonsterAcion ACTION
    {
        get { return CurAction; }
        set
        {
            CurAction = value;
            DoAcion();
        }
    }

    [SerializeField]
    MonsterState State;
    [SerializeField]
    MonsterAcion CurAction;

    [SerializeField]
    float WalkSpeed = 4.0f;
    [SerializeField]
    float RunSpeed = 6.0f;
    [SerializeField]
    float ATKRange = 4.0f;
    [SerializeField]
    float SensRange = 15.0f;
    [SerializeField]
    float WanderInterval = 3.0f;
    [SerializeField]
    float WanderTime = 0;

    NavMeshAgent Nav;

    Vector3 DestPos;


    void Update()
    {
        //Debug.Log(Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position));
        UpdateState();
    }
    void UpdateState()
    {
        if (Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position) <= SensRange)
            STATE = MonsterState.ANGER;
        else
            State = MonsterState.DEFAULT;

        switch (State)
        {
            case MonsterState.DIE:
                break;
            case MonsterState.DEFAULT:
                {
                    WanderTime += Time.deltaTime;
                    if (WanderInterval < WanderTime)
                    {
                        ACTION = MonsterAcion.MOVE;
                    }
                    else if ((DestPos - transform.position).magnitude < 1.0f)
                    {
                        ACTION = MonsterAcion.WAIT;
                    }
                }
                break;
            case MonsterState.ANGER:
                {
                    if (Vector3.Distance(transform.position, FindObjectOfType<PlayerController>().transform.position) <= ATKRange)
                        ACTION = MonsterAcion.ATTACK;
                    else
                        ACTION = MonsterAcion.CHASE;
                }
                break;
            default:
                break;
        }

        //OnStateChanged.Invoke(); 
    }

    void DoAcion()
    {
        switch (CurAction)
        {
            case MonsterAcion.WAIT:
                Anim.Play("WAIT");
                Nav.speed = 0;
                break;
            case MonsterAcion.MOVE:
                Anim.Play("MOVE");
                DestPos = transform.position + new Vector3(UnityEngine.Random.Range(-5, 5), 0, UnityEngine.Random.Range(-5, 5));
                Nav.SetDestination(DestPos);
                Nav.speed = WalkSpeed;
                WanderTime = 0;
                break;
            case MonsterAcion.CHASE:
                Anim.Play("RUN");
                DestPos = FindObjectOfType<PlayerController>().transform.position;
                Nav.SetDestination(DestPos);
                Nav.speed = RunSpeed;
                break;
            case MonsterAcion.ATTACK:
                if (!isAttacking)
                {
                    Anim.Play("ATTACK");
                    StartCoroutine("ATTACK");
                    Nav.speed = 0;
                }
                break;
            case MonsterAcion.HIT:
                break;
            default:
                break;
        }
    }

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

}
