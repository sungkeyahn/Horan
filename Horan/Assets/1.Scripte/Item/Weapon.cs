using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : Equipment
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    public BoxCollider Area { get; private set; } //공격 범위 콜라이더
    public List<Data.AnimInfomation> AnimInfo { get; private set; } = new List<Data.AnimInfomation>();

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
                float finaldamage = onwerStat.Attack;

                if (UnityEngine.Random.Range(0, 99) < onwerStat.Critical)
                    finaldamage = finaldamage * 1.5f;

                damageable.TakeDamage(finaldamage);
            }
        }
    }

    public override void Equip(int id)
    {
        base.Equip(id);
        if (Managers.DataLoder.DataCache_Weapon.ContainsKey(id))
        {
            meshFilter.mesh = Resources.Load<Mesh>(Managers.DataLoder.DataCache_Equipments[id].meshpath);
            meshRenderer.material = Resources.Load<Material>(Managers.DataLoder.DataCache_Equipments[id].materialpath);
            transform.localPosition = new Vector3(Managers.DataLoder.DataCache_Weapon[id].socketpos[0], Managers.DataLoder.DataCache_Weapon[id].socketpos[1], Managers.DataLoder.DataCache_Weapon[id].socketpos[2]);
            transform.localRotation = Quaternion.Euler(Managers.DataLoder.DataCache_Weapon[id].socketrot[0], Managers.DataLoder.DataCache_Weapon[id].socketrot[1], Managers.DataLoder.DataCache_Weapon[id].socketrot[2]);

            for (int i = 0; i < Managers.DataLoder.DataCache_Weapon[id].animinfo.Count; i++)
                AnimInfo.Add(Managers.DataLoder.DataCache_Weapon[id].animinfo[i]);
        }
    }
}
