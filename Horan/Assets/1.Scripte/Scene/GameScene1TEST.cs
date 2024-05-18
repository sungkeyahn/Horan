using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 스크립트는 인게임 담당 씬으로 구현 하기

public class GameScene1TEST : BaseScene
{
    [SerializeField]
    GameObject player; //이 변수는 임시, 추후 플레이어 생성 구현 이후 삭제 

    BoxCollider NextBox;
    bool isClear;

    protected override void Init()
    {
        //SceneName = "GameTest"; //삭제 예정 코드
        NextBox = GetComponent<BoxCollider>();   
    }
    void Start()
    {
        //Managers.ContentsManager.WaveStart();// 현재웨이브 데이터 초기화 및 세팅
        Managers.ContentsManager.OnWaveClear -= WaveClear;
        Managers.ContentsManager.OnWaveClear += WaveClear;

        //플레이어+몬스터 생성 코드 필요
        Camera.main.GetComponent<CameraComponent>().SetPlayer(player);
    }

    protected virtual void WaveClear() 
    {
        isClear = true;
        Managers.ContentsManager.OnWaveClear -= WaveClear;
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
