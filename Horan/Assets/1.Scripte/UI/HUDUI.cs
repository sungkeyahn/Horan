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
    { Text_Level, Slider_ExpBar, Slider_HpBar, Slider_SpBar, Button_Dash, Button_Atk1, Button_Atk2, Button_Atk3, Button_Defens, Button_Pause, Button_Move, Panel_AbilityICons , Text_QuestResult , Image_MovePad }

    public Action<EPlayerCharacterCtrlEvent> OnCharacterAction;
    public Action<EPlayerCharacterCtrlEvent> OnCharacterEnd;

    PlayerController ctrl;
    PlayerStat stat;

    public Vector3 input; 
    Vector2 MovePadCenterPos;
    RectTransform MovePad;
    RectTransform MoveIever;

    public override void Init()
    {
        if (isinit) return;
        base.Init();
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Pause), OnBtnClicked_Pause, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_Move), OnBtnPointUp_Move, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_Move), OnBtnDraged_Move, UIEvent.Drag); //이동 패드 드래그 

        BindEvent(GetObject((int)Components.Button_Dash), OnBtnClicked_Dash, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Defens), OnBtnClicked_Guard, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_Atk1), OnBtnClicked_FAttack, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Atk2), OnBtnClicked_SAttack, UIEvent.Click);

        MovePad = GetObject((int)Components.Button_Move).GetComponent<RectTransform>();
        MoveIever = GetObject((int)Components.Image_MovePad).GetComponent<RectTransform>();
        MovePadCenterPos = new Vector2(MovePad.anchoredPosition.x/2+ MovePad.rect.width / 2, MovePad.anchoredPosition.y/2 + MovePad.rect.height / 2);

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
        {
            if (Managers.ContentsManager.MonsterCounts <= 0)
                GetObject((int)Components.Text_QuestResult).GetComponent<TMP_Text>().text = "다음 장소로 이동 ";
            else
                GetObject((int)Components.Text_QuestResult).GetComponent<TMP_Text>().text = "남은 적 사살 하기 : " + Managers.ContentsManager.MonsterCounts.ToString();
        }
    }

    public void OnBtnClicked_Pause(PointerEventData data)
    {
      //  Managers.PrefabManager.PlaySound(Managers.PrefabManager.PrefabInstance("Sound_Click"), 1f);
    }

    public void OnBtnDraged_Move(PointerEventData data)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(MovePad, data.position, data.pressEventCamera, out Vector2 localVector))
            this.input = (localVector - MovePadCenterPos).normalized;
        if (OnCharacterAction != null)
            OnCharacterAction.Invoke(EPlayerCharacterCtrlEvent.Move);

        var clampedDir = (localVector - MovePadCenterPos).magnitude < 40 ?
         (localVector - MovePadCenterPos) : (localVector - MovePadCenterPos).normalized * 40;
        MoveIever.anchoredPosition = clampedDir;
        //MoveIever.anchoredPosition = (localVector - MovePadCenterPos);

    }
    public void OnBtnPointUp_Move(PointerEventData data)
    {
        this.input = Vector2.zero;
        if (OnCharacterEnd != null)
            OnCharacterEnd.Invoke(EPlayerCharacterCtrlEvent.Move);

        MoveIever.anchoredPosition = Vector2.zero;
    }

    public void OnBtnClicked_Dash(PointerEventData data)
    {
        if (OnCharacterAction != null)
            OnCharacterAction.Invoke(EPlayerCharacterCtrlEvent.Dash);
    }
    public void OnBtnClicked_Guard(PointerEventData data)
    {
        if (OnCharacterAction != null)
            OnCharacterAction.Invoke(EPlayerCharacterCtrlEvent.Guard);
    }
    public void OnBtnClicked_FAttack(PointerEventData data)
    {
        if (OnCharacterAction != null)
            OnCharacterAction.Invoke(EPlayerCharacterCtrlEvent.FAttack);
    }
    public void OnBtnClicked_SAttack(PointerEventData data)
    {
        if (OnCharacterAction != null)
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