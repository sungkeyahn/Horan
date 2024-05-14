using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameModes { StoryMode, DefanseMode, TutorialMode }
public class ContentsManager 
{
    /*  �Ŵ��� ���
     * �� ��帶�� �ΰ��� ���뿡 ���� �����Ǿ� ����� �����͵��� �����Ͽ� ������ ����ɶ� ���̺������� ����� �C�����ִ� ����
     * + �� ���Ӹ���� ���� ����� ó���Ǿ���ϴ� ���۵��� �޼���� ����
     * 
     * ���丮��� : ���� ����, ���� ���� , ���̺� ���� , ���̺� ���� ,�� �ش��ϴ� �Լ��� ContentsManager�� ����(ȣ���� �� Scene����)
     * 
     * ���� ���� �帧)
     * ������ ���丮 ��带 ���� -> ���������� Ŭ�� -> �� �̵� -> �� ����ũ��Ʈ���� ���ӽ��� �Լ� ȣ�� -> �������� ����
     * (�� ���� ���۰� ���������� �Ѿ�� �κ��� ���� �����ϴ�  ����ũ��Ʈ���� ���)
     * ������ ���� ����Ǵ��� �ΰ��� ������ ������ �Ѿ�� �ֵ��� ������ �Ŵ������� ������ ����
     * 
     * 
     * �ֿ� ��� 
     * 1. �������� ���� �� ���� ������ ȣ���� �Լ� ���� 
     * 2. ���� �� ���� ������ ���� �� ����
     * 
     * ���� ���� 
     * ������ ���� -> ������ ��� -> ������ ���� -> ������ �Ŵ����� ���� -> 
     * ȹ�� ������ ������ �� ������ �ֱ� 
     * ȹ�� ������ �����ؼ� �κ�� �ѱ��
     * 
     * �κ񿡼� �� ������ �κ��丮 ����
     * 
     */ 

    #region Stage
    public Action OnStageClear;
    int curStageindex = 0;
    public void StageSelect(EGameModes gameMode,int stage=1) //�κ񿡼� ���ÿ�
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
    {
        //�������� Ŭ����� ����Ǿ���� ������ ���� -> �κ������� + ���� ����ġ(���õ�) + ��ȭ
        //GamePuase();
        //Managers.UIManager.ShowPopupUI<StageClearUI>();
        Debug.Log(Managers.DataLoder.DataCache_Save.User.gold); 
    }
    #endregion

    #region Wave
    public Action OnWaveClear;
    public int MonsterCounts;
    void WaveClear() //��� ���� ����� �ڵ� ȣ��
    {
        if (OnWaveClear != null)
            OnWaveClear.Invoke();
        MonsterCounts = 0;
    }
    #endregion

    #region ItemDrop ItemAcquired
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
                        DropNothing = false;
                    }
                }
            }
        }
        return !DropNothing;
    }
    #endregion

    #region Unit
    Dictionary<string, UnitController> SpawnedUnits=new Dictionary<string, UnitController>();
    public void SpawnUnit(string id)
    {
        MonsterCounts += 1;
    }
    public void DeadUnit(string id)
    {
        if (TryItemDrop(id))
        {
            //Spawn Drop Effects  + PickupItem Objects
        }
        //��� ��� 
        //��� ����ġ 

        MonsterCounts -= 1;
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