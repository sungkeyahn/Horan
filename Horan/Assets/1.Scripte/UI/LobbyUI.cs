using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyUI : SceneUI
{
    enum Components { Slider_EXBar, Text_UserName, Text_Level, Button_StartGame, Button_Tutorial, Button_GameSetting, Button_CharacterUpgrade, Button_EquipmentUpgrade }
   
    public float ExpRatio = 1;

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_StartGame), OnBtnClicked_StartGame, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Tutorial), OnBtnClicked_Tutorial, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_CharacterUpgrade), OnBtnClicked_CharacterUpgrade, UIEvent.Click);

        GetObject((int)Components.Slider_EXBar).GetComponent<Slider>().value = ExpRatio;

        // GameObject go = GetObject((int)Components.Button_GameSetting);

    }
    public void OnBtnClicked_StartGame(PointerEventData data)
    {
        Debug.Log("StartGame");
        Managers.MySceneManager.LoadScene("GameTest1");
    }
    public void OnBtnClicked_Tutorial(PointerEventData data)
    {
        Debug.Log("Tutorial");
    }
    public void OnBtnClicked_CharacterUpgrade(PointerEventData data)
    {
       Managers.UIManager.ShowPopupUI<CharacterUpgradeUI>("CharacterUpgradeUI");
       Managers.UIManager.GetSceneUI().gameObject.SetActive(false);
    }



}
