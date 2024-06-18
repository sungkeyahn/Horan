using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    BaseUI title;
    protected override void Init()
    {
        Debug.Log("Enter the Title Scene");
        SceneName = "Title";
        title = Managers.UIManager.ShowSceneUI<TitleUI>("TitleUI");
    }

}
