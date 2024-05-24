using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameModes { StoryMode, DefanseMode, TutorialMode }
public class ContentsManager 
{
    /*  매니저 기능
     * 각 모드마다 인게임 내용에 따라 변동되어 저장될 데이터들을 수집하여 게임이 종료될때 세이브파일의 내용과 덪씌워주는 역할
     * + 각 게임모드의 시작 종료시 처리되어야하는 동작들을 메서드로 구현
     * 
     * 스토리모드 : 게임 시작, 게임 종료 , 웨이브 시작 , 웨이브 종료 ,에 해당하는 함수를 ContentsManager에 구현(호출은 각 Scene에서)
     * 
     * 예시 동작 흐름)
     * 유저가 스토리 모드를 선택 -> 스테이지를 클릭 -> 씬 이동 -> 각 씬스크립트에서 게임시작 함수 호출 -> 스테이지 진행
     * (각 씬의 시작과 다음씬으로 넘어가는 부분은 씬에 존재하는  씬스크립트에서 담당)
     * 하지만 씬이 변경되더라도 인게임 정보를 가지고 넘어갈수 있도록 컨텐츠 매니저에서 데이터 관리
     * 
     * 
     * 주요 기능 
     * 1. 스테이지 시작 및 종료 시점에 호출할 함수 정의 
     * 2. 게임 중 얻은 데이터 관리 및 전달
     * 
     * 지금 할일 
     * 아이템 구현 -> 아이템 드랍 -> 아이템 습득 -> 콘텐츠 매니저에 저장 -> 
     * 획득 아이템 아이콘 및 데이터 넣기 
     * 획득 아이템 저장해서 로비로 넘기기
     * 
     * 로비에서 볼 아이템 인벤토리 구현
     * 
     */ 

    #region Stage
    public Action OnStageClear;
    int curStageindex = 0;
    public void StageSelect(EGameModes gameMode,int stage=1) //로비에서 선택용
    {
        switch (gameMode)
        {
            case EGameModes.StoryMode:
                curStageindex = stage;
                break;
            case EGameModes.DefanseMode:
                break;
            case EGameModes.TutorialMode:
                break;
        }
        AcquiredItems.Clear();
        Managers.MySceneManager.LoadScene($"Level{stage}");
    }
    public void StageClear()
    {
        //스테이지 클리어시 저장되어야할 데이터 종류 -> 인벤아이템 + 유저 경험치(숙련도) + 재화
        //GamePuase();
        Managers.MySceneManager.LoadScene("Lobby");
    }
    #endregion

    #region Wave
    public Action OnWaveClear;
    public int MonsterCounts;
    public void WaveStart()
    {
        AbilityContainer.Init();
        //Managers.ContentsManager.AbilityContainer.AddAbility(new LatentAbility(2, playerStat)); 어빌리티 획득시 그쪽에서 호출할 코드 
        //어빌리티 획득 잘되는지 확인하기 
        //AbilityContainer.ClearAbilities();
    }
    void WaveClear() //모든 몬스터 사망시 자동 호출
    {
        if (OnWaveClear != null)
            OnWaveClear.Invoke();
        MonsterCounts = 0;
    }
    #endregion

    #region ItemDrop ItemAcquired
    public List<int> AcquiredItems = new List<int>();
    bool TryItemDrop(string id) 
    {
        Data.DataSet_Monster data;
        Managers.DataLoder.DataCache_Monsters.TryGetValue(id,out data);
        if (data == null) return false;

        bool DropNothing = true;
        for (int i = 0; i < data.dropitems.Count; i++)
        {
            int rand = UnityEngine.Random.Range(0, 99);
            if (rand < data.dropitems[i].probability)
            {
                Data.DataSet_Item itemdata;
                Managers.DataLoder.DataCache_Items.TryGetValue(data.dropitems[i].id, out itemdata);
                if (itemdata != null) 
                {
                    int emptyinedx = int.MaxValue;
                    for (int j = 0; j < 25; j++) //인벤 정보 
                    {
                        if (Managers.DataLoder.DataCache_Save.Inventory[j].id == 0 && j < emptyinedx)
                            emptyinedx = j;

                        if (Managers.DataLoder.DataCache_Save.Inventory[j].id == itemdata.id)
                        {
                            Managers.DataLoder.DataCache_Save.Inventory[j].amount += data.dropitems[i].amount;
                            DropNothing = false;
                        }
                    }
                    if (emptyinedx<25 && DropNothing)
                    {
                        Managers.DataLoder.DataCache_Save.Inventory[emptyinedx].id = itemdata.id;
                        Managers.DataLoder.DataCache_Save.Inventory[emptyinedx].amount = data.dropitems[i].amount;

                        AcquiredItems.Add(itemdata.id); //수정 필요
                        DropNothing = false;
                    }
                }
            }
        }
        return !DropNothing;
    }
    #endregion

    #region LatentAbility
    public LatentAbilityContainer AbilityContainer = new LatentAbilityContainer();
    #endregion

    #region Unit
    Dictionary<string, UnitController> SpawnedUnits=new Dictionary<string, UnitController>();
    public void SpawnUnit(string id)
    {
        MonsterCounts += 1;
        //Debug.Log(MonsterCounts);
    }
    public void DeadUnit(string id)
    {
        if (TryItemDrop(id))
        {
            //Spawn Drop Effects  + PickupItem Objects
        }
        //드랍 골드 
        //드랍 경험치 

        MonsterCounts -= 1;
        Debug.Log($"DeadCount{ MonsterCounts }");
        if (MonsterCounts <= 0)
            WaveClear();
    }
    #endregion

    #region InGameControl
    bool isPause;
    public void GamePause()
    { }
    public void GameResume()
    { }
    #endregion

}

/*  GameObject prefab = Resources.Load<GameObject>($"DropItem/{name}");
                if (prefab)
                {
                    GameObject ob = Instantiate(prefab);
                    ob.transform.position = transform.position + Vector3.up * 3;


                    //ob.GetComponent<PickupItem>().SetItem(name, amount);
                }*/