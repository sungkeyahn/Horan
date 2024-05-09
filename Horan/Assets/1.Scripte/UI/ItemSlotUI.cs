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

        //������ ������ �ε�

        if (index < Managers.DataLoder.DataCache_Save.Inventory.Count)
        {
            saveditemdata = Managers.DataLoder.DataCache_Save.Inventory[index];
            Debug.Log(saveditemdata.id);
        }
        Debug.Log(index);
        //��������Ʈ �ε�
        //int icon = Managers.DataLoder.DataCache_Items[saveditemdata.id].iconid;
        //��������Ʈ ���ҽ� ĳ�� �����
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
        //���⼭ �����Ҽ� �մ� ���� -> ���� �κ��丮 ���� �ε��� -> ������ id
        //�ؾ��ϴ� ��� -> � ���� ���� + ��� id �˾Ƴ��� �ֱ� 
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
            EquipUI equip = GetComponentInParent<EquipUI>(); //���� �ؾ� �ҵ� ?  ���� ��ü�� 
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
