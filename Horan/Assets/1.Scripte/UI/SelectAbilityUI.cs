using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SelectAbilityUI : PopupUI
{
    bool isInit;

    enum Components { Panel_AbilityPopup, Button_ClosePopup}

    AudioSource Sound_Click;
    AudioSource Sound_LevelUp;

    PlayerStat Stat;

    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));

        Sound_Click = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Click"], transform).GetComponent<AudioSource>();
        Sound_LevelUp = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_LevelUp"], transform).GetComponent<AudioSource>();

        Stat = Managers.ContentsManager.stat;
        GameObject prefab = Resources.Load<GameObject>($"UI/Slot/SelectAbilitySlot");
        for (int i = 0; i < 3; i++)
        {
            GameObject ob = Instantiate(prefab, GetObject((int)Components.Panel_AbilityPopup).transform);
            ob.name = "SelectAbilitySlot";
            ob.GetComponent<SelectAbilitySlotUI>().Init(UnityEngine.Random.Range(1, Managers.DataLoder.DataCache_LatentAbility.Count),Stat);           
        }

        isInit = true;
    }
    public void Init(PlayerStat stat)
    {
        Init();
        Stat = stat;
        Sound_LevelUp.Play();
    }
    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        Sound_Click.Play();
        ClosePopupUI();
    }
    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.ContentsManager.Resume();
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
    }

}
