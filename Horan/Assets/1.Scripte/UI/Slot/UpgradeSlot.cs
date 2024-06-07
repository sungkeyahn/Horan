using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UpgradeSlot : BaseUI
{
    bool isInit;
    enum Components { Image_NeedMaterialICon , Text_NeedQuantity }

    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        isInit = true;
    }
    public void Init(int itemid,int needamount)
    {
        GetObject((int)Components.Image_NeedMaterialICon).GetComponent<Image>().sprite = Managers.DataLoder.DataCache_Sprite[Managers.DataLoder.DataCache_Items[itemid].iconfilename];
        int i =  Managers.DataLoder.DataCache_Save.Inventory.keys.Find(x => (x.Equals(itemid)));
        if (needamount < Managers.DataLoder.DataCache_Save.Inventory.values[i])
            GetObject((int)Components.Text_NeedQuantity).GetComponent<TMP_Text>().color = Color.red;
        else
            GetObject((int)Components.Text_NeedQuantity).GetComponent<TMP_Text>().color = Color.green;

        GetObject((int)Components.Text_NeedQuantity).GetComponent<TMP_Text>().text = Managers.DataLoder.DataCache_Save.Inventory.values[i].ToString() + "/" + needamount.ToString();
    }
}
