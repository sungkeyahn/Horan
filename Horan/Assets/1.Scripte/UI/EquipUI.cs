using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipUI : PopupUI
{
    enum Components { Button_ClosePopup, Image_Head, Image_Clothes, Image_Weapon, Image_Accessory }

    Image HeadEquipImage;
    Image ClothesEquipImage;
    Image WeaponEquipImage;
    Image AccessoryEquipImage;

    public override void Init()
    {
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_ClosePopup), OnBtnClicked_ClosePopup, UIEvent.Click);

        HeadEquipImage = GetObject((int)Components.Image_Head).GetComponent<Image>();
        ClothesEquipImage = GetObject((int)Components.Image_Clothes).GetComponent<Image>(); 
        WeaponEquipImage = GetObject((int)Components.Image_Weapon).GetComponent<Image>(); 
        AccessoryEquipImage = GetObject((int)Components.Image_Accessory).GetComponent<Image>();

        UpdateEquipment();

        //인벤 슬롯 추가 [동적] 

        ItemSlotUI slotUI = GetComponentInChildren<ItemSlotUI>();
        slotUI.Init(1);
    }

    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
        ClosePopupUI(); 
    }
    public void UpdateEquipment()
    {
        string weaponicon="";
        string headicon = "";
        string clothesicon = "";
        string accessoryicon = "";
        if (Managers.DataLoder.DataCache_Items.ContainsKey(Managers.DataLoder.DataCache_Save.Equip.weapon))
            weaponicon = Managers.DataLoder.DataCache_Items[Managers.DataLoder.DataCache_Save.Equip.weapon].iconfilename;
        if (Managers.DataLoder.DataCache_Items.ContainsKey(Managers.DataLoder.DataCache_Save.Equip.head))
             headicon = Managers.DataLoder.DataCache_Items[Managers.DataLoder.DataCache_Save.Equip.head].iconfilename;
        if (Managers.DataLoder.DataCache_Items.ContainsKey(Managers.DataLoder.DataCache_Save.Equip.clothes))
             clothesicon = Managers.DataLoder.DataCache_Items[Managers.DataLoder.DataCache_Save.Equip.clothes].iconfilename;
        if (Managers.DataLoder.DataCache_Items.ContainsKey(Managers.DataLoder.DataCache_Save.Equip.accessory))
             accessoryicon = Managers.DataLoder.DataCache_Items[Managers.DataLoder.DataCache_Save.Equip.accessory].iconfilename;

        if (Managers.DataLoder.DataCache_Sprite.ContainsKey(weaponicon))
            WeaponEquipImage.sprite  = Managers.DataLoder.DataCache_Sprite[weaponicon];
        if (Managers.DataLoder.DataCache_Sprite.ContainsKey(headicon))
            HeadEquipImage.sprite = Managers.DataLoder.DataCache_Sprite[headicon];
        if (Managers.DataLoder.DataCache_Sprite.ContainsKey(clothesicon))
            ClothesEquipImage.sprite = Managers.DataLoder.DataCache_Sprite[clothesicon];
        if (Managers.DataLoder.DataCache_Sprite.ContainsKey(accessoryicon))
            AccessoryEquipImage.sprite = Managers.DataLoder.DataCache_Sprite[accessoryicon];
    }

}
