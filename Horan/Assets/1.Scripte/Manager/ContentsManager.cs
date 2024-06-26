using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsManager 
{
    /*  매니저 기능
     * 1. 몬스터 사망시 채력 + 아이템 + 재화 + 경험치 드랍 시스템 
     * 2. 캐릭터 레벨 업 시 잠재능력 선택 UI 띄우기 + 게임 퍼즈
     * 3. 각 잠재능력 UI에서 선택한 능력을 적용 
     * 4. 적용된 잠재능력은 컨텐츠매니저에서 정보 저장
     * 5. 맵 이동(웨이브 시작)시 장비+잠재+스텟 능력치 전달
     */
    public Action OnStageClear;
    public Action OnWaveClear;
    public Action OnWaveStart;
    public Action OnMonsterSpawn;
    public Action OnMonsterDead;

    public Dictionary<int ,int> AcquiredItems = new Dictionary<int, int>(); //획득한 아이템들 
    public int dropgold; //획득한 골드 
    public int killcount; //스테이지 전체 처치한 적 수 

    public LatentAbilityContainer AbilityContainer = new LatentAbilityContainer(); //획득한 잠재능력들
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
        Managers.MySceneManager.LoadScene($"Level {stage}");
    }
    public void WaveStart() // Scene Scripte애서 호출 
    {   
        if (OnWaveStart!=null)
            OnWaveStart.Invoke();

        stat.OnStatChanged -= LevelUP;
        stat.OnStatChanged += LevelUP;
    }
    public void Clear(string NextSceneName,bool isWin=true)
    {
        MonsterCounts = 0;
        if (string.IsNullOrEmpty(NextSceneName))  Clear(isWin);
        else
        {
            Managers.ContentsManager.AbilityContainer.OnAbilityUpdate = null;
            level = stat.Level;
            exp = stat.Exp;
            hp = stat.Hp;
            Managers.MySceneManager.LoadScene(NextSceneName);
        }
    }
    void Clear(bool isWin)
    {
        level = 1;
        exp = -1;
        hp = -1;

        AcquiredItems.Clear();
        AbilityContainer.ClearAbilities();

        Pause();

        Managers.UIManager.ShowPopupUI<GameResultUI>("GameResultUI").Init(isWin);
        //if (isWin) //세이브 파일에 데이터 추가 
        {
            Managers.DataLoder.DataCache_Save.User.gold += dropgold;

            foreach (int i in AcquiredItems.Keys) //얻은 아이템 키 받아서 세이브 데이터에 벨류 증가
            {
                Managers.DataLoder.DataCache_Save.Inventory.values[Managers.DataLoder.DataCache_Save.Inventory.keys.FindIndex(x => x.Equals(i))] += AcquiredItems[i];
            }
        }

    }

    void TryDrop(string monsterid) 
    {
        Data.DataSet_Monster data = null;
        if (Managers.DataLoder.DataCache_Monsters.TryGetValue(monsterid, out data))
        {
            //아이템 드랍
            for (int i = 0; i < data.dropitems.Count; i++)
            {
                if (UnityEngine.Random.Range(0, 99) < data.dropitems[i].probability)
                {
                    Data.DataSet_Item itemdata = null;
                    if (Managers.DataLoder.DataCache_Items.TryGetValue(data.dropitems[i].id, out itemdata))
                        if (AcquiredItems.ContainsKey(data.dropitems[i].id))
                            AcquiredItems[data.dropitems[i].id] += data.dropitems[i].amount;
                        else
                            AcquiredItems.Add(data.dropitems[i].id, data.dropitems[i].amount);
                }
            }
            //골드 드랍
            if (UnityEngine.Random.Range(0, 9) < 3)
                dropgold += data.dropgold;
            //경험치 드랍
            stat.Exp += data.dropexp;
            //처치한 적 수 
            killcount += 1;
        }
    }

    public void SpawnUnit(string id)
    {
        MonsterCounts += 1;
        if (OnMonsterSpawn != null)
            OnMonsterSpawn.Invoke();
    }
    public void DeadUnit(string id)
    {        
        TryDrop(id);
        //Spawn Drop Effects  + PickupItem Object;

        MonsterCounts -= 1;
        if (OnMonsterDead != null)
            OnMonsterDead.Invoke();

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
            Managers.UIManager.ShowPopupUI<SelectAbilityUI>("SelectAbilityUI").Init(stat);
        }
    }


    #region Shop
    public void AcquireGold(float price)
    {
        Managers.DataLoder.DataCache_Save.User.gold += price;
    }
    public void AddItem(int id, int amount = 1)
    {
        int i = Managers.DataLoder.DataCache_Save.Inventory.keys.FindIndex(x => x.Equals(id));
        Managers.DataLoder.DataCache_Save.Inventory.values[i] += amount;
    }
    public bool ConsumeGold(float price)
    {
        if (Managers.DataLoder.DataCache_Save.User.gold < price) 
            return false;

        Managers.DataLoder.DataCache_Save.User.gold -= price;
        return true; 
    }
  
    public bool RemoveItem(int id, int amount=1)
    {
        int i = Managers.DataLoder.DataCache_Save.Inventory.keys.FindIndex(x => x.Equals(id));
        if (Managers.DataLoder.DataCache_Save.Inventory.values[i] < amount)
            return false;

        Managers.DataLoder.DataCache_Save.Inventory.values[i] -= amount;
        return true;
    }
    #endregion

}
