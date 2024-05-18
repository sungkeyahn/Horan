using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcquiredItemSlot : BaseUI
{
    bool isinit;
    enum Components { Image_Item }
    Image itemsprite;
    public void Init(string spriteID)
    {
        Init();
        itemsprite.sprite = Managers.DataLoder.DataCache_Sprite[spriteID];
        itemsprite.color = Color.white;
    }
    public override void Init()
    {
        if (isinit) return;
        Bind<GameObject>(typeof(Components));
        itemsprite = GetObject((int)Components.Image_Item).GetComponent<Image>();
        isinit = true;
    }
}
