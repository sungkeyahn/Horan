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

    public void Init(int slotindex)
    {
        Init();
        index = slotindex;

        //�Ŵ������� ����� ������ �����͸� �޾Ƽ� �����Ҷ� �ε��� �־�� 
        //Managers.DataLoder.DataCache_SaveData.inven[index].id;
        //ItemImage.sprite = Managers.DataLoder.DataCache_Weapon[index].animinfo;
       
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
        //�ش� ����� ������ �����ͼ� ����
        //�κ��� ���� �����͸� ���� ������ �� �����;� ��
        //SaveData ���ؼ� ���� ........ �������� �����ϴ°� ��° ġ��
        //����� ������ �����͸� �ҷ����°� ���� ������

    }
    public void OnClicked_Close(PointerEventData data)
    {
        EquipBtnObject.SetActive(false);
        CloseBtnObject.SetActive(false);
    }



}
