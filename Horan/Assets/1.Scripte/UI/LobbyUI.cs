using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class LobbyUI : SceneUI
{
    bool isinit=false;

    enum Components { Slider_EXBar, Text_UserName, Text_Level, Text_Gold, Button_StartGame, Button_Tutorial, Button_GameSetting, Button_CharacterUpgrade, Button_EquipmentUpgrade }
   
    Slider ExpBar;
    TMP_Text GoldText;
    TMP_Text UserLevel;
    TMP_Text UserName;
    public override void Init()
    {
        if (isinit) return;
        base.Init();
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_StartGame), OnBtnClicked_StartGame, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Tutorial), OnBtnClicked_Tutorial, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_CharacterUpgrade), OnBtnClicked_Inventory, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_GameSetting), OnBtnClicked_GameSetting, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_EquipmentUpgrade), OnBtnClicked_EquipmentUpgrade, UIEvent.Click);

        ExpBar = GetObject((int)Components.Slider_EXBar).GetComponent<Slider>();
        GoldText = GetObject((int)Components.Text_Gold).GetComponent<TMP_Text>();
        UserLevel = GetObject((int)Components.Text_Level).GetComponent<TMP_Text>();
        UserName = GetObject((int)Components.Text_UserName).GetComponent<TMP_Text>();
        isinit = true;
    }
    public void OnBtnClicked_StartGame(PointerEventData data)
    {
        Debug.Log("SelectLevel1");
        Managers.ContentsManager.StageSelect(1);
    }
    public void OnBtnClicked_Tutorial(PointerEventData data)
    {
        Debug.Log("Tutorial");
    }
    public void OnBtnClicked_Inventory(PointerEventData data)
    {
       //   Managers.UIManager.ShowPopupUI<CharacterUpgradeUI>("EquipUI");
       Managers.UIManager.GetSceneUI().gameObject.SetActive(false);
    }
    public void OnBtnClicked_GameSetting(PointerEventData data)
    { }
    public void OnBtnClicked_EquipmentUpgrade(PointerEventData data)
    { }

    public void SetExBar(float ExpRatio)
    {
        ExpBar.value = ExpRatio;
    }
    public void SetGoldText(float GoldValue)
    {
        GoldText.text = $"{GoldValue}";
    }
    public void SetUserLevel(int level)
    {
        UserLevel.text = $"레벨 : {level}";
    }
    public void SetUserName(string name)
    {
        UserName.text = $"이름 : {name}";
    }
}
