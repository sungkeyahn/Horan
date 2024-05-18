using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroup : MonoBehaviour, IDataBind
{
    /*    [SerializeField]
    int GroupId=1;

    public BTRunner Runner { get; protected set; }

    List<MonsterController> MyGroup = new List<MonsterController>();

    [SerializeField]
    MonsterController ActableCtrl; // 현재 행동을 실행할 구성원

    [SerializeField]
    PlayerController player;
    Vector3 Destpos = Vector3.zero;

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
                                new BT_Decorator(new BT_Selector // 해당 유닛이 전투 상태 인지 체크[미구현]  
                                (
                                    new List<BT_Node>()
                                    {
                                        new BT_Decorator(new BT_Task(Attack),IsInAtkRange),
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
        ActableCtrl.Wandering(Destpos);
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Attack()
    {
        ActableCtrl.Attack(player.gameObject);
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState Chase()
    {
        ActableCtrl.Chase(player.gameObject);
        return BT_Node.NodeState.Success;
    }
    BT_Node.NodeState CombatWait()
    {
        ActableCtrl.CombatWait(player.gameObject);
        return BT_Node.NodeState.Success;
    }
   
    #endregion

    #region Service And Deco
    int ctrlnum;
    bool SelectMember() 
    {
        int count = 0;
        for (int i = 0; i < MyGroup.Count; i++) //그룹 구성원 사망 체크
        {
            if (MyGroup[i]==null)
                count++;
            if (MyGroup.Count == count) //구성원이 모두 사망시 BT정지
            {
                Runner.isActive = false;
                return false;
            }
        }
        
        if (MyGroup.Count-1 < ctrlnum)
            ctrlnum = 0;

        if (MyGroup[ctrlnum] != null)
        {
            ActableCtrl = MyGroup[ctrlnum];
            ctrlnum++;
            return true;
        }
        else
        {
            ctrlnum++;
            return false;
        }
    }
    bool SearchPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(ActableCtrl.transform.position, ActableCtrl.Stat.sensingrange, LayerMask.GetMask("Player"));
        if (cols.Length != 0)
        {
            player = cols[0].GetComponent<PlayerController>();
            if (player) //set target
                return true;
        }
        else if (player) //other member set target
            return true;
       

        // search fail
        player = null;  
        return false;
    }
    bool IsCombat()
    {
        if (player.TargetEnemy == ActableCtrl.gameObject) 
            return true;
       
        return false;
    }
    bool IsInAtkRange()
    {
        if(ActableCtrl == null) return false;
        if (Vector3.Distance(ActableCtrl.transform.position, player.transform.position) <= ActableCtrl.Stat.atkrange)
            return true;
       
        return false;
    }
    bool RandomSelectAtkorGuard() //[미구현]
    {
        return true;
    }
    void SetWanderingDestination()
    {
        Destpos = transform.position + new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
    }

    #endregion*/
    public void BindData()
    {
    }
}
