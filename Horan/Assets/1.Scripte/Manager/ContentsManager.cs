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
     * 1. �������� ���� �� ���� ���� ��� 
     * 2. ���� �� ���� ������ ���� �� ����
     * 3. 
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
    public int WaveMonsterCounts
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

    #region InGameControl
    bool isPause;
    public void GamePause()
    { }
    public void GameResume()
    { }
    #endregion

}

