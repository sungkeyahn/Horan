using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene1TEST : BaseScene
{
    protected override void Init()
    {
        Debug.Log("GameScene1TEST");
        SceneName = "GameTest1";
        //Managers.UIManager.ShowSceneUI<LobbyUI>("CharacterHUD");
    }

    void Start()
    {
        Managers.ContentsManager.OnWaveClear -= QuitLobby;
        Managers.ContentsManager.OnWaveClear += QuitLobby;
    }

    public override void Clear()
    {

    }

    void QuitLobby()
    { Managers.MySceneManager.LoadScene("Lobby"); }
}
