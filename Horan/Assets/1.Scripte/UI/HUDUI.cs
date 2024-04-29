using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class HUDUI : SceneUI
{
    bool isinit=false;
    enum Components { Text_Level, Slider_ExpBar, Slider_HpBar, Slider_SpBar, Button_Dash, Button_Atk1, Button_Atk2, Button_Atk3, Button_Defens, Button_Pause, Button_MoveBack, Toggle_Ability }

    PlayerStat Stat;
    public override void Init()
    {
        if (isinit) return;
        base.Init();
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Dash), OnBtnClicked_Dash, UIEvent.Click);

        if (Stat == null)
        {
            Stat = FindObjectOfType<PlayerStat>();
            Stat.OnStatChanged += UpdateStat;

            SetLevelText(Stat.Level);
            SetExpBar(Stat.Exp/Stat.TotalExp);
            SetHpBar(Stat.Hp/Stat.MaxHp);
            SetSpBar(Stat.Sp / Stat.MaxSp);
        }

        isinit = true;
    }

    public void UpdateStat(StatIdentifier identifier, float preValue, float curValue)
    {
        if (Stat == null) return;

        switch (identifier)
        {
            case StatIdentifier.Level:
                SetLevelText(Stat.Level);
                break;
            case StatIdentifier.MaxHp:
            case StatIdentifier.Hp:
                SetHpBar(Stat.Hp / Stat.MaxHp);
                break;
            case StatIdentifier.MaxSp:
            case StatIdentifier.Sp:
                SetSpBar(Stat.Sp / Stat.MaxSp);
                break;
            case StatIdentifier.Exp:
            case StatIdentifier.TotalExp:
                SetExpBar(Stat.Exp / Stat.TotalExp);
                break;

        }
    }
    void SetLevelText(int level)
    {
        GetObject((int)Components.Text_Level).GetComponent<TMP_Text>().text = $"{level}";
    }
    void SetExpBar(float ratio)
    {
        GetObject((int)Components.Slider_ExpBar).GetComponent<Slider>().value = ratio;
    }
    void SetHpBar(float ratio)
    {
        GetObject((int)Components.Slider_HpBar).GetComponent<Slider>().value = ratio;
    }
    void SetSpBar(float ratio)
    {
        GetObject((int)Components.Slider_SpBar).GetComponent<Slider>().value = ratio;
    }

    public void OnBtnClicked_Dash(PointerEventData data)
    { }

}
