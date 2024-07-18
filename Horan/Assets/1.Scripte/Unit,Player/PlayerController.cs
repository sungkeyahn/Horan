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
    Animator[]anims;
 
    HUDUI Hud;
    PlayerStat Stat;
    ActComponent Act;
    MoveComponent move;

    public Weapon weapon;
    public Equipment[] equipments = new Equipment[5];

    //에디터 키보드 마우스 조작용 컴포넌트 [개선 필요] 
    InputComponent input;
    private void Awake()
    {
        equipments = GetComponentsInChildren<Equipment>();
        weapon = GetComponentInChildren<Weapon>();
        move = GetComponent<MoveComponent>();
        Stat = GetComponent<PlayerStat>();
        input = GetComponent<InputComponent>();
        box = GetComponentInChildren<TargetSerchComponent>();
    }
    private void Start()
    {
        Sound_Hit = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Hit"].GetComponent<AudioSource>(),transform);
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
        //dash.AddAllowActID((int)ECharacterAct.Dash);
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

    void OnCharacterBtnEvent(EPlayerCharacterCtrlEvent ctrlEvent)
    {
        switch (ctrlEvent)
        {
            case EPlayerCharacterCtrlEvent.Move:
                Act.Execution((int)ECharacterAct.Move);
                break;
            case EPlayerCharacterCtrlEvent.Dash:
                if (!isDash && !isGuard)
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
    void Move()
    {
        Vector3 moveDir = new Vector3(Hud.input.x,0,Hud.input.y);
        move.SetMove(moveDir, moveDir, Stat.Speed);
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
        Stat.atkType = PlayerStat.ECharacterAtkType.FAtk;

        Data.AnimInfomation animinfo = weapon.AnimInfo_FATK[atkCount];
      
        StartCoroutine(ATTACK(animinfo));
    }
    void SAttack()
    {
        Stat.atkType = PlayerStat.ECharacterAtkType.SAtk;

        Data.AnimInfomation animinfo = weapon.AnimInfo_SATK[atkCount];        
         
        StartCoroutine(ATTACK(animinfo));
    }
    IEnumerator ATTACK(Data.AnimInfomation animinfo)
    {
        atkAble = false;
        DashAtkInput = true;
        atkCount += 1;

        AttackRotationCorrection();
        AttackContactCheck();

        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].Play(animinfo.name, -1, 0); 
        }

        yield return new WaitForSeconds(animinfo.delay); // 공격 활성화 
        atkAble = true;
        yield return new WaitForSeconds(anims[0].GetCurrentAnimatorStateInfo(0).length - animinfo.delay);
        if (atkAble)
        {
            atkCount = 0;
            DashAtkInput = false;
            Act.Finish((int)ECharacterAct.FAttack);
            Act.Finish((int)ECharacterAct.SAttack);
        }
    }
    TargetSerchComponent box;
    void AttackRotationCorrection()
    {
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
    void AttackContactCheck()
    {
        for (int i = 0; i < box.close_monsters.Count; i++)
        {
            if (box.close_monsters[i]==null)
            {
                box.close_monsters.RemoveAt(i);
                continue;
            }
            Vector3 dir = box.close_monsters[i].transform.position - transform.position;

            if (Vector3.Angle(transform.forward, dir) <= 160f / 2f)
            {
                GameObject hitob = box.close_monsters[i].gameObject;//cols[i]
                IDamageInteraction damageable = hitob.GetComponent<IDamageInteraction>();
                if (damageable != null)
                {
                    float finaldamage = Stat.Attack;

                    switch (Stat.atkType)
                    {
                        case PlayerStat.ECharacterAtkType.FAtk:
                            break;
                        case PlayerStat.ECharacterAtkType.SAtk:
                            finaldamage = finaldamage * 1.5f;
                            break;
                        case PlayerStat.ECharacterAtkType.DashAtk:
                            finaldamage = finaldamage * 2f;
                            break;
                        case PlayerStat.ECharacterAtkType.CounterAtk:
                            finaldamage = finaldamage * 2.5f;
                            break;
                    }

                    if (UnityEngine.Random.Range(0, 99) < Stat.Critical)
                        finaldamage = finaldamage * 1.5f;

                    if (damageable.TakeDamage(finaldamage))
                    {
                        Managers.PrefabManager.SpawnEffect("Hit_strong", hitob.transform.position);
                    }
                    //cols[i] = null;
                }
            }

        }

        for (int i = 0; i < box.close_objects.Count; i++)
        {
            if (box.close_objects[i] == null)
            {
                box.close_objects.RemoveAt(i);
                continue;
            }
            Vector3 dir = box.close_objects[i].transform.position - transform.position;

            if (Vector3.Angle(transform.forward, dir) <= 160f / 2f)
            {
                GameObject hitob = box.close_objects[i].gameObject;//cols[i]
                IDamageInteraction damageable = hitob.GetComponent<IDamageInteraction>();
                damageable.TakeDamage(1);
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
            Act.Finish((int)ECharacterAct.Guard);
            Act.Execution((int)ECharacterAct.Counter);
            yield break;
        }
        yield return new WaitForSeconds(1f);
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
        AttackContactCheck();
        yield return new WaitForSeconds(anims[0].GetCurrentAnimatorStateInfo(0).length + 0.5f); // 공격 활성화 
        Stat.isDamageable = true;
        isGuard = false;
        Act.Finish((int)ECharacterAct.Counter);
    }
    #endregion
    #region Dash
    bool isDash=false;
    void Dash()
    {
            StartCoroutine(DASH());
    }
    IEnumerator DASH()
    {
        isDash = true;
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

        Stat.isDamageable = false;
        yield return new WaitForSeconds(0.55f); //Anim Delay TIime 
        Stat.isDamageable = true;

        yield return new WaitForSeconds(0.45f);
        
        Act.Finish((int)ECharacterAct.Dash);

        isDash = false;
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

        AttackContactCheck();
        yield return new WaitForSeconds(0.3f); // 공격 활성화 
        
        yield return new WaitForSeconds(0.25f);//애니메이션 종료
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
                case Data.EEquipmentType.Weapon:
                    equipments[i].Equip(Managers.DataLoder.DataCache_Save.Equip.weapon);
                    break;
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
        //weapon.Equip(Managers.DataLoder.DataCache_Save.Equip.weapon);

        anims = GetComponentsInChildren<Animator>(); 
    }
    void OnCharacterHit()
    {
        isCounter = true;
    }
    void OnCharacterTakeDamaged()
    {
        Sound_Hit.Play();
    }
    void Dead()
    {
        Hud.OnCharacterAction -= OnCharacterBtnEvent;
        Hud.OnCharacterEnd -= OnCharacterBtnEndEvent;
        Stat.OnUnitDead -= Dead;

        StartCoroutine(CharacterDestroy());     
    }
    IEnumerator CharacterDestroy()
    {
        move.SetMove();
        for (int i = 0; i < anims.Length; i++)
            anims[i].Play("DEAD");
        yield return new WaitForSeconds(2.0f);
        Managers.ContentsManager.Clear("",false);
    }

}