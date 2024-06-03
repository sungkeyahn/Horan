using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SelectAbilitySlotUI : BaseUI
{
    bool isInit=false;
    enum Components { Button_SelectAbilitySlot, Image_AbilutyICon, Text_AbilityName, Text_AbilityInfo }
    
    int ID;
    PlayerStat Stat;

    public override void Init()
    {
        if (isInit) return;
       
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_SelectAbilitySlot), OnBtnClicked_AbilitySlot, UIEvent.Click);

        isInit = true;
    }
    public void Init(int id,PlayerStat stat)
    {
        Init();
        GetObject((int)Components.Text_AbilityName).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_LatentAbility[id].abilityname;
        GetObject((int)Components.Text_AbilityInfo).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_LatentAbility[id].abilityinfo;
        ID = id;
        Stat = stat;
    }
    public void OnBtnClicked_AbilitySlot(PointerEventData data)
    {
        LatentAbility ability = new LatentAbility(ID);
        Managers.ContentsManager.AbilityContainer.AddAbility(ability, Stat);
    }
}
/* 팝업  스크립트 따로 만들어서 거기다 구현 해야 함
BindEvent(GetObject((int)Components.Button_Option1), OnBtnClicked_Option1, UIEvent.Click);
BindEvent(GetObject((int)Components.Button_Option2), OnBtnClicked_Option2, UIEvent.Click);
BindEvent(GetObject((int)Components.Button_Option3), OnBtnClicked_Option3, UIEvent.Click);
Option1Text = GetObject((int)Components.Button_Option1).GetComponentInChildren<TMP_Text>();
Option2Text = GetObject((int)Components.Button_Option2).GetComponentInChildren<TMP_Text>();
Option3Text = GetObject((int)Components.Button_Option3).GetComponentInChildren<TMP_Text>();
      Option1Text.text = Managers.DataLoder.DataCache_LatentAbility[Option1AbilityID].abilityname;
Option2Text.text = Managers.DataLoder.DataCache_LatentAbility[Option2AbilityID].abilityname;
Option3Text.text = Managers.DataLoder.DataCache_LatentAbility[Option3AbilityID].abilityname;

 */