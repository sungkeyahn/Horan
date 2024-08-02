using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item, IEquip
{
    public Data.EEquipmentType type;
    protected PlayerStat onwerStat;
    GameObject Ob;
    private void Awake()
    {
        onwerStat = GetComponentInParent<PlayerStat>();
    }

    public virtual void Equip(int id)
    {
        if (id == ID) return;

        if (Managers.DataLoder.DataCache_Equipments.ContainsKey(id))
        {
            if (Ob!=null) Destroy(Ob);
            
            GameObject prefab = Resources.Load<GameObject>(Managers.DataLoder.DataCache_Equipments[id].equipmentprefabpath);
            if (prefab)
            {
                Ob = Instantiate(prefab, gameObject.transform);
                ApplyEquipmentStat(ID,id);
                ID = id;
            }
        }
    }
    protected void ApplyEquipmentStat(int preID ,int ID)
    {
        if (Managers.DataLoder.DataCache_Equipments.ContainsKey(preID))
        for (int i = 0; i < Managers.DataLoder.DataCache_Equipments[preID].abilitys.Count; i++)
        {
            switch (Managers.DataLoder.DataCache_Equipments[preID].abilitys[i].type)
            {
                case Data.EEquipmentAbilityType.MaxHp:
                    onwerStat.MaxHp -= Managers.DataLoder.DataCache_Equipments[preID].abilitys[i].value;
                    break;
                case Data.EEquipmentAbilityType.MaxSp:
                    onwerStat.MaxSp -= Managers.DataLoder.DataCache_Equipments[preID].abilitys[i].value;
                    break;
                case Data.EEquipmentAbilityType.AttackDamage:
                    onwerStat.Attack -= Managers.DataLoder.DataCache_Equipments[preID].abilitys[i].value;
                    break;
                case Data.EEquipmentAbilityType.CriticalProbability:
                    onwerStat.Critical -= Managers.DataLoder.DataCache_Equipments[preID].abilitys[i].value;
                    break;
            }
        }

        if (Managers.DataLoder.DataCache_Equipments.ContainsKey(ID))
            for (int i = 0; i < Managers.DataLoder.DataCache_Equipments[ID].abilitys.Count; i++)
            {
                switch (Managers.DataLoder.DataCache_Equipments[ID].abilitys[i].type)
                {
                    case Data.EEquipmentAbilityType.MaxHp:
                        onwerStat.MaxHp += Managers.DataLoder.DataCache_Equipments[ID].abilitys[i].value;
                        break;
                    case Data.EEquipmentAbilityType.MaxSp:
                        onwerStat.MaxSp += Managers.DataLoder.DataCache_Equipments[ID].abilitys[i].value;
                        break;
                    case Data.EEquipmentAbilityType.AttackDamage:
                        onwerStat.Attack += Managers.DataLoder.DataCache_Equipments[ID].abilitys[i].value;
                        break;
                    case Data.EEquipmentAbilityType.CriticalProbability:
                        onwerStat.Critical += Managers.DataLoder.DataCache_Equipments[ID].abilitys[i].value;
                        break;
                }
            }
    }

}
