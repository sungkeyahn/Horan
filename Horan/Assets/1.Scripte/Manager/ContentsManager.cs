using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsManager 
{
    /*  �Ŵ��� ���
     * 1. ���� ����� ä�� + ������ + ��ȭ + ����ġ ��� �ý��� 
     * 2. ĳ���� ���� �� �� ����ɷ� ���� UI ���� + ���� ����
     * 3. �� ����ɷ� UI���� ������ �ɷ��� ���� 
     * 4. ����� ����ɷ��� �������Ŵ������� ���� ����
     * 5. �� �̵�(���̺� ����)�� ���+����+���� �ɷ�ġ ����
     */
    public Action OnStageClear;
    public Action OnWaveClear;
    public Action OnWaveStart;
    public Action OnMonsterSpawn;
    public Action OnMonsterDead;

    public Dictionary<int ,int> AcquiredItems = new Dictionary<int, int>(); //ȹ���� �����۵� 
    public int dropgold; //ȹ���� ��� 
    public int killcount; //�������� ��ü óġ�� �� �� 

    public LatentAbilityContainer AbilityContainer = new LatentAbilityContainer(); //ȹ���� ����ɷµ�
    public int MonsterCounts; //���� �����ִ� ���� ��

    public PlayerController player;
    public PlayerStat stat;

    public int level=1;
    public float exp=-1;
    public float hp=-1;
    
    public void StageSelect(int stage=1) // LobbyUI���� ȣ��
    {
        AcquiredItems.Clear();
        AbilityContainer.ClearAbilities();
        Managers.MySceneManager.LoadScene($"Level {stage}");
    }
    public void WaveStart() // Scene Scripte�ּ� ȣ�� 
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
        //if (isWin) //���̺� ���Ͽ� ������ �߰� 
        {
            Managers.DataLoder.DataCache_Save.User.gold += dropgold;

            foreach (int i in AcquiredItems.Keys) //���� ������ Ű �޾Ƽ� ���̺� �����Ϳ� ���� ����
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
            //������ ���
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
            //��� ���
            if (UnityEngine.Random.Range(0, 9) < 3)
                dropgold += data.dropgold;
            //����ġ ���
            stat.Exp += data.dropexp;
            //óġ�� �� �� 
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
