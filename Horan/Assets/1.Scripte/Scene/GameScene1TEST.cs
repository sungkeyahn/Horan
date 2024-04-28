using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ش� ��ũ��Ʈ�� �ΰ��� ��� ������ ���� �ϱ�

public class GameScene1TEST : BaseScene
{
    [SerializeField]
    GameObject player; //�� ������ �ӽ�, ���� �÷��̾� ���� ���� ���� ���� 
    [SerializeField]
    string NextSceneName = null;

    protected override void Init()
    {
        Debug.Log("GameScene1TEST");
        SceneName = "GameTest1"; //���� ���� �ڵ�
    }

    void Start()
    {
        //1. �ش� �ʿ� Ŭ����� ȣ��� �Լ��� ���ε�
        //2. �÷��̾� ĳ���� + ���� ĳ���� ����[����]
        //3. ī�޶� ����
        Managers.ContentsManager.WaveStart();// ������̺� ������ �ʱ�ȭ �� ����
      
        Managers.ContentsManager.OnWaveClear -= EnterNextMap;
        Managers.ContentsManager.OnWaveClear += EnterNextMap;
        
        //�÷��̾� ĳ���� ���� �ڵ� �ʿ�
        Camera.main.GetComponent<CameraComponent>().SetPlayer(player);
    }

    public override void Clear()
    {

    }

    void EnterNextMap()
    {
        //Managers.UIManager.ShowPopupUI<WaveClearUI>();
        //�Ʒ� �ڵ�� UI ��ư Ŭ���� ȣ�� 
        if (NextSceneName != null)
            Managers.MySceneManager.LoadScene(NextSceneName);
    }
}
