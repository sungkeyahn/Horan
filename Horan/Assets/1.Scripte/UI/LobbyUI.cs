using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class LobbyUI : SceneUI
{
    bool isinit=false;
    enum Components { Button_Start , Button_Invetory , Button_Shop, Button_Upgrade, Button_Setting }
    
    AudioSource Sound_Click;

    public override void Init()
    {
        if (isinit) return;
        base.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
#region Bind
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Start), OnBtnClicked_Start, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Invetory), OnBtnClicked_Inventory, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Shop), OnBtnClicked_Shop, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Upgrade), OnBtnClicked_Upgrade, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Setting), OnBtnClicked_Setting, UIEvent.Click);

        BindEvent(GetObject((int)Components.Button_Start), OnPointDown_StartBtn, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_Invetory), OnPointDown_InvetoryBtn, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_Shop), OnPointDown_ShopBtn, UIEvent.PointDown);
        BindEvent(GetObject((int)Components.Button_Upgrade), OnPointDown_UpgradeBtn, UIEvent.PointDown);

        BindEvent(GetObject((int)Components.Button_Start), OnPointUp_StartBtn, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_Invetory), OnPointUp_InvetoryBtn, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_Shop), OnPointUp_ShopBtn, UIEvent.PointUp);
        BindEvent(GetObject((int)Components.Button_Upgrade), OnPointUp_UpgradeBtn, UIEvent.PointUp);
        #endregion Bind
        Sound_Click = Instantiate(Managers.DataLoder.DataCache_Sound["Sound_Click"]).GetComponent<AudioSource>();
        

        isinit = true;
    }
    public void OnBtnClicked_Start(PointerEventData data)
    {
        Sound_Click.Play();
        Managers.ContentsManager.StageSelect(1);
    }
    public void OnBtnClicked_Inventory(PointerEventData data)
    {        
       Sound_Click.Play();
       Managers.UIManager.ShowPopupUI<InventoryUI>("InventoryUI");
       Managers.UIManager.GetSceneUI().gameObject.SetActive(false);
    }
    public void OnBtnClicked_Shop(PointerEventData data)
    {
        Sound_Click.Play();
        Managers.UIManager.ShowPopupUI<ShopUI>("ShopUI");
        Managers.UIManager.GetSceneUI().gameObject.SetActive(false);
    }
    public void OnBtnClicked_Upgrade(PointerEventData data)
    {
        Sound_Click.Play();
        Managers.UIManager.ShowPopupUI<UpgradeUI>("UpgradeUI");
        Managers.UIManager.GetSceneUI().gameObject.SetActive(false);
    }
    public void OnBtnClicked_Setting(PointerEventData data)
    {

        Debug.Log("Setting");
        Application.Quit();
    }



    //Hover
    public void OnPointDown_StartBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Start).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_InvetoryBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Invetory).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_ShopBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Shop).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointDown_UpgradeBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Upgrade).GetComponentInChildren<TMPro.TMP_Text>().color = Color.gray;
    }
    public void OnPointUp_StartBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Start).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_InvetoryBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Invetory).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_ShopBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Shop).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }
    public void OnPointUp_UpgradeBtn(PointerEventData data)
    {
        GetObject((int)Components.Button_Upgrade).GetComponentInChildren<TMPro.TMP_Text>().color = Color.white;
    }



}

