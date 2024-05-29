using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsManager 
{
    /*  �Ŵ��� ���
     * ���丮��� : ���� ����, ���� ���� , ���̺� ���� , ���̺� ���� ,�� �ش��ϴ� �Լ��� ContentsManager�� ����(ȣ���� �� Scene����)
     * 
     * ���� �� �κе�)
     * �κ�(���ӽ��۹�ư) -> ��������UI -> ���� ���� -> �� �̵�
     * �� ��ũ��Ʈ���� �������� ���� �Լ� ȣ��
     * ���� ����� �������� Ŭ���� ���� üũ
     * ����ɷ� ���ý� ����ɷ� ���� ����
     * 
     * ���� �������� �� ������ ���͵�
     * �������� Ŭ���� ���� 
     * ȹ������۵� 
     * ȹ�� ����ɷ�
     * 
     * 
     * �̺�Ʈ�� �� �ѷ��ִ� ������ �ؾ��ҰŰ�����?
     * 
     * 
     * ���� ����)
     * 1. ���� ����� ä�� + ������ + ��ȭ + ����ġ ��� �ý��� 
     * 2. ĳ���� ���� �� �� ����ɷ� ���� UI ���� + ���� ����
     * 3. �� ����ɷ� UI���� ������ �ɷ��� ���� 
     * 4. ����� ����ɷ��� �������Ŵ������� ���� ����
     * 5. �� �̵�(���̺� ����)�� ���+����+���� �ɷ�ġ ��� 
     * 
     */
    public Action OnStageClear;
    public Action OnWaveClear;
    public Action OnWaveStart;
    public List<int> AcquiredItems = new List<int>(); //ȹ���� �����۵� 
    public LatentAbilityContainer AbilityContainer = new LatentAbilityContainer(); //ȹ���� ����ɷµ�\
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
        Managers.MySceneManager.LoadScene($"Level{stage}");
    }
    public void StageClear() // Scene Scripte�ּ� ȣ��
    {
        AcquiredItems.Clear();
        AbilityContainer.ClearAbilities();
    }
    public void WaveStart() // Scene Scripte�ּ� ȣ�� 
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
                    for (int j = 0; j < 25; j++) //�κ� ���� 
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

                        AcquiredItems.Add(itemdata.id); //���� �ʿ�
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
