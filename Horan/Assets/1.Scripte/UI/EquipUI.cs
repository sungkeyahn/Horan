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
    }
    public void OnBtnClicked_ClosePopup(PointerEventData data)
    {
        Managers.UIManager.GetSceneUI().gameObject.SetActive(true);
        ClosePopupUI(); 
    }
    public void UpdateEquipment()
    {
        //Managers.DataLoder.DataCache_Save.Equipment[(int)Data.EEquipmentType.Accessory];
        // Managers.DataLoder.DataCache_Items[1].iconid; 아이콘 받아서 넣기
        HeadEquipImage.sprite = null;
        ClothesEquipImage.sprite = null;
        WeaponEquipImage.sprite = null;
        AccessoryEquipImage.sprite = null;
    }

}
