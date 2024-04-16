using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene1TEST : BaseScene
{
   
    [SerializeField]
    GameObject player; //이 변수는 임시, 추후 플레이어 생성 구현 이후 삭제 

    protected override void Init()
    {
        Debug.Log("GameScene1TEST");
        SceneName = "GameTest1";
        //Managers.UIManager.ShowSceneUI<LobbyUI>("CharacterHUD");
    }

    void Start()
    {
        //1. 해당 맵에 클리어시 호출될 함수들 바인딩
        //2. 플레이어 캐릭터 + 몬스터 캐릭터 생성[스폰]
        //3. 카메라 세팅

        Managers.ContentsManager.OnWaveClear -= QuitLobby;
        Managers.ContentsManager.OnWaveClear += QuitLobby;
        
        //플레이어 캐릭터 생성 코드 필요
        Camera.main.GetComponent<CameraComponent>().SetPlayer(player);
    }

    public override void Clear()
    {

    }

    void QuitLobby()
    { Managers.MySceneManager.LoadScene("Lobby"); }
}
