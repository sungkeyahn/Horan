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
            resultText.text = "�߰� ����";
            resultText.color = Color.yellow;
        }
        else
        {
            resultText.text = "�߰� ����";
            resultText.color = Color.red;
        }
    }
    void SetInfoText(int enemy,int gold,int mat)
    {
        resultInfoText.text = $"óġ�� ��:         {enemy}���� ��ġ�����ϴ�.\nȹ���� ��:           {gold}�� ���� ȹ�� �߽��ϴ�.\n ���� ���:           {mat}���� ��Ḧ ȹ�� �߽��ϴ�.";
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
