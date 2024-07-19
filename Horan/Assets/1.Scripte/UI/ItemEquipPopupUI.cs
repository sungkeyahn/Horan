using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemEquipPopupUI : PopupUI
{
    bool isInit = false;
    enum Components { Text_ItemName, Text_ItemStatInfo, Image_Item, Button_Equip, Button_Close, Image_Close }
    int ID;
    public override void Init()
    {
        if (isInit) return;

        #region Bind
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Equip), OnClicked_Equip, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Close), OnClicked_Close, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_Equip), OnPointDown_EquipBtn, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_Equip), OnPointUp_EquipBtn, UIEvent.PointUp);

        BindEvent(GetObject((int)Components.Button_Close), OnPointDown_CloseBtn, UIEvent.PointDown); 
        BindEvent(GetObject((int)Components.Button_Close), OnPointUp_CloseBtn, UIEvent.PointUp);
        #endregion Bind

        SetItem(-1);
        isInit = true;
    }
    public void OnClicked_Equip(PointerEventData data)
    {
        if (Managers.DataLoder.DataCache_Equipments.ContainsKey(ID))
        {
            Data.EEquipmentType type = Managers.DataLoder.DataCache_Equipments[ID].type;
            switch (type)
            {
                case Data.EEquipmentType.Weapon:
                    Managers.DataLoder.DataCache_Save.Equip.weapon = ID;
                    break;
                case Data.EEquipmentType.Head:
                    Managers.DataLoder.DataCache_Save.Equip.head = ID;
                    break;
                case Data.EEquipmentType.Clothes:
                    Managers.DataLoder.DataCache_Save.Equip.clothes = ID;
                    break;
                case Data.EEquipmentType.Accessory:
                    Managers.DataLoder.DataCache_Save.Equip.accessory = ID;
                    break;
            }
            LobbyCharacterCtrl ctrl = FindObjectOfType<LobbyCharacterCtrl>();
            if (ctrl) ctrl.UpdateEquipments();
        }
        gameObject.SetActive(false);
    }
    public void OnClicked_Close(PointerEventData data)
    {
        gameObject.SetActive(false);
    }
    public void SetItem(int id)
    {
        if (id < 0) 
        {
            return; 
        }
        ID = id;

        Data.DataSet_Item itemdata=null;
        if (Managers.DataLoder.DataCache_Items.TryGetValue(id,out itemdata))
        {
            Sprite sp = null;
            if (Managers.DataLoder.DataCache_Sprite.TryGetValue(itemdata.iconfilename, out sp))
                GetObject((int)Components.Image_Item).GetComponent<Image>().sprite = sp;
            GetObject((int)Components.Text_ItemName).GetComponent<TMPro.TMP_Text>().text = itemdata.name;
            GetObject((int)Components.Text_ItemStatInfo).GetComponent<TMPro.TMP_Text>().text = itemdata.info;
        }
    }

    private void OnDisable()
    {
        ID = -1;
    }

    //Hover
    public void OnPointDown_EquipBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Equip).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointUp_EquipBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Equip).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointDown_CloseBtn(PointerEventData data)
    {
        GetObject((int)Components.Image_Close).GetComponent<Image>().color = Color.gray;
    }
    public void OnPointUp_CloseBtn(PointerEventData data)
    {
        GetObject((int)Components.Image_Close).GetComponent<Image>().color = Color.white;
    }
}
