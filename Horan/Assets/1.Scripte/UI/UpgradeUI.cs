using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class UpgradeUI : PopupUI
{
    bool isInit;
    enum Components { Button_ClosePopup, Panel_Stat, Panel_NeedMats, Panel_ItemSlots, Text_NeedGold, Image_ResultItemICon, Button_Upgrade , Button_WeaponTab, Button_CostumeTab, Button_HatTab, Button_AccTab, Button_MatTab, Text_Gold }

    public List<ItemSlotUI> itemslots = new List<ItemSlotUI>();
    public List<StatSlotUI> statSlotUIs = new List<StatSlotUI>();
    public List<UpgradeSlotUI> upgradeSlotUIs = new List<UpgradeSlotUI>();

    AudioSource Sound_Click;
    AudioSource Sound_Upgrade;

    int ID;
    Einventype inventype = Einventype.Weapon;

    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_ClosePopup), OnBtnClicked_ClosePopup, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Upgrade), OnBtnClicked_Upgrade, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_WeaponTab), OnBtnClicked_WeaponTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_CostumeTab), OnBtnClicked_CostumeTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_HatTab), OnBtnClicked_HatTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_AccTab), OnBtnClicked_AccTab, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_MatTab), OnBtnClicked_MatTab, UIEvent.Click);
        
        Sound_Click = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Click"], transform).GetComponent<AudioSource>();
        Sound_Upgrade = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Upgrade"], transform).GetComponent<AudioSource>();

        GameObject prefab_ItemSlotUI = Resources.Load<GameObject>($"UI/Slot/ItemSlot");
        for (int i = 0; i < 20; i++)
        {
            GameObject ob = Instantiate(prefab_ItemSlotUI, GetObject((int)Components.Panel_ItemSlots).transform);
            ob.name = "ItemSlot";
            itemslots.Add(ob.GetComponent<ItemSlotUI>());
            ob.GetComponent<ItemSlotUI>().Init(i, Einventype.Weapon);
        }
        SetGold();
        isInit = true;
    }
    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        Sound_Click.Play();
        ClosePopupUI();
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
    }
    public void OnBtnClicked_Upgrade(PointerEventData data)
    {
        if (!Managers.DataLoder.DataCache_Upgrade.ContainsKey(ID)) return;
       
        bool isUpgradePossible = true;

        float totalGold = Managers.DataLoder.DataCache_Save.User.gold; // 필요 재화 양 
        float needGold = Managers.DataLoder.DataCache_Upgrade[ID].needgold;
        if (totalGold < needGold) isUpgradePossible = false;
        
        for (int i = 0; i < Managers.DataLoder.DataCache_Upgrade[ID].materials.Count; i++)
        {
            int x = Managers.DataLoder.DataCache_Save.Inventory.keys.FindIndex(x => (x.Equals(Managers.DataLoder.DataCache_Upgrade[ID].materials[i].id)));
            if (Managers.DataLoder.DataCache_Save.Inventory.values[x] < Managers.DataLoder.DataCache_Upgrade[ID].materials[i].amount)
            {
                isUpgradePossible = false;
                break;
            }
        }

        if (isUpgradePossible)
        {
            //재료 +  재화 소모 
            Managers.ContentsManager.ConsumeGold(needGold);
            for (int i = 0; i < Managers.DataLoder.DataCache_Upgrade[ID].materials.Count; i++)
                Managers.ContentsManager.RemoveItem(Managers.DataLoder.DataCache_Upgrade[ID].materials[i].id, Managers.DataLoder.DataCache_Upgrade[ID].materials[i].amount);

            //아이템 강화 습득
            Managers.ContentsManager.AddItem(Managers.DataLoder.DataCache_Upgrade[ID].resultitemid);

            //갱신
            for (int i = 0; i < itemslots.Count; i++)
                itemslots[i].Init(i, inventype);
            SetGold();

            Sound_Upgrade.Play();
        }
    }
    public void OnBtnClicked_WeaponTab(PointerEventData data)
    {
        TabClick(Einventype.Weapon);
    }
    public void OnBtnClicked_CostumeTab(PointerEventData data)
    {
        TabClick(Einventype.Costume);
    }
    public void OnBtnClicked_HatTab(PointerEventData data)
    {
        TabClick(Einventype.Hat);
    }
    public void OnBtnClicked_AccTab(PointerEventData data)
    {
        TabClick(Einventype.Acc);
    }
    public void OnBtnClicked_MatTab(PointerEventData data)
    {
        TabClick(Einventype.Mat);
    }
    public void TabClick(Einventype type)
    {
        Sound_Click.Play();
        inventype = type;
        for (int i = 0; i < itemslots.Count; i++)
            itemslots[i].Init(i, type);
    }
    public void SetGold()
    {
        GetObject((int)Components.Text_Gold).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_Save.User.gold.ToString();
    }
    void UpdateStatSlot(int id)
    {
        for (int i = 0; i < GetObject((int)Components.Panel_Stat).transform.childCount; i++)
        {
            if (GetObject((int)Components.Panel_Stat).transform.GetChild(i))
                Destroy(GetObject((int)Components.Panel_Stat).transform.GetChild(i).gameObject);
        }
        statSlotUIs.Clear();
        GameObject prefab_StatSlotUI = Resources.Load<GameObject>($"UI/Slot/StatSlot");
        for (int i = 0; i < Managers.DataLoder.DataCache_Equipments[id].abilitys.Count; i++)
        {
            GameObject ob = Instantiate(prefab_StatSlotUI, GetObject((int)Components.Panel_Stat).transform);
            ob.name = "StatSlotUI";
            statSlotUIs.Add(ob.GetComponent<StatSlotUI>());
            switch (Managers.DataLoder.DataCache_Equipments[id].abilitys[i].type)
            {
                case Data.EEquipmentAbilityType.MaxHp:
                    ob.GetComponent<StatSlotUI>().Init(InventoryUI.EStatSlotInfo.Hp);
                    break;
                case Data.EEquipmentAbilityType.MaxSp:
                    ob.GetComponent<StatSlotUI>().Init(InventoryUI.EStatSlotInfo.Sp);
                    break;
                case Data.EEquipmentAbilityType.AttackDamage:
                    ob.GetComponent<StatSlotUI>().Init(InventoryUI.EStatSlotInfo.Atk);
                    break;
                case Data.EEquipmentAbilityType.CriticalProbability:
                    ob.GetComponent<StatSlotUI>().Init(InventoryUI.EStatSlotInfo.RegenHp);
                    break;
            }
        }
    }
    void UpdateUpgradeSlot(int id)
    {
        for (int i = 0; i < GetObject((int)Components.Panel_NeedMats).transform.childCount; i++)
        {
            if (GetObject((int)Components.Panel_NeedMats).transform.GetChild(i))
                Destroy(GetObject((int)Components.Panel_NeedMats).transform.GetChild(i).gameObject);
        }
        upgradeSlotUIs.Clear();
        GameObject prefab_UpgradeSlotUI = Resources.Load<GameObject>($"UI/Slot/UpgradeSlot");
        for (int i = 0; i < Managers.DataLoder.DataCache_Upgrade[id].materials.Count; i++)
        {
            GameObject ob = Instantiate(prefab_UpgradeSlotUI, GetObject((int)Components.Panel_NeedMats).transform);
            ob.name = "UpgradeSlotUI";
            upgradeSlotUIs.Add(ob.GetComponent<UpgradeSlotUI>());
            ob.GetComponent<UpgradeSlotUI>().Init(Managers.DataLoder.DataCache_Upgrade[id].materials[i].id,Managers.DataLoder.DataCache_Upgrade[id].materials[i].amount);
        }
    }
    public void SelectItem(int selectedInvenItemID=-1)
    {
        if (!Managers.DataLoder.DataCache_Upgrade.ContainsKey(selectedInvenItemID)) return;

        Sound_Click.Play();
        ID = selectedInvenItemID;
        
        UpdateStatSlot(selectedInvenItemID);  // StatSlot  생성
        UpdateUpgradeSlot(selectedInvenItemID); // UpgradeSlot 생성

        string path = Managers.DataLoder.DataCache_Items[Managers.DataLoder.DataCache_Upgrade[selectedInvenItemID].resultitemid].iconfilename;
        GetObject((int)Components.Image_ResultItemICon).GetComponent<Image>().sprite = Managers.DataLoder.DataCache_Sprite[path]; //결과 아이템 아이콘
        GetObject((int)Components.Image_ResultItemICon).GetComponent<Image>().color = Color.white;

        float totalGold = Managers.DataLoder.DataCache_Save.User.gold; // 필요 재화 양 
        float needGold = Managers.DataLoder.DataCache_Upgrade[selectedInvenItemID].needgold;
        GetObject((int)Components.Text_NeedGold).GetComponent<TMP_Text>().text = $"{needGold}";
        if (totalGold < needGold) GetObject((int)Components.Text_NeedGold).GetComponent<TMP_Text>().color = Color.red;
        else GetObject((int)Components.Text_NeedGold).GetComponent<TMP_Text>().color = Color.green;

    }

}
