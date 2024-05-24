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
        //Managers.ContentsManager.WaveStart();// ������̺� ������ �ʱ�ȭ �� ����
        Managers.ContentsManager.OnWaveClear -= ClearScene;
        Managers.ContentsManager.OnWaveClear += ClearScene;

        //�÷��̾�+���� ���� �ڵ� �ʿ�
        Camera.main.GetComponent<CameraComponent>().SetPlayer(player);


        Managers.ContentsManager.WaveStart();
    }

    protected virtual void ClearScene() 
    {
        isClear = true;
        Managers.ContentsManager.OnWaveClear -= ClearScene;
        if (string.IsNullOrEmpty(NextSceneName))
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && isClear)
        {
            if (NextSceneName != "")
                Managers.MySceneManager.LoadScene(NextSceneName);
            else
            {
                GameResultUI ui = Managers.UIManager.ShowPopupUI<GameResultUI>("GaemResultUI");
                ui.result = true;
            }
        }
    }
}
