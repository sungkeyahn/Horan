using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameModes { StoryMode, DefanseMode, TutorialMode }
public class ContentsManager 
{
    /*  �Ŵ����� ���
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

    public void StageStart(EGameModes gameMode,int stage=1) //�κ񿡼� ���ÿ�
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
    {//�������� Ŭ���� �� ȣ��
        Debug.Log("StageClear");
        //GamePuase();

        //Managers.UIManager.ShowPopupUI<ClearUI>(); -> ClearUI

        //AddedPlayData(curStageindex); -> DataSave();�� ���� ���� �ϱ� 
        //�� ���������� Ŭ���� �ߴ°�? , � ��� ����°�? , ���õ�?  

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
     //���̺� ���� �� ȣ��
     //���� ī��Ʈ ���� ������ �ʱ�ȭ + ���� �������� ���� ȣ�� �Ǿ�� ��  

        
    }
    public void WaveClear()
    {
     //���̺� Ŭ���� �� ȣ��
     //������ ���� �� ����? 

     
    }
    #endregion
    
    //���� ���� �� �ش� �ڷᱸ���� ������ϰ� ��������� �ش� �����͸� �Ѱܾ��� 
    public Dictionary<int,int> AcquiredItems = new Dictionary<int, int>(); // ȹ���� ������  id , ���� 

    bool TryItemDrop(string id) //���� Ű���� string ���� ���� ���� �ʿ�
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
                    Debug.Log(String.Format($"{0} {1}�� ����", data.dropitems[i].id, data.dropitems[i].amount));
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
        //��� ��� 
        //��� ����ġ 
        

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