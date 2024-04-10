using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  BT구성
 *  그룹의 정의를 일정 거리내에서 같이 행동하는 군집 느낌
 *  서로의 행동이 서로에게 영향을 주는 형태의 몬스터 군집 느낌 
 *  따라서 행동이 완전히 다른 그런 친구들을 하나의 BT로 묶을 생각 안해도 됨
 *
 *  동작을 실행하기로 결정된 MonsterController에 대하여 명령을 내리는 테스크 
 *  
 *  행동을 할 친구를 결정 하는 테스크(만약 구성원중 1명이 동작을 수행중인 경우 해당 수행자를 제외하고 다른 구성원에게 동작을 지정하도록 하기)  
 *  
 *  몬스터 컨트롤러에서 공격 void Attack()만들어 놓고 몬스터마다 상속시켜서 따로 구현
 *  공격시 적을 향해회전 같은 연속된 테스크 같은 경우는 
 *  
 *  몬스터 그룹도 여러개 만들어야 할듯 ?
 *  지금 구현 하는것은 근접 일반 몬스터 기준으로 BT 작성 
 *  
 */
public class MonsterGroup : MonoBehaviour,IDataBind
{
    [SerializeField]
    int GroupId=1;

    public BTRunner Runner { get; protected set; }

    List<MonsterController> MyGroup=new List<MonsterController>();
    [SerializeField]
    MonsterController ActableCtrl; // 현재 행동을 실행할 구성원
    [SerializeField]
    MonsterController CombatUnit; // 현재 전투중인 구성원
    [SerializeField]
    bool GroupCombat; // 전투 여부

    public void BindData()
    {
        string MonsterName;
        if (!Managers.DataLoder.DataCache_Groups.ContainsKey(GroupId)) return;

        for (int i = 0; i < Managers.DataLoder.DataCache_Groups[GroupId].member.Count; i++)
        {
            MonsterName= Managers.DataLoder.DataCache_Groups[GroupId].member[i].name;
            if (Managers.DataLoder.DataCache_Monsters.ContainsKey(MonsterName))
            {
                GameObject prefab = Resources.Load<GameObject>($"Monster/{MonsterName}");
                GameObject go = Object.Instantiate(prefab, transform);

                MonsterController addedMonster = go.GetComponent<MonsterController>();
                addedMonster.MyName = MonsterName;
                MyGroup.Add(addedMonster);

            }
        }
    } 
    private void Start()
    {
        BindData();

        //삼각형태 위치 구성
        Vector3 center = transform.position;
        float angleIncrement = 360f / 3; // 각도 
        for (int i = 0; i < MyGroup.Count; i++)
        {
            float angle = i * angleIncrement; 
            float x = center.x + 1 * Mathf.Cos(Mathf.Deg2Rad * angle);
            float z = center.z + 1 * Mathf.Sin(Mathf.Deg2Rad * angle);
            MyGroup[i].transform.position = new Vector3(x, center.y, z); 
        }

        #region BT
        Runner = new BTRunner
            (
               new BT_Decorator(new BT_Service(new BT_Selector
                (
                    new List<BT_Node>()
                    {
                        new BT_Decorator(new BT_Sequence //전투 대기
                        (
                            new List<BT_Node>()
                            {
                                new BT_Task(CombatWait)
                            }
                        ),IsGroupCombat),
                        new BT_Decorator(new BT_Selector //공,방
                        (
                           new List<BT_Node>()
                           {
                                new BT_Decorator(new BT_Selector //공격, 방어
                                (
                                    new List<BT_Node>()
                                    {
                                        new BT_Decorator(new BT_Task(Attack),RandomSelectAtkorGuard),
                                        new BT_Task(Guard) 
                                    }
                                ),IsCombatable)
                           }
                        ),IsPlayerInAtkRange),
                        new BT_Decorator(new BT_Sequence // 탐지 거리 안에 존재하는 경우
                        (
                            new List<BT_Node>()
                            {
                                new BT_Task(Chase)
                            }
                        ),IsPlayerInSenseRange),
                        new BT_Sequence //순찰
                        (
                            new List<BT_Node>()
                            {
                                new BT_Task(Wandering)
                            }
                        )
                    }
                ), this, SetWanderingDestination, 7),SelectMember)
            );
        #endregion

    }
    private void Update()
    {
        if (Runner != null)
        {
            Runner.Operate();
            Runner.ServiceOperate();
        }
    }

