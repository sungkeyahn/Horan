using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    public static Managers Instance { get { Init(); return _instance; } }

    DefaultDataLoader dataLoader = new DefaultDataLoader();
    public static DefaultDataLoader DataLoder { get { return Instance.dataLoader; } }

    UIManager uiManager = new UIManager();
    public static UIManager UIManager { get { return Instance.uiManager; } }

    MySceneManager mySceneManager = new MySceneManager();
    public static MySceneManager MySceneManager { get { return Instance.mySceneManager; } }


    static void Init()
    {
        if (_instance == null)
        {   
            GameObject go = GameObject.Find("Manager");
            if (go == null)
            {
                go = new GameObject { name = "Manager" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();

            DataLoder.DefaultDataLoad();
            
        }
    }
    void Awake()
    {
        Init();
    }
}
