using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsManager 
{
    /*  매니저 기능
     * 스토리모드 : 게임 시작, 게임 종료 , 웨이브 시작 , 웨이브 종료 ,에 해당하는 함수를 ContentsManager에 구현(호출은 각 Scene에서)
     * 
     * 연관 된 부분들)
     * 로비(게임시작버튼) -> 레벨선택UI -> 레벨 선택 -> 씬 이동
     * 씬 스크립트에서 스테이지 시작 함수 호출
     * 몬스터 사망시 스테이지 클리어 여부 체크
     * 잠재능력 선택시 잠재능력 정보 저장
     * 
     * 현재 스테이지 에 스폰된 몬스터들
     * 스테이지 클리어 조건 
     * 획득아이템들 
     * 획득 잠재능력
     * 
     * 
     * 이벤트로 다 뿌려주는 역할을 해야할거같은데?
     * 
     * 
     * 지금 할일)
     * 1. 몬스터 사망시 채력 + 아이템 + 재화 + 경험치 드랍 시스템 
     * 2. 캐릭터 레벨 업 시 잠재능력 선택 UI 띄우기 + 게임 퍼즈
     * 3. 각 잠재능력 UI에서 선택한 능력을 적용 
     * 4. 적용된 잠재능력은 컨텐츠매니저에서 정보 저장
     * 5. 맵 이동(웨이브 시작)시 장비+잠재+스텟 능력치 계승 
     * 
     */
    public Action OnStageClear;
    public Action OnWaveClear;
    public Action OnWaveStart;
    public List<int> AcquiredItems = new List<int>(); //획득한 아이템들 
    public LatentAbilityContainer AbilityContainer = new LatentAbilityContainer(); //획득한 잠재능력들\
    public int MonsterCounts; //현재 남아있는 몬스터 수

    public PlayerController player;
    public PlayerStat stat;

    public int level=1;
    public float exp=-1;
    public float hp=-1;


    public void StageSelect(int stage=1) // LobbyUI에서 호출
    {
        AcquiredItems.Clear();
        AbilityContainer.ClearAbilities();
        Managers.MySceneManager.LoadScene($"Level{stage}");
    }
    public void StageClear() // Scene Scripte애서 호출
    {
        AcquiredItems.Clear();
        AbilityContainer.ClearAbilities();
    }
    public void WaveStart() // Scene Scripte애서 호출 
    {   
        if (OnWaveStart!=null)
            OnWaveStart.Invoke();

        stat.OnStatChanged -= LevelUP;
        stat.OnStatChanged += LevelUP;
    }
        
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

        if (UnityEngine.Random.Range(0, 9)<3)
            Managers.DataLoder.DataCache_Save.User.gold += data.dropgold;

        stat.Exp += data.dropexp;

        return !DropNothing;
    }

    public void SpawnUnit(string id)
    {
        MonsterCounts += 1;
    }
    public void DeadUnit(string id)
    {
        if (TryItemDrop(id))
        {
            Debug.Log("ItemDrop");
            //Spawn Drop Effects  + PickupItem Objects
        }


        MonsterCounts -= 1;
        if (MonsterCounts <= 0)
        {
            if (OnWaveClear != null)
                OnWaveClear.Invoke();
            OnWaveStart = null;
            MonsterCounts = 0;
        }

        Debug.Log($"Monster Count : { MonsterCounts }");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Debug.Log("Pause");
    }
    public void Resume()
    {
        Time.timeScale = 1;
        Debug.Log("Resume");
    }
    void LevelUP(StatIdentifier identifier, float pre, float cur)
    {
        if (identifier == StatIdentifier.Level)
        {
            Managers.ContentsManager.Pause();
            Managers.UIManager.ShowPopupUI<SelectAbilityUI>("SelectAbilityUI").Init(
                UnityEngine.Random.Range(1, Managers.DataLoder.DataCache_LatentAbility.Count),
                UnityEngine.Random.Range(1, Managers.DataLoder.DataCache_LatentAbility.Count),
                UnityEngine.Random.Range(1, Managers.DataLoder.DataCache_LatentAbility.Count), stat);
        }
    }


}
