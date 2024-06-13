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
    Animator []anims;
    HUDUI Hud;
    PlayerStat Stat;
    ActComponent Act;

    Vector3 moveDir = Vector3.zero;
    public GameObject TargetEnemy = null;
    public Weapon weapon;
    public Equipment[] equipments = new Equipment[4];

    GameObject StepSound1;
    GameObject StepSound2;
    GameObject SwingSound1;
    GameObject SwingSound2;


    private void Awake()
    {
        equipments = GetComponentsInChildren<Equipment>();
        weapon = GetComponentInChildren<Weapon>();

        input = GetComponent<InputComponent>();
        input.MouseAction -= OnPlayerMouseEvent;
        input.MouseAction += OnPlayerMouseEvent;
        input.KeyAction -= OnPlayerKeyBoardEvent;
        input.KeyAction += OnPlayerKeyBoardEvent;

        move = GetComponent<MoveComponent>();
        Stat = GetComponent<PlayerStat>();
    }
    private void Start()
    {
        Stat.StatInit(Managers.ContentsManager.level, Managers.ContentsManager.exp, Managers.ContentsManager.hp);
        Stat.OnHit += OnCharacterHit;
        Stat.OnUnitTakeDamaged += OnCharacterTakeDamaged;

        #region Effect + Sound
        StepSound1 = Managers.PrefabManager.PrefabInstance("Step1",transform);
        StepSound2 = Managers.PrefabManager.PrefabInstance("Step2", transform);
        SwingSound1 = Managers.PrefabManager.PrefabInstance("Swing1", transform);
        SwingSound2 = Managers.PrefabManager.PrefabInstance("Swing2", transform);
        //impact_spark_block
        #endregion

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
        #endregion

        Hud = Managers.UIManager.ShowSceneUI<HUDUI>();
        Hud.Init();
        //Managers.ContentsManager.AbilityContainer.OnAbilityUpdate += Hud.UpdateAbilityIcon;

        Equip();
        Managers.ContentsManager.AbilityContainer.ApplyAllAbility(Stat);
    }
    private void FixedUpdate()
    {
        if (TargetEnemy == null)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 10, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < cols.Length; i++)
            {
                if (TargetEnemy == null)
                    TargetEnemy = cols[i].gameObject;
            }
        }
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
                    if (atkAble && atkCount < weapon.AnimInfo.Count)
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
                    move.SetMove(0);
                    //anim.SetInteger("AnimState", (int)EPlayerAnimState.IDLE);
                    for (int i = 0; i < anims.Length; i++)
                    {
                        anims[i].SetInteger("AnimState", (int)EPlayerAnimState.IDLE);
                    }
                    StepSound1.GetComponent<AudioSource>().Stop();
                    Act.Finish((int)KindOfAct.Move);
                }
                break;
            case InputComponent.KeyBoardEvent.Press:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    Act.Execution((int)KindOfAct.Move);
                }
                if (Input.GetKey(KeyCode.E) && !isGuard)
                {
                    Act.Execution((int)KindOfAct.Guard);
                }
                break;
            case InputComponent.KeyBoardEvent.ButtonDown:
                if (Input.GetKey(KeyCode.Space) && 0 < DashCount && Stat.UseSP(20))
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
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");

        move.SetMove(moveDir, moveDir, Stat.Speed);
        
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].SetInteger("AnimState", (int)EPlayerAnimState.MOVE);
            anims[i].SetFloat("WalkX", moveDir.x);
            anims[i].SetFloat("WalkY", moveDir.z);
        }
        Managers.PrefabManager.PlaySound(StepSound1);
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
        Data.AnimInfomation animinfo = weapon.AnimInfo[atkCount];
        atkCount += 1;

        for (int i = 0; i < anims.Length; i++)
            anims[i].Play(animinfo.name, -1, 0);//anim.Play(animinfo.name, -1, 0);

        if (UnityEngine.Random.Range(0, 2) == 0)
            Managers.PrefabManager.PlaySound(SwingSound1);
        else
            Managers.PrefabManager.PlaySound(SwingSound2);
        
        atkAble = false;
        yield return new WaitForSeconds(animinfo.delay); // 공격 활성화 
        weapon.Area.enabled = true;

        yield return new WaitForSeconds(animinfo.judgmenttime); // 공격 비활성화 
        weapon.Area.enabled = false;
        atkAble = true;
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length - (animinfo.delay + animinfo.judgmenttime));
        yield return new WaitForSeconds(anims[0].GetCurrentAnimatorStateInfo(0).length - (animinfo.delay + animinfo.judgmenttime));

        atkCount = 0;
        Act.Finish((int)KindOfAct.Attack);
        yield return null;

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
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].Play("GUARD");
            anims[i].SetBool("GuardEnd", false);
        }
        //anim.Play("GUARD");
        //anim.SetBool("GuardEnd", false);

        Stat.isDamageable = false;

        isCounter = false;
        yield return new WaitForSeconds(GuardSuccessTime); //해당 시간안에 공격이 들어올시 카운터 어택 (이 시간은 아마 가드애니메이션 선딜시간이될듯?)
        if (isCounter)
        {
            Act.Execution((int)KindOfAct.Counter);
            yield return null;
        }

        yield return new WaitUntil(() => !isGuard);
        isGuard = false;
        //anim.SetBool("GuardEnd", true);
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].SetBool("GuardEnd", true);
        }
        Stat.isDamageable = true;
    }

    void Counter()
    {
        StartCoroutine("COUNTER");
    }
    IEnumerator COUNTER()
    {
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].SetBool("GuardEnd", true);
            anims[i].Play("COUNTER");
        }
        Managers.PrefabManager.SpawnEffect("impact_spark_block", weapon.transform.position);

        weapon.Area.enabled = true;
        yield return new WaitForSeconds(anims[0].GetCurrentAnimatorStateInfo(0).length + 1); // 공격 활성화 
        
        weapon.Area.enabled = false;
        Stat.isDamageable = true;
        isGuard = false;

        Act.Finish((int)KindOfAct.Counter);
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

        move.SetTransMove(Vector3.forward, 6, 0.4f);

        //anim.Play("DASH");
        for (int i = 0; i < anims.Length;i++)
        {
            anims[i].Play("DASH");
        }

        DashAtkInput = false;
        yield return new WaitForSeconds(0.1f);
        if (DashAtkInput)
        {
            Act.Execution((int)KindOfAct.DashAttack);
            yield return null;
        }


        yield return new WaitForSeconds(0.2f); //대쉬 시간
        Stat.isDamageable = true;
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
        //anim.Play("DASHATTACK");
        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].Play("DASHATTACK");
        }
        weapon.Area.enabled = true;
        yield return new WaitForSeconds(0.3f); // 공격 활성화 
        weapon.Area.enabled = false;

        yield return new WaitForSeconds(0.7f);//애니메이션 종료
        Act.Finish((int)KindOfAct.DashAttack);
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
}
