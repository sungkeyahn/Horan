using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatSlotUI : BaseUI
{
    bool isinit = false;
    enum Components { Image_StatIcon, Text_StatInfo }

    InventoryUI.EStatSlotInfo info;
    string statname = null;
    Sprite sprite = null;
    float val = 0;

    public override void Init()
    {
        if (isinit) return;
        Bind<GameObject>(typeof(Components));
        isinit = true;
    }
    public void Init(InventoryUI.EStatSlotInfo Info)
    {
        Init();
        info = Info;
        UpdateStatSlot();
    }
    public void UpdateStatSlot()
    {
        PlayerStat stat = FindObjectOfType<PlayerStat>();
        switch (info)
        {
            case InventoryUI.EStatSlotInfo.Atk:
                statname = "공격력";
                //sprite = Managers.DataLoder.DataCache_Sprite["icon_statslot_attack"];
                val = stat.Attack;
                break;
            case InventoryUI.EStatSlotInfo.Hp:
                statname = "체력";
                //sprite = Managers.DataLoder.DataCache_Sprite["icon_statslot_hp"];
                val = stat.MaxHp;
                break;
            case InventoryUI.EStatSlotInfo.Sp:
                statname = "기력";
                //sprite = Managers.DataLoder.DataCache_Sprite["icon_statslot_sp"];
                val = stat.MaxSp;
                break;
            case InventoryUI.EStatSlotInfo.RegenHp:
                statname = "체력 재생";
                //sprite = Managers.DataLoder.DataCache_Sprite["Icon_StatSlot_RegenHp"];
                val = stat.HpRegenAmount;
                break;
        }

        GetObject((int)Components.Image_StatIcon).GetComponent<Image>().sprite = sprite;
        GetObject((int)Components.Text_StatInfo).GetComponent<TMP_Text>().text = statname + $": {val}";
    }
}
