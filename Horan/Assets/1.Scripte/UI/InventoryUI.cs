using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/*
 * ¹«±â : 1001 ~ 1015
 * ¿Ê : 1100 ~ 1112
 * ¸ðÀÚ : 1201 ~ 1212
 * ¾Ç¼¼ : 1301 ~ 1305
 * Àç·á : 1501 ~ 1504
 */
public class InventoryUI : PopupUI
{
    bool isInit = false;
    enum Components { Panel_Stat, Panel_ItemSlots , Button_WeaponTab , Button_CostumeTab, Button_HatTab, Button_AccTab, Button_MatTab, Text_Gold, Button_ClosePopup}
    public List<ItemSlotUI> itemslots=new List<ItemSlotUI>();
    public List<StatSlotUI> statslots = new List<StatSlotUI>();

    public enum EStatSlotInfo { Atk, Hp, Sp, RegenHp }
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

        SetGold();

        isInit = true;
    }


    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        ClosePopupUI();
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
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

    public void SetGold()
    {
        GetObject((int)Components.Text_Gold).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_Save.User.gold.ToString();
    }

}
