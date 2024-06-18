using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TitleUI : SceneUI
{
    bool isinit = false;
    enum Components { Button_Start}
    public override void Init()
    {
        if (isinit) return;
        base.Init();
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Start), OnBtnClicked_Start, UIEvent.Click);
        isinit = true;
    }
    public void OnBtnClicked_Start(PointerEventData data)
    {
        Managers.MySceneManager.LoadScene("Lobby");
    }
}
