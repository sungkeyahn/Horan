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
    
    AudioSource Sound_Click;
    
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
#region Bind
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_ClosePopup), OnBtnClicked_ClosePopup, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_BuyMatTab), OnBtnClicked_BuyMatTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_BuyAccTab), OnBtnClicked_BuyAccTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_SellTab), OnBtnClicked_SellTab, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_WeaponTab), OnBtnClicked_WeaponTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_CostumeTab), OnBtnClicked_CostumeTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_HatTab), OnBtnClicked_HatTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_AccTab), OnBtnClicked_AccTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_MatTab), OnBtnClicked_MatTab, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_BuyMatTab), OnPointDown_BuyMatTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_BuyAccTab), OnPointDown_BuyAccTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_SellTab), OnPointDown_SellTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_WeaponTab), OnPointDown_WeaponTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_CostumeTab), OnPointDown_CostumeTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_HatTab), OnPointDown_HatTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_AccTab), OnPointDown_AccTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_MatTab), OnPointDown_MatTab, UIEvent.PointDown);

        BindEvent(GetObject((int)Components.Button_BuyMatTab), OnPointUp_BuyMatTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_BuyAccTab), OnPointUp_BuyAccTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_SellTab), OnPointUp_SellTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_WeaponTab), OnPointUp_WeaponTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_CostumeTab), OnPointUp_CostumeTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_HatTab), OnPointUp_HatTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_AccTab), OnPointUp_AccTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_MatTab), OnPointUp_MatTab, UIEvent.PointUp);

        #endregion Bind
        Sound_Click = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Click"], transform).GetComponent<AudioSource>();

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
        Sound_Click.Play();
        ClosePopupUI();
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
    }
    public void OnBtnClicked_WeaponTab(PointerEventData data)
    {
        Sound_Click.Play();
        TabClick(Einventype.Weapon);
    }
    public void OnBtnClicked_CostumeTab(PointerEventData data)
    {
        Sound_Click.Play();
        TabClick(Einventype.Costume);
    }
    public void OnBtnClicked_HatTab(PointerEventData data)
    {
        Sound_Click.Play();
        TabClick(Einventype.Hat);
    }
    public void OnBtnClicked_AccTab(PointerEventData data)
    {
        Sound_Click.Play();
        TabClick(Einventype.Acc);
    }
    public void OnBtnClicked_MatTab(PointerEventData data)
    {
        TabClick(Einventype.Mat);
    }

    public void OnBtnClicked_BuyMatTab(PointerEventData data)
    {
        TabClick(Data.EShopTabType.BuyMaterial);
    }
    public void OnBtnClicked_BuyAccTab(PointerEventData data)
    {
        TabClick(Data.EShopTabType.BuyAccessory);
    }
    public void OnBtnClicked_SellTab(PointerEventData data)
    {
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
        Sound_Click.Play();
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

    //Hover
    public void OnPointDown_WeaponTab(PointerEventData data)
    {
        GetObject((int)Components.Button_WeaponTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_CostumeTab(PointerEventData data)
    {
        GetObject((int)Components.Button_CostumeTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_HatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_HatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_AccTab(PointerEventData data)
    {
        GetObject((int)Components.Button_AccTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_MatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_MatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_BuyMatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_BuyMatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_BuyAccTab(PointerEventData data)
    {
        GetObject((int)Components.Button_BuyAccTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_SellTab(PointerEventData data)
    {
        GetObject((int)Components.Button_SellTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointUp_WeaponTab(PointerEventData data)
    {
        GetObject((int)Components.Button_WeaponTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_CostumeTab(PointerEventData data)
    {
        GetObject((int)Components.Button_CostumeTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_HatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_HatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_AccTab(PointerEventData data)
    {
        GetObject((int)Components.Button_AccTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_MatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_MatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_BuyMatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_BuyMatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_BuyAccTab(PointerEventData data)
    {
        GetObject((int)Components.Button_BuyAccTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_SellTab(PointerEventData data)
    {
        GetObject((int)Components.Button_SellTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
}
