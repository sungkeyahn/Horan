using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Einventype { Weapon, Costume, Hat, Acc, Mat }
public class ItemSlotUI : BaseUI
{       
    Color a = new(1, 1, 1, 1);
    Color b = new(1, 1, 1, 0.15f);
    Color c = new(1, 1, 1, 0);

    bool isinit;
    enum Components { Image_ItemSlot, Button_Use, Button_Close, Text_ItemCount }
    Data.DataSet_Item itemdata = null;

    public override void Init()
    {
        if (isinit) return;
        Bind<GameObject>(typeof(Components));
        BindEvent(gameObject, OnClicked_ItemSlot, UIEvent.Click);
        isinit = true;
    }
    public void Init(int slotindex, Einventype type)
    {
        Init();
        itemdata = null;
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

        if (Managers.DataLoder.DataCache_Items.TryGetValue(startID + slotindex, out itemdata))
            SetItemSlot(slotindex);
        else
            SetItemSlot(-1);
    }
    void SetItemSlot(int slotindex)
    {
        if (slotindex < 0)
        {
            gameObject.GetComponent<Image>().color = c;
            GetObject((int)Components.Text_ItemCount).GetComponent<TMPro.TMP_Text>().text = "";
            return;
        }
        if (slotindex == itemdata.slotindex)
        {
            if (Managers.DataLoder.DataCache_Sprite.ContainsKey(itemdata.iconfilename)) //아이콘 
                gameObject.GetComponent<Image>().sprite = Managers.DataLoder.DataCache_Sprite[itemdata.iconfilename];

            int i = Managers.DataLoder.DataCache_Save.Inventory.keys.FindIndex(x => x.Equals(itemdata.id)); //수량 
            int count = Managers.DataLoder.DataCache_Save.Inventory.values[i];
            GetObject((int)Components.Text_ItemCount).GetComponent<TMPro.TMP_Text>().text = count.ToString();
            if (0 < count)
                gameObject.GetComponent<Image>().color = a;
            else
                gameObject.GetComponent<Image>().color = b;
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
            {
                upgrade.SelectItem(itemdata.id);
                return;
            }

            InventoryUI inven= GetComponentInParent<InventoryUI>();
            if (inven)
            {
                inven.ShowEquipPopup(itemdata.id); 
                return;
            } 
        }   
    }

}
