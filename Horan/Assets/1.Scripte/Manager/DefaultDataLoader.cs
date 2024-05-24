using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDataBind
{
    public void BindData();
}
public class DefaultDataLoader //CoreManager가 단 하나만 가지고 있을 클래스
{
    //세이브 파일 캐시
    public Data.SaveData DataCache_Save = new Data.SaveData();

    //데이터 테이블 캐시
    public Dictionary<int, Data.Stat_Player> DataCache_LevelByStat { get; private set; } = new Dictionary<int, Data.Stat_Player>();
    public Dictionary<string, Data.DataSet_Monster> DataCache_Monsters { get; private set; } = new Dictionary<string, Data.DataSet_Monster>();
    public Dictionary<int, Data.DataSet_Group> DataCache_Groups { get; private set; } = new Dictionary<int, Data.DataSet_Group>();
    public Dictionary<int, Data.DataSet_Item> DataCache_Items { get; private set; } = new Dictionary<int, Data.DataSet_Item>();
    public Dictionary<int, Data.DataSet_Equipment> DataCache_Equipments { get; private set; } = new Dictionary<int, Data.DataSet_Equipment>();
    public Dictionary<int, Data.DataSet_Weapon> DataCache_Weapon { get; private set; } = new Dictionary<int, Data.DataSet_Weapon>();
    public Dictionary<int, Data.DataSet_LatentAbility> DataCache_LatentAbility { get; private set; } = new Dictionary<int, Data.DataSet_LatentAbility>();


    //리소스 캐시(추가 예정)
    public Dictionary<string, Sprite> DataCache_Sprite { get; private set; } = new Dictionary<string, Sprite>();
    public Dictionary<string, GameObject> DataCache_Effect { get; private set; } = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> DataCache_Sound { get; private set; } = new Dictionary<string, GameObject>();

    
    //예외처리 코드 추가 예정...
    public void DefaultDataLoad()
    {
        DataCache_LevelByStat = LoadData<Data.Stat_PlayerDataSeparator, int, Data.Stat_Player>("PlayerStatData").MakeDict();
        DataCache_Monsters = LoadData<Data.Separator_MonsterTable, string, Data.DataSet_Monster>("MonsterTable").MakeDict();
        DataCache_Groups = LoadData<Data.Separator_GroupTable, int, Data.DataSet_Group>("GroupTable").MakeDict();
        DataCache_Items = LoadData<Data.Separator_ItemTable, int, Data.DataSet_Item>("ItemTable").MakeDict();
        DataCache_Equipments = LoadData<Data.Separator_EquipmentTable, int, Data.DataSet_Equipment>("EquipmentTable").MakeDict();
        DataCache_Weapon = LoadData<Data.Separator_WeaponTable, int, Data.DataSet_Weapon>("WeaponTable").MakeDict();
        DataCache_LatentAbility= LoadData<Data.Separator_LatentAbilityTable,int,Data.DataSet_LatentAbility>("LatentAbilityTable").MakeDict();

        DataCache_Sprite = LoadSprite();
        DataCache_Effect = LoadPrefab("Effect");
        DataCache_Sound = LoadPrefab("Sound");
    }
    public DataDict LoadData<DataDict, Key, Value>(string DataFileName) where DataDict : Data.IDataSeparator<Key, Value>
    {
        TextAsset textasset = Resources.Load<TextAsset>($"Data/{DataFileName}");
        
        DataDict data = JsonUtility.FromJson<DataDict>(textasset.text);
       
        return JsonUtility.FromJson<DataDict>(textasset.text);
    }
    Dictionary<string, Sprite> LoadSprite()
    {
        Sprite[] Sprites = Resources.LoadAll<Sprite>("Sprite");
        Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();
        foreach (Sprite data in Sprites)
            dict.Add(data.name, data);
        return dict;

    }
    Dictionary<string, GameObject> LoadPrefab(string path)
    {
        GameObject[] objects = Resources.LoadAll<GameObject>(path);
        Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();
        foreach (GameObject data in objects)
            dict.Add(data.name, data);
        return dict;
    }
}
