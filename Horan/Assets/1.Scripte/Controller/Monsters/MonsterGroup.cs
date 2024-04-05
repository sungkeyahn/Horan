using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroup : MonoBehaviour,IDataBind
{

    /*
     * 게임이 시작되면 몬스터 그룹은
     * 0.자신이 관리할 구성원들을 랜덤으로 지정 
     * 1.구성원(몬스터)들의 생성
     * [생성완료 이후]
     * 2.구성원들의 상태에 맞는 행동을 명령 
     * 3.구성원의 정보를 업데이트 하여 적절한 명령을 호출
     */

    [SerializeField]
    int GroupId=1;
    
    List<MonsterController> MyGroup=new List<MonsterController>();

    public BTRunner Runner { get; protected set; }

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
                Object.Instantiate(prefab, transform);
                MonsterController addedMonster = prefab.GetComponent<MonsterController>();
                addedMonster.MyName = MonsterName;
                MyGroup.Add(addedMonster);
            }

        }
    } 
    private void Start()
    {
        BindData();

        // 몬스터 진형 위치 설정
        // addedMonster.transform.position;

        /*  BT구성
         *  3마리 기준으로
         *
         *  동작을 실행하기로 결정된 MonsterController에 대하여 명령을 내리는 테스크 
         *  
         *  행동을 할 친구를 결정 하는 테스크(만약 구성원중 1명이 동작을 수행중인 경우 해당 수행자를 제외하고 다른 구성원에게 동작을 지정하도록 하기)  
         *  
         *  몬스터 컨트롤러에서 공격 void Attack()만들어 놓고 몬스터마다 상속시켜서 따로 구현
         *  공격시 적을 향해회전 같은 연속된 테스크 같은 경우는 
         *  
         *  결국 그룹에 구성원들은 update에서 항상 행동들을 실행하고 있을것이다 
         *  어떤 행동을 누가 할지만 BT에서 지정해서 호출하는것 
         *  그리고 구성원의 행동이나 상태가 변화할 때 그룹에 알려주어야한다.
         *  
         *  문제점 : 몬스터의 행동은 해당 몬스터의 컨트롤러에 구현되어 있다 
         *  테스크를 통하여 어떤 메서드를 호출할지를 결정해야 한다.
         *  
         *  
         */

        Runner = new BTRunner
            (
                new BT_Selector
                (
                    new List<BT_Node>()
                    {
                        new BT_Task(RotateToAtkTarget)
                    }
                )
            );


    }
    private void Update()
    {
        if (Runner != null)
        {
            Runner.Operate();
            Runner.ServiceOperate();
        }
    }
    protected void StopUnit(bool isStop)
    {
        Runner.isActive = !isStop;
        //Nav.isStopped = isStop;
    }

    BT_Node.NodeState RotateToAtkTarget()
    {
        return BT_Node.NodeState.Success;
    }
    /*
     *  
     *  #region TaskFuns
    
    BT_Node.NodeState RotateToAtkTarget()
    {
        GameObject gameObject = FindCloseUnit(1 << 9);
        if (gameObject)
        {
            Vector3 dir = gameObject.transform.position - transform.position;
            if (Vector3.Angle(transform.forward, dir) <= 45f / 2f) //몬스터 시야각 
            {
                return BT_Node.NodeState.Success;
            }
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 5*Time.deltaTime);
            return BT_Node.NodeState.Running;
        }
        return BT_Node.NodeState.Failure;
    }
    
    BT_Node.NodeState CheckAtkAnimPlaying()
    {
        if (IsAnimationRunning("ATTACK"))
        {
            return BT_Node.NodeState.Running;
        }

        return BT_Node.NodeState.Success;
    }

    BT_Node.NodeState Attack()
    {
        if (TargetPlayer != null)
        {       
            Anim.Play("ATTACK");
            return BT_Node.NodeState.Success;
        }   
        else
            return BT_Node.NodeState.Failure;
    }


    #endregion



 
     */

}
