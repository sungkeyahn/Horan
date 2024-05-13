using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ش� ��ũ��Ʈ�� �ΰ��� ��� ������ ���� �ϱ�

public class GameScene1TEST : BaseScene
{
    [SerializeField]
    GameObject player; //�� ������ �ӽ�, ���� �÷��̾� ���� ���� ���� ���� 

    BoxCollider NextBox;
    bool isClear;

    protected override void Init()
    {
        //SceneName = "GameTest"; //���� ���� �ڵ�
        NextBox = GetComponent<BoxCollider>();   
    }
    void Start()
    {
        //1. �ش� �ʿ� Ŭ����� ȣ��� �Լ��� ���ε�
        //2. �÷��̾� ĳ���� + ���� ĳ���� ����[����]
        //3. ī�޶� ����

        //Managers.ContentsManager.WaveStart();// ������̺� ������ �ʱ�ȭ �� ����
        Managers.ContentsManager.OnWaveClear -= WaveClear;
        Managers.ContentsManager.OnWaveClear += WaveClear;

        //�÷��̾�+���� ���� �ڵ� �ʿ�
        Camera.main.GetComponent<CameraComponent>().SetPlayer(player);
    }

    protected virtual void WaveClear() 
    {
        isClear = true;
        Managers.ContentsManager.OnWaveClear -= WaveClear;
        if (string.IsNullOrEmpty(NextSceneName))
        {
            Managers.ContentsManager.StageClear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.layer==LayerMask.NameToLayer("Player") && isClear)
        {
            if (NextSceneName != "")
                Managers.MySceneManager.LoadScene(NextSceneName);
        }
    }
}
