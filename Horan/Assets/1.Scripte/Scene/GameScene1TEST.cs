using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 스크립트는 인게임 담당 씬으로 구현 하기

public class GameScene1TEST : BaseScene
{
    GameObject player; //이 변수는 임시, 추후 플레이어 생성 구현 이후 삭제 
    bool isClear;

    void Start()
    {
       // if (string.IsNullOrEmpty(NextSceneName))
            //Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_BossMapBGM"), 1f);
       // else
            //Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_DefaultMapBGM"), 1f);
        //플레이어+몬스터 생성 코드 필요
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
        Managers.ContentsManager.OnWaveClear -= ClearScene;
    }
    /*        PlayerStat stat = player.GetComponent<PlayerStat>();
        Managers.ContentsManager.level = stat.Level;
        Managers.ContentsManager.exp = stat.Exp;
        Managers.ContentsManager.hp = stat.Hp;*/
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && isClear)
        {
            Managers.ContentsManager.Clear(NextSceneName); 
        }
    }
}
