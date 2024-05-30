using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class SelectAbilityUI : PopupUI
{
    bool isInit;

    enum Components { Button_Option1, Button_Option2, Button_Option3, Button_ClosePopup }

    PlayerStat Stat;

    int AbilityID1;
    int AbilityID2;
    int AbilityID3;

    TMP_Text Option1Text;
    TMP_Text Option2Text;
    TMP_Text Option3Text;

    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_ClosePopup), OnBtnClicked_ClosePopup, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_Option1), OnBtnClicked_Option1, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Option2), OnBtnClicked_Option2, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Option3), OnBtnClicked_Option3, UIEvent.Click);
        Option1Text = GetObject((int)Components.Button_Option1).GetComponentInChildren<TMP_Text>();
        Option2Text = GetObject((int)Components.Button_Option2).GetComponentInChildren<TMP_Text>();
        Option3Text = GetObject((int)Components.Button_Option3).GetComponentInChildren<TMP_Text>();
        isInit = true;
    }
    public void Init(int Option1AbilityID, int Option2AbilityID, int Option3AbilityID,PlayerStat stat)
    {
        Init();
        Stat = stat;

        AbilityID1 = Option1AbilityID;
        AbilityID2 = Option2AbilityID;
        AbilityID3 = Option3AbilityID;

        Option1Text.text = Managers.DataLoder.DataCache_LatentAbility[Option1AbilityID].abilityname;
        Option2Text.text = Managers.DataLoder.DataCache_LatentAbility[Option2AbilityID].abilityname;
        Option3Text.text = Managers.DataLoder.DataCache_LatentAbility[Option3AbilityID].abilityname;
    }
    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        ClosePopupUI();
        //Managers.UIManager.ClosePopupUI(this);
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
    }
    public void OnBtnClicked_Option1(PointerEventData data)
    {
        LatentAbility ability = new LatentAbility(AbilityID1);
        Managers.ContentsManager.AbilityContainer.AddAbility(ability,Stat);

        ClosePopupUI();
        //Managers.UIManager.ClosePopupUI(this);
    }
    public void OnBtnClicked_Option2(PointerEventData data)
    {
        LatentAbility ability = new LatentAbility(AbilityID2);
        Managers.ContentsManager.AbilityContainer.AddAbility(ability, Stat);

        ClosePopupUI();
    }
    public void OnBtnClicked_Option3(PointerEventData data)
    {
        LatentAbility ability = new LatentAbility(AbilityID3);
        Managers.ContentsManager.AbilityContainer.AddAbility(ability, Stat);

        ClosePopupUI();
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.ContentsManager.Resume();
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
    }
}
