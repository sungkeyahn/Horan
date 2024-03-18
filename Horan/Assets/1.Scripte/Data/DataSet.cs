using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    //데이터 파일을 키값을 기준으로 분리하여 저장하기 위한 인터페이스
    public interface IDataSeparator<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    #region Stats
    [Serializable] 
    public class Stat_Player
    {
        public int level;
        public int maxHp;
        public int attack;
        public int totalExp;
    }
    [Serializable]
    public class Stat_PlayerDataSeparator : IDataSeparator<int, Stat_Player> //데이터파일을 게임에 로드될 형태로 잘라 하는 클래스
    {
        public List<Stat_Player> playerstats = new List<Stat_Player>(); //데이터 파일의 리스트와 해당 변수의 이름이 동일해야 함

        public Dictionary<int, Stat_Player> MakeDict()
        {
            Dictionary<int, Stat_Player> dict = new Dictionary<int, Stat_Player>();
            foreach (Stat_Player data in playerstats)
                dict.Add(data.level, data);
            return dict;
        }
    }

    [Serializable]
    public class Stat_Monster
    { }

    [Serializable]
    public class Stat_Equipment
    { }
    #endregion

    #region DataSet2 

    #endregion

}
