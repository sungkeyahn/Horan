using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopUI : PopupUI
{
    bool isInit;

    enum Components { Button_ClosePopup, Panel_ItemSlots, Button_WeaponTab, Button_CostumeTab, Button_HatTab, Button_AccTab, Button_MatTab, Text_Gold , Button_BuyMatTab, Button_BuyAccTab, Button_SellTab, Panel_ShopList }
    public List<ItemSlotUI> itemslots = new List<ItemSlotUI>();


    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_ClosePopup), OnBtnClicked_ClosePopup, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_WeaponTab), OnBtnClicked_WeaponTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_CostumeTab), OnBtnClicked_CostumeTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_HatTab), OnBtnClicked_HatTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_AccTab), OnBtnClicked_AccTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_MatTab), OnBtnClicked_MatTab, UIEvent.Click);
        GameObject prefab = Resources.Load<GameObject>($"UI/Slot/ItemSlot");
        for (int i = 0; i < 20; i++)
        {
            GameObject ob = Instantiate(prefab, GetObject((int)Components.Panel_ItemSlots).transform);
            ob.name = "ItemSlot";
            itemslots.Add(ob.GetComponent<ItemSlotUI>());
            ob.GetComponent<ItemSlotUI>().Init(i, Einventype.Weapon);

        }

        isInit = true;
    }

    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        ClosePopupUI();
    }
    public void OnBtnClicked_WeaponTab(PointerEventData data)
    {
        for (int i = 0; i < itemslots.Count; i++)
            itemslots[i].Init(i, Einventype.Weapon);
    }
    public void OnBtnClicked_CostumeTab(PointerEventData data)
    {
        for (int i = 0; i < itemslots.Count; i++)
            itemslots[i].Init(i, Einventype.Costume);
    }
    public void OnBtnClicked_HatTab(PointerEventData data)
    {
        for (int i = 0; i < itemslots.Count; i++)
            itemslots[i].Init(i, Einventype.Hat);
    }
    public void OnBtnClicked_AccTab(PointerEventData data)
    {
        for (int i = 0; i < itemslots.Count; i++)
            itemslots[i].Init(i, Einventype.Acc);
    }
    public void OnBtnClicked_MatTab(PointerEventData data)
    {
        for (int i = 0; i < itemslots.Count; i++)
            itemslots[i].Init(i, Einventype.Mat);
    }

}
