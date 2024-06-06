using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIconSlotUI : BaseUI
{
    bool isinit = false;
    enum Components { Image }

    public override void Init()
    {
        if (isinit) return;
        Bind<GameObject>(typeof(Components));
        isinit = true;
    }
    public void Init(Sprite sprite)
    {
        Init();
        GetObject((int)Components.Image).GetComponent<Image>().sprite = sprite;
    }
}
