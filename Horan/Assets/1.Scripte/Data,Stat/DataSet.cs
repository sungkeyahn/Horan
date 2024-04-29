using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    //������ ������ Ű���� �������� �и��Ͽ� �����ϱ� ���� �������̽�
    public interface IDataSeparator<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    #region DataSet
    [Serializable] 
    public class Stat_Player
    {
        public int level;
        public int maxHp;
        public int maxSp;
        public int attack;
        public int totalExp;
    }

    [Serializable]
    public class DataSet_Monster
    {
        public string name;
        public float maxHp;
        public float maxSp;
        public float damage;
        public float walkspeed;
        public float runspeed;
        public float atkrange;
        public float sensingrange;
        public List<DropItems> dropitems;
    }
    [Serializable]
    public struct DropItems
    {
        public string name;
        public int amount;
        public float probability;
    }


    [Serializable]
    public class DataSet_Group
    {
        public int id;
        public List<GroupNames> member;
    }
    [Serializable]
    public struct GroupNames
    {
        public string name;
    }


    #endregion

    #region DataSeparator 

    [Serializable]
    public class Stat_PlayerDataSeparator : IDataSeparator<int, Stat_Player> //������������ ���ӿ� �ε�� ���·� �߶� �ϴ� Ŭ����
    {
        public List<Stat_Player> playerstats = new List<Stat_Player>(); //������ ������ ����Ʈ�� �ش� ������ �̸��� �����ؾ� ��

        public Dictionary<int, Stat_Player> MakeDict()
        {
            Dictionary<int, Stat_Player> dict = new Dictionary<int, Stat_Player>();
            foreach (Stat_Player data in playerstats)
                dict.Add(data.level, data);
            return dict;
        }
    }

    [Serializable]
    public class Separator_MonsterTable : IDataSeparator<string, DataSet_Monster> //������������ ���ӿ� �ε�� ���·� �߶� �ϴ� Ŭ����
    {
        public List<DataSet_Monster> monsters = new List<DataSet_Monster>(); //������ ������ ����Ʈ�� �ش� ������ �̸��� �����ؾ� ��

        public Dictionary<string, DataSet_Monster> MakeDict()
        {
            Dictionary<string, DataSet_Monster> dict = new Dictionary<string, DataSet_Monster>();
            foreach (DataSet_Monster data in monsters)
                dict.Add(data.name, data);
            return dict;
        }
    }

    [Serializable]
    public class Separator_GroupTable : IDataSeparator<int, DataSet_Group> //������������ ���ӿ� �ε�� ���·� �߶� �ϴ� Ŭ����
    {
        public List<DataSet_Group> groups = new List<DataSet_Group>(); //������ ������ ����Ʈ�� �ش� ������ �̸��� �����ؾ� ��

        public Dictionary<int, DataSet_Group> MakeDict()
        {
            Dictionary<int, DataSet_Group> dict = new Dictionary<int, DataSet_Group>();
            foreach (DataSet_Group data in groups)
                dict.Add(data.id, data);
            return dict;
        }
    }
    #endregion

}
