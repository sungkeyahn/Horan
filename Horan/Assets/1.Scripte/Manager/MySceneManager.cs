using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySceneManager
{
    public BaseScene CurScene;
    //예외처리 코드 추가 예정
    public void LoadScene(string nextScene)
    {
        LoadingScene.LoadScene(nextScene);
    }

    public void CloseScene()
    { }
}