    #region Task
    BT_Node.NodeState Wandering()
    {
        if (ActableCtrl.isActing)
            return BT_Node.NodeState.Running;

        ActableCtrl.Wandering(Runner);

        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Attack()
    {
        if (ActableCtrl.isActing)
            return BT_Node.NodeState.Running;

        ActableCtrl.Attack(Runner);

        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Chase()
    {
        if (ActableCtrl.isActing)
            return BT_Node.NodeState.Running;

        ActableCtrl.Chase(Runner);

        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState CombatWait()
    {
        if (ActableCtrl.isActing)
            return BT_Node.NodeState.Running;

        ActableCtrl.CombatWait(Runner);

        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Guard()
    {
        Debug.Log("Guard");
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Exhaustion()
    {
        Debug.Log("Exhaustion");
        return BT_Node.NodeState.Success;
    }
    #endregion
    #region Service And Deco
    void SetWanderingDestination()
    {
        Vector3 pos = transform.position + new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
        for (int i = 0; i < MyGroup.Count; i++)
        {
            MyGroup[i].DestPos = pos;
        }
    }

    bool IsPlayerInAtkRange()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null|| ActableCtrl==null) return false;

        if (Vector3.Distance(ActableCtrl.transform.position ,player.transform.position) <= ActableCtrl.Stat.atkrange)
        {
            CombatUnit = ActableCtrl;
            GroupCombat = true;
            return true;
        }
        else
            return false;
    }
    bool IsPlayerInSenseRange()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return false;

        // 1명만 감지해도 타겟 on 하지만 
        // 모두 감지 못하면 타겟 off
        bool isSensing = false;
        for (int i = 0; i < MyGroup.Count; i++)
        {
            if (Vector3.Distance(MyGroup[i].transform.position, player.transform.position) <= MyGroup[i].Stat.sensingrange)
                isSensing = true;
        }

        if (isSensing)
        {
            for (int i = 0; i < MyGroup.Count; i++)
            {
                MyGroup[i].Target = player;
            }
            return true;
        }
 
        for (int i = 0; i < MyGroup.Count; i++)
        {
            MyGroup[i].Target = null;
        }
        CombatUnit = null;
        GroupCombat = false;
        return false;
    }
    bool SelectMember() // 행동을 수행중이지 않은 멤버를 선택
    {
        int count = 0;
        for (int i = 0; i < MyGroup.Count; i++) //그룹 구성원 사망했을 경우 그룹 리셋 
        {
            if (!MyGroup[i].gameObject.activeSelf && MyGroup[i] == CombatUnit) 
            {
                count++;
                CombatUnit = null;
                GroupCombat = false;
            }
        }
        if (MyGroup.Count == count)
        {
            Runner.isActive = false;
            return false;
        }

        for (int i = 0; i < MyGroup.Count; i++)
        {
            if (MyGroup[i].isActing == false)
            {
                ActableCtrl = MyGroup[i];
                return true;
            }
        }

        ActableCtrl = null;

        return false;
    }
    bool RandomSelectAtkorGuard() // 공격or방어 랜덤 선택
    {
        return true;
    }
    bool IsCombatable() //전투가능 상황 인지 판단 
    {
        if (ActableCtrl.Target == null) return false; // 플레이어 미 인지 상황  

        if (IsGroupCombat()) return false;

        if (CombatUnit == ActableCtrl)
            return true;
        
        return false;
    }
    bool IsGroupCombat() 
    {
        if (GroupCombat && ActableCtrl != CombatUnit )
            return true;
        else
            return false;
    }
    #endregion



}
