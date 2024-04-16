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


    GameObject player;


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
               new BT_Decorator(new BT_Selector
                (
                    new List<BT_Node>()
                    {
                       new BT_Decorator(new BT_Selector //플레이어 인식 
                        (
                            new List<BT_Node>()
                            { 
                                new BT_Decorator( new BT_Selector // 해당 유닛이 전투 상태 인지 체크[미구현]  
                                (
                                    new List<BT_Node>()
                                    {
                                        new BT_Decorator(new BT_Selector //전투 중인 유닛이며 공격거리에 존재하면 공격
                                        (
                                           new List<BT_Node>()
                                           {
                                               new BT_Decorator(new BT_Task(Attack),RandomSelectAtkorGuard),
                                               new BT_Task(Guard)
                                           }
                                        ),IsInAtkRange),
                                        new BT_Task(Chase) //전투 유닛 이지만 공격 거리에 없으면 추격
                                    }
                                ),IsCombat),
                                new BT_Task(CombatWait) //전투 중인 유닛이 아니면 대기
                            }
                        ),SearchPlayer),
                        new BT_Service(new BT_Task(Wandering),this,SetWanderingDestination,6f)
                    }
                ), SelectMember)
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
    bool SelectMember() // 행동을 수행중이지 않은 멤버를 선택
    {
        int count = 0;
        for (int i = 0; i < MyGroup.Count; i++) //그룹 구성원 사망했을 경우 리셋 
        {
            if (!MyGroup[i].gameObject.activeSelf)
            {
                count++;
               // CombatUnit = null;
            }
        }

        if (MyGroup.Count == count) //구성원이 모두 사망시 BT정지
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
    bool SearchPlayer()
    {
        GameObject ob=null;
        for (int i = 0; i < MyGroup.Count; i++)
        {
            Collider[] cols = Physics.OverlapSphere(MyGroup[i].transform.position, MyGroup[i].Stat.sensingrange, LayerMask.GetMask("Player"));
            if (cols != null)
            {
                ob= cols[0].gameObject;
                break;
            }
        }

        for (int i = 0; i < MyGroup.Count; i++)
            MyGroup[i].Target = ob;
        if (ob)
            return true;

        return false;
    }
    bool IsCombat() //나의 전투참여 가능 여부 
    {
        PlayerController p = player.GetComponent<PlayerController>();
        if (ActableCtrl.isCombat && p.TargetEnemy == ActableCtrl.gameObject) return true;
        
        //모든 구성원이 전투 중이 아님 + 플레이어가 전투 타겟이 없는 경우 
        if (p.TargetEnemy == null && !ActableCtrl.isCombat)
        {
            p.TargetEnemy = ActableCtrl.gameObject;
            ActableCtrl.isCombat = true;
            return true;
        }
       
        return false;
    }
    bool IsInAtkRange()
    {
        if(ActableCtrl == null) return false;
        if (Vector3.Distance(ActableCtrl.transform.position, player.transform.position) <= ActableCtrl.Stat.atkrange)
            return true;
        return false;
    }
    bool RandomSelectAtkorGuard() // 공격or방어 랜덤 선택
    {
        return true;
    }
    void SetWanderingDestination()
    {
        Vector3 pos = transform.position + new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
        for (int i = 0; i < MyGroup.Count; i++)
        {
            MyGroup[i].DestPos = pos;
        }
    }
    #endregion



}
