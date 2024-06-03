using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : BaseUI
{
    public override void Init()
    {
        Managers.UIManager.SetCanvas(gameObject,true);
    }
    public virtual void ClosePopupUI()
    {
        Managers.UIManager.ClosePopupUI(this);
    }
}
