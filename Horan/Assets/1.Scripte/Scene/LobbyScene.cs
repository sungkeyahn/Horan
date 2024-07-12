using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    LobbyUI lobby;


    //구입 판매 강화 클릭

    protected override void Init()
    {
        SceneName = "Lobby";
        lobby= Managers.UIManager.ShowSceneUI<LobbyUI>("LobbyUI"); //lobby.Init();
            
        Sound_BGM = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_LobbyBGM"], transform).GetComponent<AudioSource>();
        Sound_BGM.Play();
    }
   
    /*
    Sound_Click = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Click"], transform).GetComponent<AudioSource>();
        Sound_Buy = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Buy"], transform).GetComponent<AudioSource>();
        Sound_Sell = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Sell"], transform).GetComponent<AudioSource>();
        Sound_Upgrade = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Upgrade"], transform).GetComponent<AudioSource>();
        */
}
