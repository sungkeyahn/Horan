using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class GameResultUI : PopupUI
{
    bool isInit=false;
    enum Components { Image_Back,Text_Result, Button_Exit }
    
    TMP_Text text;
  
    public override void Init()
    {
        if (isInit) return;
        Bind<GameObject>(typeof(Components));
        BindEvent(GetObject((int)Components.Button_Exit), OnBtnClicked_CloseBtn, UIEvent.Click);
        text = GetObject((int)Components.Text_Result).GetComponent<TMP_Text>();
        
        GameObject prefab = Resources.Load<GameObject>($"UI/ResultSlotUI");
        for (int i = 0; i < Managers.ContentsManager.AcquiredItems.Count; i++)
        {
            int id = Managers.ContentsManager.AcquiredItems[i];
            string icon = Managers.DataLoder.DataCache_Items[id].iconfilename;

            GameObject ob = Instantiate(prefab, GetObject((int)Components.Image_Back).transform);
            ob.name = "ResultSlotUI";
            ob.GetComponent<AcquiredItemSlot>().Init(icon);
        }

        isInit = true;
    }
    public void OnBtnClicked_CloseBtn(PointerEventData data)
    {
        Managers.UIManager.ClosePopupUI(this);
        Managers.ContentsManager.Resume();
        Managers.MySceneManager.LoadScene("Lobby");
    }
    public void SetResultText(bool result)
    {
        if (result)
            text.text = "추격 성공";
        else 
            text.text = "추격 실패";
    }
}
