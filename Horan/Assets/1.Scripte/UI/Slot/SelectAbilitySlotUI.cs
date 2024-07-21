using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectAbilitySlotUI : BaseUI
{
    bool isInit=false;
    enum Components { Button_SelectAbilitySlot, Image_AbilutyICon, Text_AbilityName, Text_AbilityInfo }
   
    AudioSource Sound_Click;

    int ID;
    PlayerStat Stat;

    public override void Init()
    {
        if (isInit) return;
       
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_SelectAbilitySlot), OnBtnClicked_AbilitySlot, UIEvent.Click);

        Sound_Click = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Click"], transform).GetComponent<AudioSource>();

        isInit = true;
    }
    public void Init(int id,PlayerStat stat)
    {
        Init(); 
        GetObject((int)Components.Image_AbilutyICon).GetComponent<Image>().sprite = Managers.DataLoder.DataCache_Sprite[$"icon_ability{id}"];

        GetObject((int)Components.Text_AbilityName).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_LatentAbility[id].abilityname;
        GetObject((int)Components.Text_AbilityInfo).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_LatentAbility[id].abilityinfo;
        ID = id;
        Stat = stat;
    }
    public void OnBtnClicked_AbilitySlot(PointerEventData data)
    {
        Sound_Click.Play();
        Managers.ContentsManager.AbilityContainer.AddAbility(new LatentAbility(ID), Stat);
        Managers.UIManager.ClosePopupUI();
        Managers.ContentsManager.Resume();
    }
}
