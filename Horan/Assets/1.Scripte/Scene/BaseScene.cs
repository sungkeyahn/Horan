using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    public string SceneName { get; protected set; }

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Debug.Log("SceneInit");
       // Managers.UIManager.ShowSceneUI<LobbyUI>("LobbyUI");
    }

    public abstract void Clear();
}
