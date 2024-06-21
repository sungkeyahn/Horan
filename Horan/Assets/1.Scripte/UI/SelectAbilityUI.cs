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

    
    PlayerStat Stat;

    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        //BindEvent(GetObject((int)Components.Button_ClosePopup), OnBtnClicked_ClosePopup, UIEvent.Click);

        Stat = Managers.ContentsManager.stat;
        //SelectAbilityPopup 프리팹 동적 생성 필요
        GameObject prefab = Resources.Load<GameObject>($"UI/Slot/SelectAbilitySlot");
        for (int i = 0; i < 3; i++)
        {
            GameObject ob = Instantiate(prefab, GetObject((int)Components.Panel_AbilityPopup).transform);
            ob.name = "SelectAbilitySlot";
            ob.GetComponent<SelectAbilitySlotUI>().Init(UnityEngine.Random.Range(1, Managers.DataLoder.DataCache_LatentAbility.Count),Stat);
           
        }
        //동적 생성 이후 해당 버튼 클릭시 Close 호출 필요


        isInit = true;
    }
    public void Init(PlayerStat stat)
    {
        Init();
        Stat = stat;
       // Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_LevelUp"), 1f);
    }

    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
      //  Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        ClosePopupUI();
    }
    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.ContentsManager.Resume();
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
    }
}
