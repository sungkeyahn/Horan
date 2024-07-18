using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class InventoryUI : PopupUI
{
    bool isInit = false;
    enum Components { Panel_Stat, Panel_ItemSlots , Button_WeaponTab , Button_CostumeTab, Button_HatTab, Button_AccTab, Button_MatTab, Text_Gold, ItemEquipPopupUI, Button_ClosePopup}
    public enum EStatSlotInfo { Atk, Hp, Sp, RegenHp }

    public List<ItemSlotUI> itemslots=new List<ItemSlotUI>();
    public List<StatSlotUI> statslots = new List<StatSlotUI>();

    ItemEquipPopupUI EquipPopup;

    AudioSource Sound_Click;
    
    public override void Init()
    {
        if (isInit) return;

        #region Bind
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_ClosePopup), OnBtnClicked_ClosePopup, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_WeaponTab), OnBtnClicked_WeaponTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_CostumeTab), OnBtnClicked_CostumeTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_HatTab), OnBtnClicked_HatTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_AccTab), OnBtnClicked_AccTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_MatTab), OnBtnClicked_MatTab, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_WeaponTab), OnPointDown_WeaponTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_CostumeTab), OnPointDown_CostumeTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_HatTab), OnPointDown_HatTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_AccTab), OnPointDown_AccTab, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_MatTab), OnPointDown_MatTab, UIEvent.PointDown);

        BindEvent(GetObject((int)Components.Button_WeaponTab), OnPointUp_WeaponTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_CostumeTab), OnPointUp_CostumeTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_HatTab), OnPointUp_HatTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_AccTab), OnPointUp_AccTab, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_MatTab), OnPointUp_MatTab, UIEvent.PointUp);

        #endregion Bind


        Sound_Click = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Click"], transform).GetComponent<AudioSource>();

        GameObject prefab = Resources.Load<GameObject>($"UI/Slot/ItemSlot");
        for (int i = 0; i < 20; i++)
        {
            GameObject ob = Instantiate(prefab, GetObject((int)Components.Panel_ItemSlots).transform);
            ob.name = "ItemSlot";
            itemslots.Add(ob.GetComponent<ItemSlotUI>());
            ob.GetComponent<ItemSlotUI>().Init(i,Einventype.Weapon);

        }
        GameObject prefab2 = Resources.Load<GameObject>($"UI/Slot/StatSlot");
        for (int i = 0; i < 4; i++)
        {
            GameObject ob2 = Instantiate(prefab2, GetObject((int)Components.Panel_Stat).transform);
            ob2.name = "ItemSlot";
            statslots.Add(ob2.GetComponent<StatSlotUI>());
            ob2.GetComponent<StatSlotUI>().Init((EStatSlotInfo)i);//EStatSlotInfo.Atk
        }

        EquipPopup = GetObject((int)Components.ItemEquipPopupUI).GetComponent<ItemEquipPopupUI>();
        EquipPopup.Init();
        EquipPopup.gameObject.SetActive(false);

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
        ClickTab(Einventype.Weapon);
    }
    public void OnBtnClicked_CostumeTab(PointerEventData data)
    {
        ClickTab(Einventype.Costume);
    }
    public void OnBtnClicked_HatTab(PointerEventData data)
    {
        ClickTab(Einventype.Hat);
    }
    public void OnBtnClicked_AccTab(PointerEventData data)
    {
        ClickTab(Einventype.Acc);
    }
    public void OnBtnClicked_MatTab(PointerEventData data)
    {
        ClickTab(Einventype.Mat);
    }


    public void SetGold()
    {
        GetObject((int)Components.Text_Gold).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_Save.User.gold.ToString();
    }
    public void ShowEquipPopup(int itemid)
    {
        if (EquipPopup)
        {
            EquipPopup.gameObject.SetActive(true);
            EquipPopup.SetItem(itemid);
        }
    }

    void ClickTab(Einventype einventype)
    {
        Sound_Click.Play();
        for (int i = 0; i < itemslots.Count; i++)
            itemslots[i].Init(i, einventype);
    }

    //Hover
    public void OnPointDown_WeaponTab(PointerEventData data)
    {
        GetObject((int)Components.Button_WeaponTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointUp_WeaponTab(PointerEventData data)
    {
        GetObject((int)Components.Button_WeaponTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointDown_CostumeTab(PointerEventData data)
    {
        GetObject((int)Components.Button_CostumeTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointUp_CostumeTab(PointerEventData data)
    {
        GetObject((int)Components.Button_CostumeTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointDown_HatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_HatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointUp_HatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_HatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointDown_AccTab(PointerEventData data)
    {
        GetObject((int)Components.Button_AccTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointUp_AccTab(PointerEventData data)
    {
        GetObject((int)Components.Button_AccTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointDown_MatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_MatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointUp_MatTab(PointerEventData data)
    {
        GetObject((int)Components.Button_MatTab).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
}
