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

        //매니저에서 저장된 아이템 데이터를 받아서 생성할때 인덱스 넣어소 
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
        //해당 장비의 정보를 가져와서 장착
        //인벤에 대한 데이터를 게임 시작할 때 가져와야 함
        //SaveData 관해서 생각 ........ 아이템을 저장하는건 둘째 치고
        //저장된 아이템 데이터를 불러오는것 부터 만들자

    }
    public void OnClicked_Close(PointerEventData data)
    {
        EquipBtnObject.SetActive(false);
        CloseBtnObject.SetActive(false);
    }



}
