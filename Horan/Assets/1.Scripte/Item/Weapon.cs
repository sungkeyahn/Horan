using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEquipment
{
    public void Equipment(int id);
}

public class Weapon : MonoBehaviour, IEquipment
{
    public BoxCollider Area { get; private set; } //공격 범위 콜라이더
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    PlayerStat onwerStat;



    public float damage { get; private set; }
    public List<Data.AnimInfomation> AnimInfo { get; private set; } = new List<Data.AnimInfomation>();


    private void Awake()
    {
        Area = GetComponent<BoxCollider>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        onwerStat = GetComponentInParent<PlayerStat>();
    }
    private void Start()
    {
        Area.enabled = false;
        onwerStat = GetComponentInParent<PlayerStat>();
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitob = other.gameObject;
        if (hitob)
        {
            IDamageInteraction damageable = hitob.GetComponent<IDamageInteraction>();
            if (damageable != null)
            {
                float finaldamage = onwerStat.Attack + damage;

                if (UnityEngine.Random.Range(0, 99) < onwerStat.Critical)
                    finaldamage = finaldamage * 1.5f;
                
                damageable.TakeDamage(finaldamage);
            }
        }
    }

    public void Equipment(int id)
    {
        if (Managers.DataLoder.DataCache_Equipments.ContainsKey(id))
        {
            for (int i = 0; i < Managers.DataLoder.DataCache_Equipments[id].abilitys.Count; i++)
            {
                if(Managers.DataLoder.DataCache_Equipments[id].abilitys[i].type==Data.EEquipmentAbilityType.AttackDamage)
                    damage = Managers.DataLoder.DataCache_Equipments[id].abilitys[i].value;
            }
           

            if (Managers.DataLoder.DataCache_Weapon.ContainsKey(id))
            {
                meshFilter.mesh = Resources.Load<Mesh>(Managers.DataLoder.DataCache_Weapon[id].meshpath);
                meshRenderer.material = Resources.Load<Material>(Managers.DataLoder.DataCache_Weapon[id].materialpath);

                transform.localPosition = new Vector3(Managers.DataLoder.DataCache_Weapon[id].socketpos[0], Managers.DataLoder.DataCache_Weapon[id].socketpos[1], Managers.DataLoder.DataCache_Weapon[id].socketpos[2]);
                transform.localRotation = Quaternion.Euler(Managers.DataLoder.DataCache_Weapon[id].socketrot[0], Managers.DataLoder.DataCache_Weapon[id].socketrot[1], Managers.DataLoder.DataCache_Weapon[id].socketrot[2]);

                //Area.center =;
                //Area.size=

                for (int i = 0; i < Managers.DataLoder.DataCache_Weapon[id].animinfo.Count; i++)
                {
                    AnimInfo.Add(Managers.DataLoder.DataCache_Weapon[id].animinfo[i]);
                }
            }
        }
    }
}
