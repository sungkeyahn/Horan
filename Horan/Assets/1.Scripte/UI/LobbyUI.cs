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

        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Start), OnBtnClicked_Start, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Invetory), OnBtnClicked_Inventory, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Shop), OnBtnClicked_Shop, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Upgrade), OnBtnClicked_Upgrade, UIEvent.Click);
        BindEvent(GetObject((int)Components.Button_Setting), OnBtnClicked_Setting, UIEvent.Click);

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
}

