using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEquip
{
    public void Equip(int id);
}
public class Equipment : MonoBehaviour, IEquip
{
    public Data.EEquipmentType type;
    protected PlayerStat onwerStat;
    private void Awake()
    {
        onwerStat = GetComponentInParent<PlayerStat>();
    }

    public virtual void Equip(int id)
    {
        GameObject prefab = Resources.Load<GameObject>(Managers.DataLoder.DataCache_Equipments[id].equipmentprefabpath);
        if (prefab)
        {
            Instantiate(prefab, gameObject.transform);

            if (Managers.DataLoder.DataCache_Equipments.ContainsKey(id))
            {
                for (int i = 0; i < Managers.DataLoder.DataCache_Equipments[id].abilitys.Count; i++)
                {
                    switch (Managers.DataLoder.DataCache_Equipments[id].abilitys[i].type)
                    {
                        case Data.EEquipmentAbilityType.MaxHp:
                            onwerStat.MaxHp += Managers.DataLoder.DataCache_Equipments[id].abilitys[i].value;
                            break;
                        case Data.EEquipmentAbilityType.MaxSp:
                            onwerStat.MaxSp += Managers.DataLoder.DataCache_Equipments[id].abilitys[i].value;
                            break;
                        case Data.EEquipmentAbilityType.AttackDamage:
                            onwerStat.Attack += Managers.DataLoder.DataCache_Equipments[id].abilitys[i].value;
                            break;
                        case Data.EEquipmentAbilityType.CriticalProbability:
                            onwerStat.Critical += Managers.DataLoder.DataCache_Equipments[id].abilitys[i].value;
                            break;
                    }
                }
            }
        }
    }

}
