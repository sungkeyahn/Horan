using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        Debug.Log("LobbySceneInit");
        SceneName = "Lobby";
        Managers.UIManager.ShowSceneUI<LobbyUI>("LobbyUI");
    }

    public override void Clear()
    {
        
    }
}
