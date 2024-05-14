using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    [SerializeField]
    public string SceneName;
    [SerializeField]
    public string NextSceneName;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Debug.Log("SceneInit");
    }


}
