using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class BossHPBarUI : PopupUI
{
    bool isInit = false;
    enum Components { HPBar }
    MonsterStat stat;
    Slider slider;
    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        slider = GetObject((int)Components.HPBar).GetComponent<Slider>();
        isInit = true;
    }
    public void Init(MonsterStat stat,string name)
    {
        Init();
        if (stat)
        {
            this.stat = stat;
            stat.OnStatChanged+= SetHPRatio;
            transform.SetParent(stat.gameObject.transform);
        }

        SetHPRatio(StatIdentifier.Hp,1,1);
    }
    void SetHPRatio(StatIdentifier identifier,float pre ,float current)
    {
       if (identifier == StatIdentifier.Hp)
            slider.value = (float)stat.hp / stat.maxhp;
    }
    void OnDestroy()
    {
        if (stat) stat.OnStatChanged -= SetHPRatio;
    }
}
