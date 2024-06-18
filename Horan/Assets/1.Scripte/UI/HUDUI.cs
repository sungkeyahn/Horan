using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

public enum EPlayerCharacterCtrlEvent
{
    Move,Dash,Guard,FAttack,SAttack
}
public class HUDUI : SceneUI
{
    bool isinit=false;
    enum Components 
    { Text_Level, Slider_ExpBar, Slider_HpBar, Slider_SpBar, Button_Dash, Button_Atk1, Button_Atk2, Button_Atk3, Button_Defens, Button_Pause, Button_Move, Panel_AbilityICons , Text_QuestResult }

    public Action<EPlayerCharacterCtrlEvent> OnCharacterAction;

    PlayerController ctrl;
    PlayerStat stat;
    public override void Init()
    {
        if (isinit) return;
        base.Init();
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Pause), OnBtnClicked_Pause, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_Move), OnBtnClicked_Move, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Move), OnBtnDraged_Move, UIEvent.Drag); //이동 패드 드래그 

        BindEvent(GetObject((int)Components.Button_Dash), OnBtnClicked_Dash, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Defens), OnBtnClicked_Dash, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_Atk1), OnBtnClicked_Dash, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Atk2), OnBtnClicked_Dash, UIEvent.Click);

        isinit = true;
    }
    public void Init(PlayerController Ctrl)
    {
        Init();
        Managers.ContentsManager.OnMonsterDead -= UpdateQusetInfo;
        Managers.ContentsManager.OnMonsterSpawn -= UpdateQusetInfo;
        Managers.ContentsManager.AbilityContainer.OnAbilityUpdate -= AddAbilityIcon;

        Managers.ContentsManager.OnMonsterDead += UpdateQusetInfo;
        Managers.ContentsManager.OnMonsterSpawn += UpdateQusetInfo;
        Managers.ContentsManager.AbilityContainer.OnAbilityUpdate += AddAbilityIcon;
        UpdateAbilityIcon();

        ctrl = Ctrl;
        if (ctrl)
        {
            stat = ctrl.GetComponent<PlayerStat>();
            if (stat != null)
            {
                stat.OnStatChanged += UpdateStat;
                UpdateStat(StatIdentifier.Hp);
                UpdateStat(StatIdentifier.Sp);
                UpdateStat(StatIdentifier.Level);
                UpdateStat(StatIdentifier.Exp);
            }
        }

    }

    void UpdateStat(StatIdentifier identifier, float preValue=0, float curValue=0)
    {
        if (stat == null) return;

        switch (identifier)
        {
            case StatIdentifier.Level:
                GetObject((int)Components.Text_Level).GetComponent<TMP_Text>().text = $"{stat.Level}";
                break;
            case StatIdentifier.MaxHp:
            case StatIdentifier.Hp:
                GetObject((int)Components.Slider_HpBar).GetComponent<Slider>().value = stat.Hp / stat.MaxHp; 
                break;
            case StatIdentifier.MaxSp:
            case StatIdentifier.Sp:
                GetObject((int)Components.Slider_SpBar).GetComponent<Slider>().value = stat.Sp / stat.MaxSp; 
                break;
            case StatIdentifier.Exp:
            case StatIdentifier.TotalExp:
                GetObject((int)Components.Slider_ExpBar).GetComponent<Slider>().value = stat.Exp / stat.TotalExp;
                break;
        }
    }

    void UpdateAbilityIcon()//int abilityID
    {
        GameObject prefab; 
        GameObject ob;
        Data.DataSet_LatentAbility data = null;
        for (int i = 0; i < Managers.ContentsManager.AbilityContainer.abilities.Count; i++)
        {
            prefab = Resources.Load<GameObject>($"UI/Slot/AbilityIconSlot");
            ob = Instantiate(prefab, GetObject((int)Components.Panel_AbilityICons).transform);
            ob.name = "AbilityIconSlot"; 
            Managers.DataLoder.DataCache_LatentAbility.TryGetValue(Managers.ContentsManager.AbilityContainer.abilities[i].Data.id, out data);
            if (Managers.DataLoder.DataCache_Sprite.ContainsKey(data.iconpath))
                ob.GetComponent<AbilityIconSlotUI>().Init(Managers.DataLoder.DataCache_Sprite[data.iconpath]);
        }
    }
    void AddAbilityIcon(int id)
    {
        Data.DataSet_LatentAbility data = null;
        GameObject  prefab = Resources.Load<GameObject>($"UI/Slot/AbilityIconSlot");
        GameObject ob = Instantiate(prefab, GetObject((int)Components.Panel_AbilityICons).transform);
        ob.name = "AbilityIconSlot";
        Managers.DataLoder.DataCache_LatentAbility.TryGetValue(id, out data);
        if (Managers.DataLoder.DataCache_Sprite.ContainsKey(data.iconpath))
            ob.GetComponent<AbilityIconSlotUI>().Init(Managers.DataLoder.DataCache_Sprite[data.iconpath]);
    }

    void UpdateQusetInfo()
    {
        if (GetObject((int)Components.Text_QuestResult))
            GetObject((int)Components.Text_QuestResult).GetComponent<TMP_Text>().text = "남은 적 사살 하기 : " + Managers.ContentsManager.MonsterCounts.ToString();
    }

    public void OnBtnClicked_Pause(PointerEventData data)
    {
        Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
    }


    //플레이어 조작  클릭 시리즈는 각 버튼에 알맞는 이벤트를 뿌려주고 해당 버튼이미지처리 만 해주기
    public void OnBtnClicked_Move(PointerEventData data)
    {
    }
    public void OnBtnDraged_Move(PointerEventData data)
    {
        OnCharacterAction.Invoke(EPlayerCharacterCtrlEvent.Move);
    }
    public void OnBtnClicked_Dash(PointerEventData data)
    {
        OnCharacterAction.Invoke(EPlayerCharacterCtrlEvent.Dash);
    }
    public void OnBtnClicked_Guard(PointerEventData data)
    {
        OnCharacterAction.Invoke(EPlayerCharacterCtrlEvent.Guard);
    }
    public void OnBtnClicked_FAttack(PointerEventData data)
    {
        OnCharacterAction.Invoke(EPlayerCharacterCtrlEvent.FAttack);
    }
    public void OnBtnClicked_SAttack(PointerEventData data)
    {
        OnCharacterAction.Invoke(EPlayerCharacterCtrlEvent.SAttack);
    }


}

