using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//구조 변경 점
//0. 세이브 구조를 변경 해야 한다.. -> 이미 모든 아이템에 대한 정보를 가지고 있고 각 인벤 칸에 할당된 아이템정보를 가져오는 형태로 
//1. 
//2. 투명도 조절 필요(수량 없는 아이템 + 장착한 아이템) 투명도 조절 필요
//3.  
//4.

public enum Einventype { Weapon, Costume, Hat, Acc, Mat }
public class ItemSlotUI : BaseUI
{
    bool isinit;
    enum Components { Image_ItemSlot, Button_Use, Button_Close, Text_ItemCount }
    int index = -1;
    Data.DataSet_Item itemdata = null;

    Color a = new Color(1, 1, 1, 1);
    Color b = new Color(1, 1, 1, 0.15f);
    Color c = new Color(1, 1, 1, 0);


    GameObject EquipBtnObject;
    GameObject CloseBtnObject;


    public override void Init()
    {
        if (isinit) return;
        Bind<GameObject>(typeof(Components));
        BindEvent(gameObject, OnClicked_ItemSlot, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Use), OnClicked_Equip, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Close), OnClicked_Close, UIEvent.Click);

        EquipBtnObject = GetObject((int)Components.Button_Use);
        CloseBtnObject = GetObject((int)Components.Button_Close);
        EquipBtnObject.SetActive(false);
        CloseBtnObject.SetActive(false);

        isinit = true;
    }
    public void Init(int slotindex, Einventype type)
    {
        Init();
        itemdata = null;
        index = slotindex;
        int startID = -1;
        switch (type)
        {
            case Einventype.Weapon:
                startID = 1001;
                break;
            case Einventype.Costume:
                startID = 1100;
                break;
            case Einventype.Hat:
                startID = 1200;
                break;
            case Einventype.Acc:
                startID = 1301;
                break;
            case Einventype.Mat:
                startID = 1501;
                break;
        }

        gameObject.GetComponent<Image>().color = c;
        if (Managers.DataLoder.DataCache_Items.ContainsKey(startID + slotindex))
            if (slotindex == Managers.DataLoder.DataCache_Items[startID + slotindex].slotindex)
            {
                itemdata = Managers.DataLoder.DataCache_Items[startID + slotindex];
                SetTransparent();
            }

    }
    public void OnClicked_ItemSlot(PointerEventData data)
    {
        if (itemdata == null) return;
        int i = Managers.DataLoder.DataCache_Save.Inventory.keys.FindIndex(x => x.Equals(itemdata.id)); //수량 
            if (0 < Managers.DataLoder.DataCache_Save.Inventory.values[i])
            {
                UpgradeUI upgrade = GetComponentInParent<UpgradeUI>();
                if (upgrade) 
                    upgrade.SelectItem(itemdata.id);
                else
                {
                    EquipBtnObject.SetActive(true);
                    CloseBtnObject.SetActive(true);
                }
            }
    }
    public void OnClicked_Equip(PointerEventData data)
    {
        Debug.Log(index);
        if (Managers.DataLoder.DataCache_Equipments.ContainsKey(itemdata.id))
        {
            Data.EEquipmentType type = Managers.DataLoder.DataCache_Equipments[itemdata.id].type;
            switch (type)
            {
                case Data.EEquipmentType.Weapon:
                    Managers.DataLoder.DataCache_Save.Equip.weapon = itemdata.id;
                    break;
                case Data.EEquipmentType.Head:
                    Managers.DataLoder.DataCache_Save.Equip.head = itemdata.id;
                    break;
                case Data.EEquipmentType.Clothes:
                    Managers.DataLoder.DataCache_Save.Equip.clothes = itemdata.id;
                    break;
                case Data.EEquipmentType.Accessory:
                    Managers.DataLoder.DataCache_Save.Equip.accessory = itemdata.id;
                    break;
            }
            LobbyCharacterCtrl ctrl = FindObjectOfType<LobbyCharacterCtrl>();
            if (ctrl) ctrl.UpdateEquipments();
        }
    }
    public void OnClicked_Close(PointerEventData data)
    {
        EquipBtnObject.SetActive(false);
        CloseBtnObject.SetActive(false);
    }

    void SetTransparent()
    {
        if (itemdata == null) return;
        if (Managers.DataLoder.DataCache_Sprite.ContainsKey(itemdata.iconfilename)) //아이콘 
            gameObject.GetComponent<Image>().sprite = Managers.DataLoder.DataCache_Sprite[itemdata.iconfilename];

        UpdateItemCount();
    }
    public void UpdateItemCount()
    {
        if (itemdata == null) return;
        int i = Managers.DataLoder.DataCache_Save.Inventory.keys.FindIndex(x => x.Equals(itemdata.id)); //수량 
        Debug.Log(i);
        int count = Managers.DataLoder.DataCache_Save.Inventory.values[i];
        GetObject((int)Components.Text_ItemCount).GetComponent<TMPro.TMP_Text>().text = count.ToString();
        if (0 < count)
            gameObject.GetComponent<Image>().color = a;
        else
            gameObject.GetComponent<Image>().color = b;
        
    }
}
