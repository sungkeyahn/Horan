using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        Debug.Log("LobbySceneInit");
        SceneName = "Lobby";
        LobbyUI lobby= Managers.UIManager.ShowSceneUI<LobbyUI>("LobbyUI");
        lobby.Init();

        lobby.SetUserName("¿Ã∏ß 1");
        lobby.SetUserLevel(99);
        lobby.SetGoldText(123123123);
        lobby.SetExBar(0.5f);
    }

    public override void Clear()
    {
        
    }
}
