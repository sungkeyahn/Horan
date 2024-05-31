using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    LobbyUI lobby;
    protected override void Init()
    {
        Debug.Log("Enter the Lobby Scene");
        SceneName = "Lobby";
        lobby= Managers.UIManager.ShowSceneUI<LobbyUI>("LobbyUI");
        //lobby.Init();
    }
    private void Start()
    {
        Data.Save_User userinfo = Managers.DataLoder.DataCache_Save.User;
        /*lobby.SetUserName(userinfo.name);
        lobby.SetUserLevel(userinfo.level);
        lobby.SetGoldText(userinfo.gold);
        lobby.SetExBar(userinfo.exp);*/
    }

}
