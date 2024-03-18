using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterUpgradeUI : PopupUI
{
    enum Components { Button_ClosePopup }

    public override void Init()
    {
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_ClosePopup), OnBtnClicked_ClosePopup, UIEvent.Click);
    }
    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        Managers.UIManager.ClosePopupUI(this);
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
    }
}
