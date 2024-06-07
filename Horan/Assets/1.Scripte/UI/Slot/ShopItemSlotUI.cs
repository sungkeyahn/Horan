using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlotUI : BaseUI
{
    bool isInit;

    enum Components { Image_ProductICon, Text_PriceValue, Button_ShopSlot, Text_ShopSlotBtnName }
    public List<ItemSlotUI> itemslots = new List<ItemSlotUI>();


    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        isInit = true;
    }

}