/*
 * HUD 에서는 각 버튼 클릭에 해당하는 이벤트를  pctrl에 뿌려줌
 * -> pctrl에서는 받은 이벤트에 해당하는 함수를 호출하기 이전에 현재 입력 상태에 대하여 
 * 고려하여 알맞는 함수를 호출
 */
/*  GetObject((int)Components.Text_Level).GetComponent<TMP_Text>().text = $"{stat.Level}";
            GetObject((int)Components.Slider_HpBar).GetComponent<Slider>().value = stat.Hp / stat.MaxHp;
            GetObject((int)Components.Slider_SpBar).GetComponent<Slider>().value = stat.Sp / stat.MaxSp;
            GetObject((int)Components.Slider_ExpBar).GetComponent<Slider>().value = stat.Exp / stat.TotalExp;*/
/*        GameObject prefab = Resources.Load<GameObject>($"UI/Slot/AbilityIconSlot");
GameObject ob = Instantiate(prefab, GetObject((int)Components.Panel_AbilityICons).transform);
ob.name = "AbilityIconSlot";
ob.GetComponent<AbilityIconSlotUI>().Init(null);

Data.DataSet_LatentAbility data = null;
Managers.DataLoder.DataCache_LatentAbility.TryGetValue(abilityID, out data);
if (Managers.DataLoder.DataCache_Sprite.ContainsKey(data.iconpath))
    ob.GetComponent<AbilityIconSlotUI>().Init(Managers.DataLoder.DataCache_Sprite[data.iconpath]);*/