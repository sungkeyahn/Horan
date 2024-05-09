using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : BaseUI
{
    enum Components { Image_ItemSlot, Button_Equip, Button_Close }
    public int index { get; private set; }
    Image ItemImage; 
    GameObject EquipBtnObject;
    GameObject CloseBtnObject;
    Data.Save_ItemSlot saveditemdata;

    public void Init(int slotindex)
    {
        index = slotindex;
        Init();

        //아이템 데이터 로드

        if (index < Managers.DataLoder.DataCache_Save.Inventory.Count)
        {
            saveditemdata = Managers.DataLoder.DataCache_Save.Inventory[index];
            Debug.Log(saveditemdata.id);
        }
        Debug.Log(index);
        //스프라이트 로드
        //int icon = Managers.DataLoder.DataCache_Items[saveditemdata.id].iconid;
        //스프라이트 리소스 캐시 만들기
        //ItemImage.sprite = Managers.DataLoder.DataCache_Icon[icon];
    }
    public override void Init()
    {
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Image_ItemSlot), OnClicked_ItemSlot, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Equip), OnClicked_Equip, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Close), OnClicked_Close, UIEvent.Click);
       
        ItemImage = GetImage((int)Components.Image_ItemSlot);

        EquipBtnObject = GetObject((int)Components.Button_Equip);
        CloseBtnObject = GetObject((int)Components.Button_Close);

        EquipBtnObject.SetActive(false);
        CloseBtnObject.SetActive(false);
    }

    public void OnClicked_ItemSlot(PointerEventData data)
    {
        EquipBtnObject.SetActive(true);
        CloseBtnObject.SetActive(true);
    }
    public void OnClicked_Equip(PointerEventData data)
    {
        //여기서 접근할수 잇는 정보 -> 나의 인벤토리 슬롯 인덱스 -> 아이템 id
        //해야하는 기능 -> 어떤 부위 인지 + 장비 id 알아내서 넣기 
        if (Managers.DataLoder.DataCache_Equipments.ContainsKey(saveditemdata.id))
        {
            Data.EEquipmentType type = Managers.DataLoder.DataCache_Equipments[saveditemdata.id].type;
            switch (type)
            {
                case Data.EEquipmentType.Weapon:
                    Managers.DataLoder.DataCache_Save.Equip.weapon = saveditemdata.id;
                    break;
                case Data.EEquipmentType.Head:
                    Managers.DataLoder.DataCache_Save.Equip.head = saveditemdata.id;
                    break;
                case Data.EEquipmentType.Clothes:
                    Managers.DataLoder.DataCache_Save.Equip.clothes = saveditemdata.id;
                    break;
                case Data.EEquipmentType.Accessory:
                    Managers.DataLoder.DataCache_Save.Equip.accessory = saveditemdata.id;
                    break;
            }
            EquipUI equip = GetComponentInParent<EquipUI>(); //개선 해야 할듯 ?  구조 자체를 
            if (equip)
                equip.UpdateEquipment();
        }
    }
    public void OnClicked_Close(PointerEventData data)
    {
        EquipBtnObject.SetActive(false);
        CloseBtnObject.SetActive(false);
    }



}
