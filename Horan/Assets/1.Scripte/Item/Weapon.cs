using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ECharacterAtkInfo
{ 
}

public class Weapon : Equipment
{
    int weaponid;
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
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitob = other.gameObject;
        if (hitob)
        {
            IDamageInteraction damageable = hitob.GetComponent<IDamageInteraction>();
            if (damageable != null)
            {
                //공격에 대한 정보를가지고 전달할 복합 데이터 필요
                //참조 : PalyerStat, ->AttackType , 
                //스텟 + *치명 판정 + *공격 타입 배수 적용 
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
        }
    }
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
    }
    void AttachSocket()
    {
        WeaponSocket Socket = FindObjectOfType<WeaponSocket>();
        transform.parent = Socket.transform;
        transform.localPosition = new Vector3(Managers.DataLoder.DataCache_Weapon[weaponid].socketpos[0], Managers.DataLoder.DataCache_Weapon[weaponid].socketpos[1], Managers.DataLoder.DataCache_Weapon[weaponid].socketpos[2]);
        transform.localRotation = Quaternion.Euler(Managers.DataLoder.DataCache_Weapon[weaponid].socketrot[0], Managers.DataLoder.DataCache_Weapon[weaponid].socketrot[1], Managers.DataLoder.DataCache_Weapon[weaponid].socketrot[2]);
    }
}
