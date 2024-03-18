using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController
{
    Animator anim;
    public enum EPlayerAnimState
    {
        IDLE,
        MOVE,
        DASH
    }
    EPlayerAnimState ePlayerAnimState;
    public EPlayerAnimState AnimState 
    { 
        get { return ePlayerAnimState; }
        set 
        { 
            ePlayerAnimState = value;
            if (anim)
            {
                anim.SetInteger("AnimState", (int)ePlayerAnimState);
                switch (ePlayerAnimState)
                {
                    case EPlayerAnimState.IDLE:
                        move.SetMoveDir(Vector3.zero, 0);
                        break;
                    case EPlayerAnimState.MOVE:
                        float hor = Input.GetAxis("Horizontal"); //ad
                        float ver = Input.GetAxis("Vertical"); //ws
                        move.SetMoveDir(new Vector3(hor, 0, ver), stat.speed);
                        anim.SetFloat("WalkX", hor);
                        anim.SetFloat("WalkY", ver);
                        break;
                    case EPlayerAnimState.DASH:
                        break;
                }
               
            }
        } 
    }

    PlayerStat stat;

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
        stat.OnHit += () => { isHit = true; };
    }
    private void Start()
    {
        equippedWeapon = GetComponentInChildren<Weapon>();
      
        AnimState = EPlayerAnimState.IDLE;
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
                    Attack();
                    //Debug.Log("Click");
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
                AnimState = EPlayerAnimState.IDLE;
                break;
            case InputComponent.KeyBoardEvent.Press:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    AnimState = EPlayerAnimState.MOVE;
                if (Input.GetKey(KeyCode.E))
                {
                    Guard();
                }
                break;
            case InputComponent.KeyBoardEvent.ButtonDown:
                if (Input.GetKey(KeyCode.Space))
                    Dash();
                break;
            case InputComponent.KeyBoardEvent.ButtonUp:
                if (isGuard && !Input.GetKeyDown(KeyCode.E))
                {
                    isGuard = false;
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region Move
    MoveComponent move;

    #endregion

    #region Attack

    Weapon equippedWeapon;

    public Action OnAttackStart;
    public Action OnAttackEnd;

    int atkCount;
    bool atkAble=true;

    public void Attack()
    {
        if (equippedWeapon == null) return;
        if (equippedWeapon.AttackNames.Length <= atkCount) return;
        string atkName = equippedWeapon.AttackNames[atkCount];
        if (atkName == null) return;

        if (atkAble)
        {
            atkCount += 1;
            StopCoroutine("ATTACK");
            StartCoroutine("ATTACK", atkName);
        }
        //equippedWeapon.AttackNames[atkCount]
    }
    IEnumerator ATTACK(string atkName) //해당 매개변수를 atkinfo같은 구조체 형식으로 변경해도 전달할 예정
    {
        if (OnAttackStart != null)
            OnAttackStart.Invoke();

        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play(atkName, -1, 0);
        atkAble = false;

        yield return new WaitForSeconds(0.3f); // 모션 선 딜레이

        equippedWeapon.Area.enabled = true;
        yield return new WaitForSeconds(0.3f); // 공격 판정 
        equippedWeapon.Area.enabled = false;

        yield return new WaitForSeconds(0.25f); //연속 공격 판정
        atkAble = true;

        yield return new WaitForSeconds(curAnimStateInfo.length-0.85f);   // 공격 모션 완전 종료
        atkCount = 0;

        if (OnAttackEnd != null)
            OnAttackEnd.Invoke();
    }
    
    #endregion

    #region Guard

    bool isGuard;
    public bool isHit; 
    float GuardSuccessTime=0.3f;

    void Guard()
    {
        if (!isGuard)
        {
            isGuard = true;
            StartCoroutine("GUARD");
        }
    }
    IEnumerator GUARD()
    {
        isHit = false;
        stat.isDamageable = !isGuard;
        anim.SetBool("GuardEnd", false);
        anim.Play("GUARD");
        yield return new WaitForSeconds(GuardSuccessTime); //해당 시간안에 공격이 들어올시 카운터 어택 (이 시간은 아마 가드애니메이션 선딜시간이될듯?)

        //while() 으로 스텟이 다 떨어질때 까지 가드하고 있도록 할예정 지금은 무제한 
        if (isHit)
        {
            stat.isDamageable = true;
            anim.SetBool("GuardEnd", true);
            StartCoroutine("ATTACK", "COUNTER");
        }

        yield return new WaitUntil(() => !isGuard);
        anim.SetBool("GuardEnd",true);
        stat.isDamageable = !isGuard;
    }

  
    #endregion

    #region Dash

    const float DashCoolTime=3.0f;
    [SerializeField]
    float DashCount = 1;
    bool isDash;
    void Dash()
    {
        //무적 추가 예정 
        if  (0 < DashCount&&!isDash)
        {
            StartCoroutine("DASH");
        }
    }
    IEnumerator DASH()
    {
        isDash = true;
        DashCount -= 1;
        move.MoveActive = false;

        anim.Play("DASH");      
        move.MoveByPower(transform.forward,10);
        yield return new WaitForSeconds(0.4f); //애니메이션 모션 
   
        isDash = false;
        move.MoveActive = true;
   
        yield return new WaitForSeconds(DashCoolTime);
 
        DashCount += 1;
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

    //행동 제어 구조화 고민좀 하기 
    //이후 몬스터 제작 
}
