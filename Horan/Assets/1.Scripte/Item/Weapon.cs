using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : Equipment
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    public List<Data.AnimInfomation> AnimInfo_FATK { get; private set; } = new List<Data.AnimInfomation>();
    public List<Data.AnimInfomation> AnimInfo_SATK { get; private set; } = new List<Data.AnimInfomation>();

    private void Awake()
    {
        onwerStat = GetComponentInParent<PlayerStat>();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        AttachWeapon();
    }

    public void AttachWeapon()
    {
        ID = Managers.DataLoder.DataCache_Save.Equip.weapon;
        if (Managers.DataLoder.DataCache_Weapon.ContainsKey(ID))
        {
            meshFilter.mesh = Resources.Load<Mesh>(Managers.DataLoder.DataCache_Equipments[ID].meshpath);
            meshRenderer.material = Resources.Load<Material>(Managers.DataLoder.DataCache_Equipments[ID].materialpath);

            AnimInfo_FATK.Clear();
            AnimInfo_SATK.Clear();
            for (int i = 0; i < Managers.DataLoder.DataCache_Weapon[ID].fatkaniminfo.Count; i++)
                AnimInfo_FATK.Add(Managers.DataLoder.DataCache_Weapon[ID].fatkaniminfo[i]);
            for (int i = 0; i < Managers.DataLoder.DataCache_Weapon[ID].satkaniminfo.Count; i++)
                AnimInfo_SATK.Add(Managers.DataLoder.DataCache_Weapon[ID].satkaniminfo[i]);

            WeaponSocket Socket = FindObjectOfType<WeaponSocket>();
            if (Socket)
            {
                transform.parent = Socket.transform;
                transform.localPosition = new Vector3(Managers.DataLoder.DataCache_Weapon[ID].socketpos[0], Managers.DataLoder.DataCache_Weapon[ID].socketpos[1], Managers.DataLoder.DataCache_Weapon[ID].socketpos[2]);
                transform.localRotation = Quaternion.Euler(Managers.DataLoder.DataCache_Weapon[ID].socketrot[0], Managers.DataLoder.DataCache_Weapon[ID].socketrot[1], Managers.DataLoder.DataCache_Weapon[ID].socketrot[2]);
            }
        }
    }
}
