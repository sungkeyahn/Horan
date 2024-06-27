using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EPlayerAnimState
{
    IDLE,
    MOVE,
    DASH
}

public class PlayerController : UnitController
{
    HUDUI Hud;

    Animator[]anims;
    
    PlayerStat Stat;
    ActComponent Act;

    InputComponent input; //삭제 보류 
    MoveComponent move;

    public Weapon weapon;
    public Equipment[] equipments = new Equipment[4];

    private void Awake()
    {
        equipments = GetComponentsInChildren<Equipment>();
        weapon = GetComponentInChildren<Weapon>();
        move = GetComponent<MoveComponent>();
        Stat = GetComponent<PlayerStat>();
        input = GetComponent<InputComponent>();
    }
    private void Start()
    {
        #region Input
        //input.MouseAction -= OnPlayerMouseEvent;
        //input.KeyAction -= OnPlayerKeyBoardEvent;
       // input.MouseAction += OnPlayerMouseEvent;
        //input.KeyAction += OnPlayerKeyBoardEvent;
        //input.TouchAction -= OnPlayerTouchEvent;
       // input.TouchAction += OnPlayerTouchEvent;

        #endregion
        #region Stat
        Stat.StatInit(Managers.ContentsManager.level, Managers.ContentsManager.exp, Managers.ContentsManager.hp);
        Stat.OnHit += OnCharacterHit;
        Stat.OnUnitTakeDamaged += OnCharacterTakeDamaged;
        Stat.OnUnitDead += Dead;
        #endregion
        #region Acts 
        Act attack = new Act((int)ECharacterAct.FAttack, Attack);
        attack.AddAllowActID((int)ECharacterAct.FAttack);
        attack.AddAllowActID((int)ECharacterAct.Guard);
        attack.AddAllowActID((int)ECharacterAct.Dash);

        Act sattack = new Act((int)ECharacterAct.SAttack, SAttack);
        sattack.AddAllowActID((int)ECharacterAct.SAttack);
        sattack.AddAllowActID((int)ECharacterAct.Guard);
        sattack.AddAllowActID((int)ECharacterAct.Dash);

        Act move = new Act((int)ECharacterAct.Move, Move);
        move.AddAllowActID((int)ECharacterAct.Dash);
        move.AddAllowActID((int)ECharacterAct.Move);

        Act guard = new Act((int)ECharacterAct.Guard, Guard);
        guard.AddAllowActID((int)ECharacterAct.FAttack);
        guard.AddAllowActID((int)ECharacterAct.SAttack);
        guard.AddAllowActID((int)ECharacterAct.Counter);

        Act dash = new Act((int)ECharacterAct.Dash, Dash);
        dash.AddAllowActID((int)ECharacterAct.Dash);
        dash.AddAllowActID((int)ECharacterAct.DashAttack);

        Act dashAtttack = new Act((int)ECharacterAct.DashAttack, DashAttack);

        Act counter = new Act((int)ECharacterAct.Counter, Counter);

        //행동 등록
        Act = GetComponent<ActComponent>();
        Act.AddAct(attack);
        Act.AddAct(sattack);
        Act.AddAct(move);
        Act.AddAct(guard);
        Act.AddAct(dash);
        Act.AddAct(dashAtttack);
        Act.AddAct(counter);
        #endregion

        Equip();
        Managers.ContentsManager.AbilityContainer.ApplyAllAbility(Stat);
        Hud = Managers.UIManager.ShowSceneUI<HUDUI>();
        Hud.Init(this);
        Hud.OnCharacterAction += OnCharacterBtnEvent;
        Hud.OnCharacterEnd += OnCharacterBtnEndEvent;
    }
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
                    if (atkAble && atkCount < weapon.AnimInfo_FATK.Count)
                    {
                        Act.Execution((int)ECharacterAct.FAttack);
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
                    move.SetMove(0);
                    //anim.SetInteger("AnimState", (int)EPlayerAnimState.IDLE);
                    for (int i = 0; i < anims.Length; i++)
                    {
                        anims[i].SetInteger("AnimState", (int)EPlayerAnimState.IDLE);
                    }
                    Act.Finish((int)ECharacterAct.Move);
                }
                break;
            case InputComponent.KeyBoardEvent.Press:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    Act.Execution((int)ECharacterAct.Move);
                }
                if (Input.GetKey(KeyCode.E) && !isGuard)
                {
                    Act.Execution((int)ECharacterAct.Guard);
                }
                break;
            case InputComponent.KeyBoardEvent.ButtonDown:
                if (Input.GetKey(KeyCode.Space) && 0 < DashCount && Stat.UseSP(20))
                    Act.Execution((int)ECharacterAct.Dash);
                break;
            case InputComponent.KeyBoardEvent.ButtonUp:
                //if (isGuard && !Input.GetKeyDown(KeyCode.E))
               // {
                    //isGuard = false;
                   // Act.Finish((int)ECharacterAct.Guard);
                //}
                break;
            default:
                break;
        }
    }
    void OnCharacterBtnEvent(EPlayerCharacterCtrlEvent ctrlEvent)
    {
        switch (ctrlEvent)
        {
            case EPlayerCharacterCtrlEvent.Move:
                Act.Execution((int)ECharacterAct.Move);
                break;
            case EPlayerCharacterCtrlEvent.Dash:
                if (0 < DashCount)
                    if (Stat.UseSP(20))
                        Act.Execution((int)ECharacterAct.Dash);
                break;
            case EPlayerCharacterCtrlEvent.Guard:
                if (!isGuard)
                    Act.Execution((int)ECharacterAct.Guard);
                break;
            case EPlayerCharacterCtrlEvent.FAttack:
                DashAtkInput = true;
                if (atkAble && atkCount < weapon.AnimInfo_FATK.Count)
                    Act.Execution((int)ECharacterAct.FAttack); 
                break;
            case EPlayerCharacterCtrlEvent.SAttack:
                if (atkAble && atkCount < weapon.AnimInfo_SATK.Count)
                    Act.Execution((int)ECharacterAct.SAttack); 
                break;
        }
    }
    void OnCharacterBtnEndEvent(EPlayerCharacterCtrlEvent ctrlEvent)
    {
        switch (ctrlEvent)
        {
            case EPlayerCharacterCtrlEvent.Move:
                for (int i = 0; i < anims.Length; i++)
                    anims[i].SetInteger("AnimState", (int)EPlayerAnimState.IDLE); 
                move.SetMove(0);
                Act.Finish((int)ECharacterAct.Move);
                break;
            case EPlayerCharacterCtrlEvent.Dash:

                break;
            case EPlayerCharacterCtrlEvent.Guard:

                break;
            case EPlayerCharacterCtrlEvent.FAttack:

                break;
            case EPlayerCharacterCtrlEvent.SAttack:

                break;
        }
    }
    void OnPlayerTouchEvent(Touch evt)
    {
        switch (evt.phase)
        {
            case TouchPhase.Began:
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
                break;
            case TouchPhase.Canceled:
                break;
        }
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Hud.input.x,0,Hud.input.y);
        move.SetMove(moveDir, moveDir, Stat.Speed);

        //Vector3 moveDir = Vector3.zero;
        //moveDir.x = Input.GetAxis("Horizontal");
        //moveDir.z = Input.GetAxis("Vertical");
        //move.SetMove(moveDir, moveDir, Stat.Speed);
        
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].SetInteger("AnimState", (int)EPlayerAnimState.MOVE);
            anims[i].SetFloat("WalkX", moveDir.x);
            anims[i].SetFloat("WalkY", moveDir.z);
        }
    }

    #region Attack
    bool atkAble = true;
    int atkCount = 0;
    void Attack()
    {
        Data.AnimInfomation animinfo = weapon.AnimInfo_FATK[atkCount];

        Stat.atkType = PlayerStat.ECharacterAtkType.FAtk;

        StopCoroutine(ATTACK(animinfo));
        StartCoroutine(ATTACK(animinfo));
    }
    void SAttack()
    {
        Data.AnimInfomation animinfo = weapon.AnimInfo_SATK[atkCount];
        
        Stat.atkType = PlayerStat.ECharacterAtkType.SAtk;

        StopCoroutine(ATTACK(animinfo));
        StartCoroutine(ATTACK(animinfo));
    }
    IEnumerator ATTACK(Data.AnimInfomation animinfo)
    {
        DashAtkInput = true;

        atkCount += 1;

        AttackRotationCorrection();

        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].Play(animinfo.name, -1, 0);
        }

        atkAble = false;
        yield return new WaitForSeconds(animinfo.delay); // 공격 활성화 
        weapon.Area.enabled = true;
        yield return new WaitForSeconds(animinfo.judgmenttime); // 공격 비활성화 
        weapon.Area.enabled = false;
        atkAble = true;


        yield return new WaitForSeconds(anims[0].GetCurrentAnimatorStateInfo(0).length - (animinfo.delay + animinfo.judgmenttime));

        if (atkAble)
        {
            DashAtkInput = false;//asdasdasdasdasdasdasdasd

            atkCount = 0;

            Act.Finish((int)ECharacterAct.FAttack);
            Act.Finish((int)ECharacterAct.SAttack);
        }
        yield return null;

    }
    void AttackRotationCorrection()
    {
        TargetSerchComponent box = GetComponentInChildren<TargetSerchComponent>();
        if (box)
        {
            box.SelectMainTarget();
            if (box.mainTarget)
            {
                Vector3 dir = new Vector3(box.mainTarget.transform.position.x - transform.position.x, 0, box.mainTarget.transform.position.z - transform.position.z);
                move.SetMove(dir, dir, 0);
            }
        }
    }
    #endregion
    #region Guard
    bool isGuard;
    public bool isCounter;
    const float GuardSuccessTime = 0.3f;
    void Guard()
    {
        StartCoroutine("GUARD");
    }
    IEnumerator GUARD()
    {
        isGuard = true;
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].Play("GUARD");
            anims[i].SetBool("GuardEnd", false);
        }
        Stat.isDamageable = false;

        isCounter = false;
        yield return new WaitForSeconds(GuardSuccessTime); //해당 시간안에 공격이 들어올시 카운터 어택 (이 시간은 아마 가드애니메이션 선딜시간이될듯?)
        if (isCounter)
        {
            Act.Execution((int)ECharacterAct.Counter);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        //yield return new WaitUntil(() => !isGuard);
        isGuard = false;
        for (int i = 0; i < anims.Length; i++)
            anims[i].SetBool("GuardEnd", true);
        Act.Finish((int)ECharacterAct.Guard);

        Stat.isDamageable = true;
    }
    void Counter()
    {
        Stat.atkType = PlayerStat.ECharacterAtkType.CounterAtk;

        StartCoroutine("COUNTER");
    }
    IEnumerator COUNTER()
    {
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].SetBool("GuardEnd", true);
            anims[i].Play("COUNTER");
        }
        //Managers.PrefabManager.SpawnEffect("impact_spark_block", weapon.transform.position);

        weapon.Area.enabled = true;
        yield return new WaitForSeconds(anims[0].GetCurrentAnimatorStateInfo(0).length + 0.5f); // 공격 활성화 
        weapon.Area.enabled = false;
        Stat.isDamageable = true;
        isGuard = false;

        Act.Finish((int)ECharacterAct.Counter);
    }
    #endregion
    #region Dash
    const float DashCoolTime = 3.0f;
    [SerializeField]
    float DashCount = 1;
    void Dash()
    {
        StopCoroutine("DASH");
        StartCoroutine("DASH");
    }
    IEnumerator DASH()
    {
        DashCount -= 1;
        Stat.isDamageable = false;
        // yield return new WaitForSeconds(0.2f);
        if (DashAtkInput)
        {
            Act.Execution((int)ECharacterAct.DashAttack);
            move.SetTransMove(Vector3.forward, 15, 0.25f);
        }
        else
        {
            for (int i = 0; i < anims.Length; i++)
            {
                anims[i].Play("DASH");
            }
            move.SetTransMove(Vector3.forward, 6, 0.4f);
        }

        yield return new WaitForSeconds(0.55f); //대쉬 시간
        Stat.isDamageable = true;
        Act.Finish((int)ECharacterAct.Dash);

        yield return new WaitForSeconds(DashCoolTime);
        DashCount += 1;
    }
    bool DashAtkInput;
    void DashAttack()
    {
        Stat.atkType = PlayerStat.ECharacterAtkType.DashAtk;

        StartCoroutine("DASHATTACK");
    }
    IEnumerator DASHATTACK()
    {
        //anim.Play("DASHATTACK");
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].Play("DASHATTACK");
        }

        weapon.Area.enabled = true;
        yield return new WaitForSeconds(0.3f); // 공격 활성화 
        weapon.Area.enabled = false;

        yield return new WaitForSeconds(0.7f);//애니메이션 종료
        DashAtkInput = false;
        Act.Finish((int)ECharacterAct.DashAttack);
    }
    #endregion

    void Equip()
    {
        for (int i = 0; i < equipments.Length; i++)
        {
            switch (equipments[i].type)
            {
                case Data.EEquipmentType.Head:
                    equipments[i].Equip(Managers.DataLoder.DataCache_Save.Equip.head);
                    break;
                case Data.EEquipmentType.Clothes:
                    equipments[i].Equip(Managers.DataLoder.DataCache_Save.Equip.clothes);
                    break;
                case Data.EEquipmentType.Accessory:
                    equipments[i].Equip(Managers.DataLoder.DataCache_Save.Equip.accessory);
                    break;
            }
        }
        weapon.Equip(Managers.DataLoder.DataCache_Save.Equip.weapon);

        anims = GetComponentsInChildren<Animator>(); 
    }
    void OnCharacterHit()
    {
        isCounter = true;
    }
    void OnCharacterTakeDamaged()
    {

    }
    void Dead()
    {//캐릭터 조작 입력 + 재생중인 사운드 + 캐릭터 물리 + 실행중인 애니메이션 끄고 사망 결과창 출력
        Hud.OnCharacterAction -= OnCharacterBtnEvent;
        Hud.OnCharacterEnd -= OnCharacterBtnEndEvent;
        
        move.SetMove();        

        Stat.OnUnitDead -= Dead;

        StartCoroutine(CharacterDestroy());     
    }
    IEnumerator CharacterDestroy()
    {   
        for (int i = 0; i < anims.Length; i++)
            anims[i].Play("DEAD");
        yield return new WaitForSeconds(2.0f);
        Managers.ContentsManager.Clear("",false);
       
    }

}
