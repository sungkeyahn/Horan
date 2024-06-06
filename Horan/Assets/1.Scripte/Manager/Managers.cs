using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    ContentsManager contentsManager = new ContentsManager();
    public static ContentsManager ContentsManager { get { return Instance.contentsManager; } }

    const string filename = "SaveData.json";
    string filepath;

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
        filepath = Application.persistentDataPath + "/" + filename; //�ϴ� �ѱ�� 
        Debug.Log(filepath); 
        Init();
        dataLoader.DataCache_Save = SaveFileLoad(filepath);

    }

    void OnApplicationQuit() //���� ����� �ڵ� ����
    {
        SaveFileWrite(filepath);
    }

    Data.SaveData SaveFileLoad(string path)
    {
        //����� ���� �ҷ�����
        if (!File.Exists(path))
        {
            dataLoader.InitSaveData();
            SaveFileWrite(path);
        }
        string ReadJson = File.ReadAllText(path);
        return JsonUtility.FromJson<Data.SaveData>(ReadJson);
    }
    public void SaveFileWrite(string path)
    {
        //������ ���� ���
        string jsondata = JsonUtility.ToJson(dataLoader.DataCache_Save, true);
        File.WriteAllText(path, jsondata);
    }
    public bool SaveFileDelete(string path)
    {
        // ���� ���� ����
        if (File.Exists(path))
        {
            string ReadJson = File.ReadAllText(path);
            File.Delete(ReadJson);
            return true;
        }
        return false;
    }
}
