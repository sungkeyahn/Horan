using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDataBind
{
    public void BindData();
}
public class DefaultDataLoader //CoreManager가 단 하나만 가지고 있을 클래스
{
    /* 
     * 추후 데이터 파일을 받아서 적용 시키는 형태로 변경
     * 자신이 어떤 데이터를 받아야하는지 식별자를 정의해야함 (enum형태)
     * 
     * 1.로드가 필요한 데이터 종류 검색  
     * 2.저장된 데이터를 로드
     * 3.데이터 적용
     */

    public Dictionary<int, Data.Stat_Player> playerStatDict { get; private set; } = new Dictionary<int, Data.Stat_Player>();
    
    //예외처리 코드 추가 예정...
    public void DefaultDataLoad()
    {
        playerStatDict = LoadData<Data.Stat_PlayerDataSeparator, int, Data.Stat_Player>("PlayerStatData").MakeDict();
    }

    //예외처리 코드 추가 예정...
    public DataDict LoadData<DataDict, Key, Value>(string DataFileName) where DataDict : Data.IDataSeparator<Key, Value>
    {
        TextAsset textasset = Resources.Load<TextAsset>($"Data/{DataFileName}");
        
        DataDict data = JsonUtility.FromJson<DataDict>(textasset.text);
       
        return JsonUtility.FromJson<DataDict>(textasset.text);
    }

}
