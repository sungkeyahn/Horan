using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UpgradeSlotUI : BaseUI
{
    bool isInit;
    enum Components { Image_NeedMaterialICon, Text_NeedQuantity }

    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        isInit = true;
    }
    public void Init(int materialID,int needQuantity)
    {
        Init();
        GetObject((int)Components.Image_NeedMaterialICon).GetComponent<Image>().sprite = Managers.DataLoder.DataCache_Sprite[Managers.DataLoder.DataCache_Items[materialID].iconfilename];
        
        int i = Managers.DataLoder.DataCache_Save.Inventory.keys.FindIndex(x => (x.Equals(materialID)));
        if (needQuantity  < Managers.DataLoder.DataCache_Save.Inventory.values[i])
            GetObject((int)Components.Text_NeedQuantity).GetComponent<TMP_Text>().color = Color.green;
        else
            GetObject((int)Components.Text_NeedQuantity).GetComponent<TMP_Text>().color = Color.red;

        GetObject((int)Components.Text_NeedQuantity).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_Save.Inventory.values[i].ToString() + "/" + needQuantity.ToString();
    }

}

