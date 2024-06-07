using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class HUDUI : SceneUI
{
    bool isinit=false;
    enum Components { Text_Level, Slider_ExpBar, Slider_HpBar, Slider_SpBar, Button_Dash, Button_Atk1, Button_Atk2, Button_Atk3, Button_Defens, Button_Pause, Button_Move, Panel_AbilityICons , Text_QuestResult }

    PlayerStat Stat;
    public override void Init()
    {
        if (isinit) return;
        base.Init();
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Dash), OnBtnClicked_Dash, UIEvent.Click);

        Managers.ContentsManager.OnMonsterDead -= UpdateQusetInfo;
        Managers.ContentsManager.OnMonsterSpawn -= UpdateQusetInfo;
        Managers.ContentsManager.OnMonsterDead += UpdateQusetInfo;
        Managers.ContentsManager.OnMonsterSpawn += UpdateQusetInfo;

        for (int i = 0; i < Managers.ContentsManager.AbilityContainer.abilities.Count; i++)
        {
            UpdateAbilityIcon(Managers.ContentsManager.AbilityContainer.abilities[i].Data.id);
        }

        if (Stat == null)
        {
            Stat = FindObjectOfType<PlayerStat>();
            Stat.OnStatChanged += UpdateStat;

            GetObject((int)Components.Text_Level).GetComponent<TMP_Text>().text = $"{Stat.Level}";
            GetObject((int)Components.Slider_HpBar).GetComponent<Slider>().value = Stat.Hp / Stat.MaxHp;  
            GetObject((int)Components.Slider_SpBar).GetComponent<Slider>().value = Stat.Sp / Stat.MaxSp; 
            GetObject((int)Components.Slider_ExpBar).GetComponent<Slider>().value = Stat.Exp / Stat.TotalExp;  
        }
        isinit = true;
    }  

    public void UpdateStat(StatIdentifier identifier, float preValue, float curValue)
    {
        if (Stat == null) return;

        switch (identifier)
        {
            case StatIdentifier.Level:

                GetObject((int)Components.Text_Level).GetComponent<TMP_Text>().text = $"{Stat.Level}";
                break;
            case StatIdentifier.MaxHp:
            case StatIdentifier.Hp:
                GetObject((int)Components.Slider_HpBar).GetComponent<Slider>().value = Stat.Hp / Stat.MaxHp; 
                break;
            case StatIdentifier.MaxSp:
            case StatIdentifier.Sp:
                GetObject((int)Components.Slider_SpBar).GetComponent<Slider>().value = Stat.Sp / Stat.MaxSp; 
                break;
            case StatIdentifier.Exp:
            case StatIdentifier.TotalExp:
                GetObject((int)Components.Slider_ExpBar).GetComponent<Slider>().value = Stat.Exp / Stat.TotalExp;
                break;

        }
    }

    public void UpdateAbilityIcon(int abilityID) 
    {
        GameObject prefab = Resources.Load<GameObject>($"UI/Slot/AbilityIconSlot");
        GameObject ob = Instantiate(prefab, GetObject((int)Components.Panel_AbilityICons).transform);
        ob.name = "AbilityIconSlot";
        ob.GetComponent<AbilityIconSlotUI>().Init(null);

        Data.DataSet_LatentAbility data = null;
        Managers.DataLoder.DataCache_LatentAbility.TryGetValue(abilityID, out data);
        if (Managers.DataLoder.DataCache_Sprite.ContainsKey(data.iconpath))
            ob.GetComponent<AbilityIconSlotUI>().Init(Managers.DataLoder.DataCache_Sprite[data.iconpath]);
    }

    public void UpdateQusetInfo()
    {
        if (GetObject((int)Components.Text_QuestResult))
            GetObject((int)Components.Text_QuestResult).GetComponent<TMP_Text>().text = "남은 적 사살 하기 : " + Managers.ContentsManager.MonsterCounts.ToString();
    }

    public void OnBtnClicked_Dash(PointerEventData data)
    { }

}
