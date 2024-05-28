using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCharacterCtrl : MonoBehaviour
{
    Animator[] anims;
    PlayerStat Stat;
    Weapon weapon;
    Equipment[] equipments = new Equipment[4];

    private void Start()
    {
        equipments = GetComponentsInChildren<Equipment>();
        weapon = GetComponentInChildren<Weapon>();
        Stat = GetComponent<PlayerStat>();
        anims = GetComponentsInChildren<Animator>();
        UpdateEquipments();
    }

    public void UpdateEquipments()
    {
        for (int i = 0; i < equipments.Length; i++)
        {
            switch (equipments[i].type)
            {
                case Data.EEquipmentType.Head:
                    equipments[i].Equip(Managers.DataLoder.DataCache_Save.Equip.head);
                    break;
                case Data.EEquipmentType.Clothes:
                    equipments[i].Equip(Managers.DataLoder.DataCache_Save.Equip.clothes);
                    break;
                case Data.EEquipmentType.Accessory:
                    equipments[i].Equip(Managers.DataLoder.DataCache_Save.Equip.accessory);
                    break;
                default:
                    break;
            }
        }

        weapon.Equip(Managers.DataLoder.DataCache_Save.Equip.weapon);
    }
}
