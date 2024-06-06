using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Data
{
    //데이터 테이블 파일을 키값을 기준으로 분리하여 저장하기 위한 인터페이스
    public interface IDataSeparator<Key, Value>
    {
        Dictionary<Key, Value> MakeDict();
    }

    #region Save 
    //.json
    [Serializable]
    public class SaveData
    {
        public Save_User User;
        public Save_Equip Equip;
        public Save_Inventory Inventory;
    }
    [Serializable]
    public struct Save_User
    {
        public int level;
        public string name;
        public float exp;
        public int gold;
        //숙련도 ? 
    }
    [Serializable]
    public struct Save_Equip
    {
        public int weapon;
        public int head;
        public int clothes;
        public int accessory;
    }
    [Serializable]
    public struct Save_Inventory
    {
        public List<int> keys;
        public List<int> values;
    }
    #endregion

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
        public int dropgold;
        public float dropexp;
        public List<DropItems> dropitems;
    }
    [Serializable]
    public struct DropItems
    {
        public int id;
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

    [Serializable]
    public class DataSet_Item
    {
        public int id;
        public string name;
        public int slotindex;
        public EItemType type;
        public string info;
        public string iconfilename;
    }

    [Serializable]
    public class DataSet_Equipment
    {
        public int id;
        public string name;
        public string meshpath;
        public string materialpath;
        public string equipmentprefabpath;
        public EEquipmentType type;
        public List<EquipmentAbility> abilitys;
    }
    [Serializable]
    public enum EItemType { Equipment = 1, Material }
    [Serializable]
    public enum EEquipmentType { Weapon = 1, Head, Clothes, Accessory }
    [Serializable]
    public enum EEquipmentAbilityType { MaxHp = 1, MaxSp, AttackDamage, CriticalProbability }
    [Serializable]
    public struct EquipmentAbility
    {
        public EEquipmentAbilityType type;
        public float value;
    }
    [Serializable]
    public class DataSet_Weapon
    {
        public int id;
        public float[] socketpos;
        public float[] socketrot;
        public List<AnimInfomation> animinfo;
    }
    [Serializable]
    public struct AnimInfomation
    {
        public string name;
        public float delay;
        public float judgmenttime;
    }
    [Serializable]
    public enum LatentAbilityValueType {MaxHpUp = 1, MaxSpUp, DamageUp,HpRegen}
    [Serializable]
    public class DataSet_LatentAbility
    {
        public int id;
        public string abilityname;
        public string abilityinfo;
        public string iconpath;
        public LatentAbilityValueType type;
        public float value;
    }
    #endregion

    #region DataSeparator 

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
    public class Separator_MonsterTable : IDataSeparator<string, DataSet_Monster> //데이터파일을 게임에 로드될 형태로 잘라 하는 클래스
    {
        public List<DataSet_Monster> monsters = new List<DataSet_Monster>(); //데이터 파일의 리스트와 해당 변수의 이름이 동일해야 함

        public Dictionary<string, DataSet_Monster> MakeDict()
        {
            Dictionary<string, DataSet_Monster> dict = new Dictionary<string, DataSet_Monster>();
            foreach (DataSet_Monster data in monsters)
                dict.Add(data.name, data);
            return dict;
        }
    }
    [Serializable]
    public class Separator_GroupTable : IDataSeparator<int, DataSet_Group> //데이터파일을 게임에 로드될 형태로 잘라 하는 클래스
    {
        public List<DataSet_Group> groups = new List<DataSet_Group>(); //데이터 파일의 리스트와 해당 변수의 이름이 동일해야 함

        public Dictionary<int, DataSet_Group> MakeDict()
        {
            Dictionary<int, DataSet_Group> dict = new Dictionary<int, DataSet_Group>();
            foreach (DataSet_Group data in groups)
                dict.Add(data.id, data);
            return dict;
        }
    }

    [Serializable]
    public class Separator_ItemTable : IDataSeparator<int, DataSet_Item> 
    {
        public List<DataSet_Item> items = new List<DataSet_Item>(); 

        public Dictionary<int, DataSet_Item> MakeDict()
        {
            Dictionary<int, DataSet_Item> dict = new Dictionary<int, DataSet_Item>();
            foreach (DataSet_Item data in items)
                dict.Add(data.id, data);
            return dict;
        }
    }
    [Serializable]
    public class Separator_EquipmentTable : IDataSeparator<int, DataSet_Equipment>
    {
        public List<DataSet_Equipment> equipments = new List<DataSet_Equipment>();

        public Dictionary<int, DataSet_Equipment> MakeDict()
        {
            Dictionary<int, DataSet_Equipment> dict = new Dictionary<int, DataSet_Equipment>();
            foreach (DataSet_Equipment data in equipments)
                dict.Add(data.id, data);
            return dict;
        }
    }

    [Serializable]
    public class Separator_WeaponTable : IDataSeparator<int, DataSet_Weapon>
    {
        public List<DataSet_Weapon> weapons = new List<DataSet_Weapon>();

        public Dictionary<int, DataSet_Weapon> MakeDict()
        {
            Dictionary<int, DataSet_Weapon> dict = new Dictionary<int, DataSet_Weapon>();
            foreach (DataSet_Weapon data in weapons)
                dict.Add(data.id, data);
            return dict;
        }
    }

    [Serializable]
    public class Separator_LatentAbilityTable : IDataSeparator<int, DataSet_LatentAbility>
    {
        public List<DataSet_LatentAbility> abilitys = new List<DataSet_LatentAbility>();

        public Dictionary<int, DataSet_LatentAbility> MakeDict()
        {
            Dictionary<int, DataSet_LatentAbility> dict = new Dictionary<int, DataSet_LatentAbility>();
            foreach (DataSet_LatentAbility data in abilitys)
                dict.Add(data.id, data);
            return dict;
        }
    }

    #endregion

}
