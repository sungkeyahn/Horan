using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GameResultUI : PopupUI
{
    bool isInit=false;
    enum Components { Image_Back,Text_Result, Text_ResultInfo, Button_Exit }
    
    TMP_Text resultText;
    TMP_Text resultInfoText;

    public override void Init()
    {
        if (isInit) return;

        Bind<GameObject>(typeof(Components));
        
        BindEvent(GetObject((int)Components.Button_Exit), OnBtnClicked_CloseBtn, UIEvent.Click);
        
        resultText = GetObject((int)Components.Text_Result).GetComponent<TMP_Text>();
        resultInfoText = GetObject((int)Components.Text_ResultInfo).GetComponent<TMP_Text>();

        isInit = true;
    }
    public void Init(bool isWin)
    {
        Init();
        SetResultText(isWin);
        SetInfoText(Managers.ContentsManager.killcount, Managers.ContentsManager.dropgold, Managers.ContentsManager.AcquiredItems.Count);
    }
    public void OnBtnClicked_CloseBtn(PointerEventData data)
    {
        Managers.UIManager.ClosePopupUI(this);
        Managers.ContentsManager.Resume();
        Managers.MySceneManager.LoadScene("Lobby");
    }
    void SetResultText(bool result)
    {
        if (result)
        {
            resultText.text = "추격 성공";
            resultText.color = Color.yellow;
        }
        else
        {
            resultText.text = "추격 실패";
            resultText.color = Color.red;
        }
    }
    void SetInfoText(int enemy,int gold,int mat)
    {
        resultInfoText.text = $"처치한 적:         {enemy}명을 해치웠습니다.\n획득한 돈:           {gold}의 돈을 획득 했습니다.\n 얻은 재료:           {mat}개의 재료를 획득 했습니다.";
    }
}
/*        GameObject prefab = Resources.Load<GameObject>($"UI/ResultSlotUI");
for (int i = 0; i < Managers.ContentsManager.AcquiredItems.Count; i++)
{
    int id = Managers.ContentsManager.AcquiredItems[i];
    string icon = Managers.DataLoder.DataCache_Items[id].iconfilename;

    GameObject ob = Instantiate(prefab, GetObject((int)Components.Image_Back).transform);
    ob.name = "ResultSlotUI";
    ob.GetComponent<AcquiredItemSlot>().Init(icon);
}*/
