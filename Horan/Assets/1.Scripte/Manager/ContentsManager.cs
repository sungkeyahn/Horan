using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameModes { StoryMode, DefanseMode, TutorialMode }
public class ContentsManager 
{
    /*  매니저의 기능
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

    public void StageStart(EGameModes gameMode,int stage=1) //로비에서 선택용
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
    }
    public void StageClear()
    {//스테이지 클리어 시 호출
        Debug.Log("StageClear");
        //GamePuase();

        //Managers.UIManager.ShowPopupUI<ClearUI>(); -> ClearUI

        //AddedPlayData(curStageindex); -> DataSave();와 구현 구분 하기 
        //몇 스테이지를 클리어 했는가? , 어떤 장비를 얻었는가? , 숙련도?  

        //PlayerCharacterInfo
    }
    public void StageEnd()
    {
        //Application.Quit();   
    }
    #endregion

    #region Wave

    public Action OnWaveClear; //ScneScript ref 
    int waveMonsterCounts;
    int WaveMonsterCounts
    {
        get { return waveMonsterCounts; }
        set { waveMonsterCounts = value; if (waveMonsterCounts == 0) if (OnWaveClear != null) OnWaveClear.Invoke(); }
    }
    public void WaveStart() 
    {
     //웨이브 시작 시 호출
     //몬스터 카운트 등의 데이터 초기화 + 몬스터 생성보다 먼저 호출 되어야 함  

        
    }
    public void WaveClear()
    {
     //웨이브 클리어 시 호출
     //데이터 삭제 및 전달? 

     
    }
    #endregion
    
    //게임 시작 시 해당 자료구조를 비워야하고 게임종료시 해당 데이터를 넘겨야함 
    public Dictionary<int,int> AcquiredItems = new Dictionary<int, int>(); // 획득한 아이템  id , 수량 

    bool TryItemDrop(string id) //몬스터 키값이 string 형임 추후 수정 필요
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
                    if (AcquiredItems.ContainsKey(data.dropitems[i].id))
                        AcquiredItems[data.dropitems[i].id] += data.dropitems[i].amount;
                    else
                        AcquiredItems.Add(data.dropitems[i].id, data.dropitems[i].amount);

                    DropNothing = false;
                    Debug.Log(String.Format($"{0} {1}개 습득", data.dropitems[i].id, data.dropitems[i].amount));
                }
            }
        }
        return !DropNothing;
    }


    #region Unit
    Dictionary<string, UnitController> SpawnedUnits=new Dictionary<string, UnitController>();
    public void SpawnUnit(string id)
    {
        WaveMonsterCounts += 1;
    }
    public void DeadUnit(string id)
    {
        if (TryItemDrop(id))
        {
            //Spawn Drop Effects  + PickupItem Objects
        }
        //드랍 골드 
        //드랍 경험치 
        

        WaveMonsterCounts -= 1;
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