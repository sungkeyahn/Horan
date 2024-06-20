using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatentAbilityContainer
{
    public Action<int>  OnAbilityUpdate;
    //Managers.ContentsManager.AbilityContainer.AddAbility(new LatentAbility(2, playerStat)); 잠능UI 선택 시 그쪽에서 호출할 코드 
    public List<LatentAbility> abilities = new List<LatentAbility>();
    public void ApplyAllAbility(PlayerStat stat)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].Apply(stat);
        }
    }
    public bool AddAbility(LatentAbility ability, PlayerStat stat)
    {
        if (ability == null) return false;
        abilities.Add(ability);
        ability.Apply(stat);
        OnAbilityUpdate.Invoke(ability.Data.id);
        return true;
    }
    public void ClearAbilities()
    {
        abilities.Clear();
        Debug.Log("ClearAbilities");
    }

}
public class LatentAbility
{
    public Data.DataSet_LatentAbility Data;
    public LatentAbility(int id)
    {
        Data.DataSet_LatentAbility data = null;
        Managers.DataLoder.DataCache_LatentAbility.TryGetValue(id, out data);
        if (data != null)
            Data = data;
    }
    public void Apply(PlayerStat stat)
    {
        if (Data == null) return;
       
        switch (Data.type)
        {
            case global::Data.LatentAbilityValueType.MaxHpUp:
                stat.MaxHp += Data.value;
                break;
            case global::Data.LatentAbilityValueType.MaxSpUp:
                stat.MaxSp += Data.value;
                break;
            case global::Data.LatentAbilityValueType.DamageUp:
                stat.Attack += Data.value;
                break;
            case global::Data.LatentAbilityValueType.HpRegen:
                stat.Hp += Data.value;
                break;
        }
    }
}
