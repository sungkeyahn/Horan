using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ش� ��ũ��Ʈ�� �ΰ��� ��� ������ ���� �ϱ�

public class GameScene1TEST : BaseScene
{
    GameObject player; //�� ������ �ӽ�, ���� �÷��̾� ���� ���� ���� ���� 
    bool isClear;

    void Start()
    {
        //�÷��̾�+���� ���� �ڵ� �ʿ�
        player = FindObjectOfType<PlayerController>().gameObject;
        Camera.main.GetComponent<CameraComponent>().SetPlayer(player);

        Managers.ContentsManager.player = player.GetComponent<PlayerController>();
        Managers.ContentsManager.stat = player.GetComponent<PlayerStat>();

        Managers.ContentsManager.WaveStart();

        Managers.ContentsManager.OnWaveClear -= ClearScene;
        Managers.ContentsManager.OnWaveClear += ClearScene;
    }

    protected virtual void ClearScene() 
    {
        isClear = true;
        PlayerStat stat = player.GetComponent<PlayerStat>();
        Managers.ContentsManager.level = stat.Level;
        Managers.ContentsManager.exp = stat.Exp;
        Managers.ContentsManager.hp = stat.Hp;

        Managers.ContentsManager.OnWaveClear -= ClearScene;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && isClear)
        {
            if (string.IsNullOrEmpty(NextSceneName))
            {
                Managers.UIManager.ShowPopupUI<GameResultUI>("GameResultUI").Init(true);
                Managers.ContentsManager.Pause();
                Managers.ContentsManager.StageClear();  
            }
            else
            {
                Managers.MySceneManager.LoadScene(NextSceneName);
            }
        }
    }
}
