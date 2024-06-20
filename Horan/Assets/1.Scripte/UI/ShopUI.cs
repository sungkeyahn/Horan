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
    public List<ShopItemSlotUI> sellslots = new List<ShopItemSlotUI>();

    public Einventype inventype = Einventype.Weapon;

    GameObject prefab_ItemSlotUI;
    GameObject prefab_ShopItemSlotUI;

    Color selectedBtnColor = new Color(1, 1, 1, 1);
    Color defaultBtnColor = new Color(1, 1, 1, 0.25f);

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

        BindEvent(GetObject((int)Components.Button_BuyMatTab), OnBtnClicked_BuyMatTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_BuyAccTab), OnBtnClicked_BuyAccTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_SellTab), OnBtnClicked_SellTab, UIEvent.Click);


        prefab_ItemSlotUI = Resources.Load<GameObject>($"UI/Slot/ItemSlot");
        for (int i = 0; i < 20; i++)
        {
            GameObject ob = Instantiate(prefab_ItemSlotUI, GetObject((int)Components.Panel_ItemSlots).transform);
            ob.name = "ItemSlot";
            itemslots.Add(ob.GetComponent<ItemSlotUI>());
            ob.GetComponent<ItemSlotUI>().Init(i, Einventype.Weapon);

        }
        prefab_ShopItemSlotUI = Resources.Load<GameObject>($"UI/Slot/ShopItemSlot");

        TabClick(Data.EShopTabType.BuyMaterial);

        SetGold();
        isInit = true;
    }

    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        ClosePopupUI();
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
    }
    public void OnBtnClicked_WeaponTab(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        TabClick(Einventype.Weapon);
    }
    public void OnBtnClicked_CostumeTab(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        TabClick(Einventype.Costume);
    }
    public void OnBtnClicked_HatTab(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        TabClick(Einventype.Hat);
    }
    public void OnBtnClicked_AccTab(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        TabClick(Einventype.Acc);
    }
    public void OnBtnClicked_MatTab(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        TabClick(Einventype.Mat);
    }


    public void OnBtnClicked_BuyMatTab(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        TabClick(Data.EShopTabType.BuyMaterial);
    }
    public void OnBtnClicked_BuyAccTab(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        TabClick(Data.EShopTabType.BuyAccessory);
    }
    public void OnBtnClicked_SellTab(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
        TabClick(Data.EShopTabType.SellItem);
    }
    void DeleteChild(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i))
                Destroy(gameObject.transform.GetChild(i).gameObject);
        }
     
    }

    public void TabClick(Einventype type)
    {
        inventype = type;
        for (int i = 0; i < itemslots.Count; i++)
            itemslots[i].Init(i, type);
    }
    void TabClick(Data.EShopTabType type)
    {
        switch (type)
        {
            case Data.EShopTabType.BuyMaterial:
                GetObject((int)Components.Button_BuyMatTab).GetComponent<Image>().color = selectedBtnColor;
                GetObject((int)Components.Button_BuyAccTab).GetComponent<Image>().color = defaultBtnColor;
                GetObject((int)Components.Button_SellTab).GetComponent<Image>().color = defaultBtnColor;
                break;
            case Data.EShopTabType.BuyAccessory:
                GetObject((int)Components.Button_BuyMatTab).GetComponent<Image>().color = defaultBtnColor;
                GetObject((int)Components.Button_BuyAccTab).GetComponent<Image>().color = selectedBtnColor;
                GetObject((int)Components.Button_SellTab).GetComponent<Image>().color = defaultBtnColor;
                break;
            case Data.EShopTabType.SellItem:
                GetObject((int)Components.Button_BuyMatTab).GetComponent<Image>().color = defaultBtnColor;
                GetObject((int)Components.Button_BuyAccTab).GetComponent<Image>().color = defaultBtnColor;
                GetObject((int)Components.Button_SellTab).GetComponent<Image>().color = selectedBtnColor;
                break;
        }
        DeleteChild(GetObject((int)Components.Panel_ShopList));
        sellslots.Clear();

        for (int i = 0; i < Managers.DataLoder.DataCache_Shop[type].products.Count; i++)
        {
            GameObject ob = Instantiate(prefab_ShopItemSlotUI, GetObject((int)Components.Panel_ShopList).transform);
            ob.name = "ShopItemSlot";
            sellslots.Add(ob.GetComponent<ShopItemSlotUI>());
            ob.GetComponent<ShopItemSlotUI>().Init(type, i);
        }
    }
    public void SetGold()
    {
        GetObject((int)Components.Text_Gold).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_Save.User.gold.ToString();
    }
}
