using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ECharacterAtkInfo
{ 
}

//이거 MonoBehavior로 바꾸고 Equipment에서 일괄적으로 장비 장착 해야 겟는데?
public class Weapon : MonoBehaviour
{
    int weaponid;
    PlayerStat onwerStat;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    public BoxCollider Area { get; private set; } //공격 범위 콜라이더
    public List<Data.AnimInfomation> AnimInfo_FATK { get; private set; } = new List<Data.AnimInfomation>();
    public List<Data.AnimInfomation> AnimInfo_SATK { get; private set; } = new List<Data.AnimInfomation>();

    private void Awake()
    {
        Area = GetComponentInChildren<BoxCollider>();
        Area.enabled = false;
        onwerStat = GetComponentInParent<PlayerStat>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        AttachWeapon();
    }
    
    //        

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitob = other.gameObject;
        if (hitob)
        {
            Debug.Log("ATTCKT");
            IDamageInteraction damageable = hitob.GetComponent<IDamageInteraction>();
            if (damageable != null)
            {
                float finaldamage = onwerStat.Attack;

                switch (onwerStat.atkType)
                {
                    case PlayerStat.ECharacterAtkType.FAtk:
                        break;
                    case PlayerStat.ECharacterAtkType.SAtk:
                        finaldamage = finaldamage * 1.5f;
                        break;
                    case PlayerStat.ECharacterAtkType.DashAtk:
                        finaldamage = finaldamage * 2f;
                        break;
                    case PlayerStat.ECharacterAtkType.CounterAtk:
                        finaldamage = finaldamage * 2.5f;
                        break;
                }

                if (UnityEngine.Random.Range(0, 99) < onwerStat.Critical)
                    finaldamage = finaldamage * 1.5f;

                if (damageable.TakeDamage(finaldamage))
                {
                    Managers.PrefabManager.SpawnEffect("Hit_strong",transform.position);
                }
            }
            Area.enabled = false;
        }
    }
    /*
    public override void Equip(int id)
    {
        if (id == weaponid) return;
        if (Managers.DataLoder.DataCache_Weapon.ContainsKey(id))
        {
            weaponid = id;
            meshFilter.mesh = Resources.Load<Mesh>(Managers.DataLoder.DataCache_Equipments[id].meshpath);
            meshRenderer.material = Resources.Load<Material>(Managers.DataLoder.DataCache_Equipments[id].materialpath);

            for (int i = 0; i < Managers.DataLoder.DataCache_Weapon[id].fatkaniminfo.Count; i++)
                AnimInfo_FATK.Add(Managers.DataLoder.DataCache_Weapon[id].fatkaniminfo[i]);
            for (int i = 0; i < Managers.DataLoder.DataCache_Weapon[id].satkaniminfo.Count; i++)
                AnimInfo_SATK.Add(Managers.DataLoder.DataCache_Weapon[id].satkaniminfo[i]);

            ApplyEquipmentStat(weaponid, id);
            AttachSocket();
        }
    }*/

    public void AttachWeapon()
    {
        weaponid = Managers.DataLoder.DataCache_Save.Equip.weapon;
        if (Managers.DataLoder.DataCache_Weapon.ContainsKey(weaponid))
        {
            meshFilter.mesh = Resources.Load<Mesh>(Managers.DataLoder.DataCache_Equipments[weaponid].meshpath);
            meshRenderer.material = Resources.Load<Material>(Managers.DataLoder.DataCache_Equipments[weaponid].materialpath);

            AnimInfo_FATK.Clear();
            AnimInfo_SATK.Clear();
            for (int i = 0; i < Managers.DataLoder.DataCache_Weapon[weaponid].fatkaniminfo.Count; i++)
                AnimInfo_FATK.Add(Managers.DataLoder.DataCache_Weapon[weaponid].fatkaniminfo[i]);
            for (int i = 0; i < Managers.DataLoder.DataCache_Weapon[weaponid].satkaniminfo.Count; i++)
                AnimInfo_SATK.Add(Managers.DataLoder.DataCache_Weapon[weaponid].satkaniminfo[i]);

            WeaponSocket Socket = FindObjectOfType<WeaponSocket>();
            if (Socket)
            {
                transform.parent = Socket.transform;
                transform.localPosition = new Vector3(Managers.DataLoder.DataCache_Weapon[weaponid].socketpos[0], Managers.DataLoder.DataCache_Weapon[weaponid].socketpos[1], Managers.DataLoder.DataCache_Weapon[weaponid].socketpos[2]);
                transform.localRotation = Quaternion.Euler(Managers.DataLoder.DataCache_Weapon[weaponid].socketrot[0], Managers.DataLoder.DataCache_Weapon[weaponid].socketrot[1], Managers.DataLoder.DataCache_Weapon[weaponid].socketrot[2]);
            }
        }
    }
}
