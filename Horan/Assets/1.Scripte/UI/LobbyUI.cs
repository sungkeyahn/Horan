using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class LobbyUI : SceneUI
{
    bool isinit=false;
    enum Components { Button_Start , Button_Invetory , Button_Shop, Button_Upgrade, Button_Setting }
    public override void Init()
    {
        if (isinit) return;
        base.Init();
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Start), OnBtnClicked_Start, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Invetory), OnBtnClicked_Inventory, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Shop), OnBtnClicked_Shop, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Upgrade), OnBtnClicked_Upgrade, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Setting), OnBtnClicked_Setting, UIEvent.Click);
        isinit = true;
    }
    public void OnBtnClicked_Start(PointerEventData data)
    {
        Managers.ContentsManager.StageSelect(1);
    }
    public void OnBtnClicked_Inventory(PointerEventData data)
    {
       Debug.Log("Inventory");
        //InventoryUI 에서 플레이어 캐릭터로 카메라로 전환 필요 
       Managers.UIManager.ShowPopupUI<InventoryUI>("InventoryUI");

       Managers.UIManager.GetSceneUI().gameObject.SetActive(false);
    }
    public void OnBtnClicked_Shop(PointerEventData data)
    {
        Debug.Log("Shop");
        Managers.UIManager.ShowPopupUI<ShopUI>("ShopUI");
        Managers.UIManager.GetSceneUI().gameObject.SetActive(false);
    }
    public void OnBtnClicked_Upgrade(PointerEventData data)
    {
        Debug.Log("Upgrade");
        //Managers.UIManager.ShowPopupUI<UpgradeUI>("UpgradeUI");
        Managers.UIManager.GetSceneUI().gameObject.SetActive(false);
    }
    public void OnBtnClicked_Setting(PointerEventData data)
    {
        Debug.Log("Setting");
        //Managers.UIManager.ShowPopupUI<SettingUI>("SettingUI");
        Managers.UIManager.GetSceneUI().gameObject.SetActive(false);
    }



}

//개선 이전 코드들 
/*
 * enum Components { Slider_EXBar, Text_UserName, Text_Level, Text_Gold, Button_StartGame, Button_Tutorial, Button_GameSetting, Button_CharacterUpgrade, Button_EquipmentUpgrade }
Slider ExpBar;
TMP_Text GoldText;
TMP_Text UserLevel;
TMP_Text UserName;
    ExpBar = GetObject((int)Components.Slider_EXBar).GetComponent<Slider>();
    GoldText = GetObject((int)Components.Text_Gold).GetComponent<TMP_Text>();
    UserLevel = GetObject((int)Components.Text_Level).GetComponent<TMP_Text>();
    UserName = GetObject((int)Components.Text_UserName).GetComponent<TMP_Text>();
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
 */