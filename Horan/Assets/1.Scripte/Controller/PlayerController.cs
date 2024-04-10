using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController
{
    public enum EPlayerAnimState
    {
        IDLE,
        MOVE,
        DASH
    }

    Animator anim;
    PlayerStat stat;
    ActComponent Act;

    Weapon equippedWeapon;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        input = GetComponent<InputComponent>();
        input.MouseAction -= OnPlayerMouseEvent;
        input.MouseAction += OnPlayerMouseEvent;
        input.KeyAction -= OnPlayerKeyBoardEvent;
        input.KeyAction += OnPlayerKeyBoardEvent;

        move = GetComponent<MoveComponent>();
        stat = GetComponent<PlayerStat>();
    }
    private void Start()
    {
        //수정 예정 코드
        equippedWeapon = GetComponentInChildren<Weapon>();

        #region Acts 
        Act attack = new Act((int)KindOfAct.Attack, Attack);
        attack.AddAllowActID((int)KindOfAct.Attack);
        attack.AddAllowActID((int)KindOfAct.Guard);
        attack.AddAllowActID((int)KindOfAct.Dash);

        Act move = new Act((int)KindOfAct.Move, Move);
        move.AddAllowActID((int)KindOfAct.Dash);
        move.AddAllowActID((int)KindOfAct.Move);

        Act guard = new Act((int)KindOfAct.Guard, Guard);
        guard.AddAllowActID((int)KindOfAct.Attack);
        guard.AddAllowActID((int)KindOfAct.Counter);

        Act dash = new Act((int)KindOfAct.Dash, Dash);
        dash.AddAllowActID((int)KindOfAct.Dash);
        dash.AddAllowActID((int)KindOfAct.DashAttack);

        Act dashAtttack = new Act((int)KindOfAct.DashAttack, DashAttack);

        Act counter = new Act((int)KindOfAct.Counter, Counter);

        //행동 등록
        Act = GetComponent<ActComponent>();
        Act.AddAct(attack);
        Act.AddAct(move);
        Act.AddAct(guard);
        Act.AddAct(dash);
        Act.AddAct(dashAtttack);
        Act.AddAct(counter);
        //행동 실행 및 종료 호출
        //Act.Execution((int)KindOfAct.Attack);
        //Act.Finish((int)KindOfAct.Attack);

        #endregion
        stat.OnHit += ()=>isCounter = true; 
    }

    #region Input
    InputComponent input;
    void OnPlayerMouseEvent(InputComponent.MouseEvent evt)
    {
        switch (evt)
        {
            case InputComponent.MouseEvent.None:
                {
                }
                break;
            case InputComponent.MouseEvent.Press:
                {

                    // Debug.Log("Press");
                }
                break;
            case InputComponent.MouseEvent.PointerDown:
                {
                    // Debug.Log("PointerDown");
                }
                break;
            case InputComponent.MouseEvent.PointerUp:
                {
                    // Debug.Log("PointerUp");
                }
                break;
            case InputComponent.MouseEvent.Click:
                {
                    if (atkAble && atkCount < equippedWeapon.AttackAnimNames.Length)
                    {
                        Act.Execution((int)KindOfAct.Attack);
                        DashAtkInput = true;
                    }//Debug.Log("Click");
                }
                break;
            default:
                break;
        }
    }
    void OnPlayerKeyBoardEvent(InputComponent.KeyBoardEvent evt)
    {
        switch (evt)
        {
            case InputComponent.KeyBoardEvent.None:
                {
                    Act.Finish((int)KindOfAct.Move);
                    move.SetMoveDir(Vector3.zero, 0);
                    anim.SetInteger("AnimState", (int)EPlayerAnimState.IDLE);
                }
                break;
            case InputComponent.KeyBoardEvent.Press:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    Act.Execution((int)KindOfAct.Move);
                }
                if (Input.GetKey(KeyCode.E)&& !isGuard)
                {
                    Act.Execution((int)KindOfAct.Guard);
                }
                break;
            case InputComponent.KeyBoardEvent.ButtonDown:
                if (Input.GetKey(KeyCode.Space) && 0 < DashCount && stat.UseSP(20))
                    Act.Execution((int)KindOfAct.Dash);
                break;
            case InputComponent.KeyBoardEvent.ButtonUp:
                if (isGuard && !Input.GetKeyDown(KeyCode.E))
                {
                    isGuard = false;
                    Act.Finish((int)KindOfAct.Guard);
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region Move
    MoveComponent move;
    void Move()
    {
        float hor = Input.GetAxis("Horizontal"); //ad
        float ver = Input.GetAxis("Vertical"); //ws
        move.SetMoveDir(new Vector3(hor, 0, ver), stat.speed);
        anim.SetInteger("AnimState", (int)EPlayerAnimState.MOVE);
        anim.SetFloat("WalkX", hor);
        anim.SetFloat("WalkY", ver);
    }
    #endregion

    #region Attack

    bool atkAble = true;
    int atkCount = 0;

    void Attack()
    {
        StopCoroutine("ATTACK");
        StartCoroutine("ATTACK");
    }
    IEnumerator ATTACK()
    {
        if (atkCount < equippedWeapon.AttackAnimNames.Length)
        {
            anim.Play(equippedWeapon.AttackAnimNames[atkCount], -1, 0);
            atkCount += 1;
            atkAble = false;
        }
        else
        {
            atkCount = 0;
            Act.Finish((int)KindOfAct.Attack);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f); // 공격 활성화 equippedWeapon.AtkDelayTimes[atkCount]
        equippedWeapon.Area.enabled = true;
        yield return new WaitForSeconds(0.3f); // 공격 비활성화 
        equippedWeapon.Area.enabled = false;

        atkAble = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - 0.8f); //curAnimStateInfo.length - (equippedWeapon.AtkDelayTimes[atkCount]+0.3f)
        atkCount = 0;
        Act.Finish((int)KindOfAct.Attack);
    }

    #endregion

    #region Guard

    bool isGuard;
    public bool isCounter; //해당 변수는 stat에서 접근해서 변경해야 함
    const float GuardSuccessTime = 0.3f;

    void Guard()
    {
        StartCoroutine("GUARD");
    }
    IEnumerator GUARD()
    {
        isGuard = true;
        anim.Play("GUARD");
        anim.SetBool("GuardEnd", false);

        stat.isDamageable = false;

        isCounter = false;
        yield return new WaitForSeconds(GuardSuccessTime); //해당 시간안에 공격이 들어올시 카운터 어택 (이 시간은 아마 가드애니메이션 선딜시간이될듯?)
        if (isCounter)
        {
            Act.Execution((int)KindOfAct.Counter);
            yield return null;
        }

        yield return new WaitUntil(() => !isGuard);
        isGuard = false;
        anim.SetBool("GuardEnd", true);

        stat.isDamageable = true;
    }

    void Counter()
    {
        StartCoroutine("COUNTER");
    }
    IEnumerator COUNTER()
    {
        anim.SetBool("GuardEnd", true);

        anim.Play("COUNTER");

        equippedWeapon.Area.enabled = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); // 공격 활성화 
        equippedWeapon.Area.enabled = false;

        isGuard = false;

        stat.isDamageable = true;
       
        Act.Finish((int)KindOfAct.Counter);
    }


    #endregion

    #region Dash

    const float DashCoolTime = 3.0f;
    [SerializeField]
    float DashCount = 1;
    void Dash()
    {
        Debug.Log(stat.sp);
        StopCoroutine("DASH");
        StartCoroutine("DASH");
    }
    IEnumerator DASH()
    {
        DashCount -= 1;
        stat.isDamageable = false;

        move.MoveByPower(transform.forward, 10);
        anim.Play("DASH");

        DashAtkInput = false;
        yield return new WaitForSeconds(0.2f);
        if (DashAtkInput)
        {
            Act.Execution((int)KindOfAct.DashAttack);
            yield return null;
        }


        yield return new WaitForSeconds(0.2f); //애니메이션 모션 
        stat.isDamageable = false;
        Act.Finish((int)KindOfAct.Dash);

        yield return new WaitForSeconds(DashCoolTime);
        DashCount += 1;
    }

    bool DashAtkInput; 
    void DashAttack()
    {
        StartCoroutine("DASHATTACK");
    }
    IEnumerator DASHATTACK()
    {
        anim.Play("DASHATTACK");
        equippedWeapon.Area.enabled = true;
        yield return new WaitForSeconds(0.3f); // 공격 활성화 
        equippedWeapon.Area.enabled = false;

        yield return new WaitForSeconds(0.7f);//애니메이션 종료
        Act.Finish((int)KindOfAct.DashAttack);
    }
    #endregion

    #region Loot&Equip
    void Loot()
    {
        //루팅 애니메이션 실행  + 범위감지->아이템 습득->인벤토리 저장 + 아이템 감지 박스 필요 
    }
    void Equip()
    {
        //인벤토리UI에서 아이템 선택후 이 함수 호출시  해당 아이템을 장착   
        //인벤토리 기능 구현 필요 
    }
    void WeaponEquip(Weapon weapon)
    {
        //만약 기존에 장비하고 있던 장비가 존재한다면 장착된 장비를 비활성화 이후 장착하는 코드필요
    }
    #endregion
}
