using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class UpgradeUI : PopupUI
{
    bool isInit;

    enum Components { Button_ClosePopup, Panel_Stat, Panel_NeedMats, Text_NeedGold , Image_ResultItemICon }
    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_ClosePopup), OnBtnClicked_ClosePopup, UIEvent.Click);
        isInit = true;
    }
    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        ClosePopupUI();
    }
}
