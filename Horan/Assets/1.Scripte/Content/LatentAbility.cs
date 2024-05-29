using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * �߰������� ����ؾ��ϴ� ��
 * 1. �� �̵��� ������ �ѱ�� -> ��� �������� ��쿡�� ���̺굥���Ϳ��� ���� ������ �������
 * ������ ����ɷ°��� ���� �ֹ߼� ������ ������ 
 * ������ ����� ĳ���Ϳ��� �ٽ� ������� �� �ʿ䰡 ����
 * -> �׷��� ������ �Ŵ������� �����̳ʸ� ������ �ְ� ������ �Ѿ�� ���� ��Ű�� ������� ó��?
 * 2. �����Ƽ�� �÷��̾��� ���ݿ��� ������ ���� �� �ƴ��� Ȯ���ϰ� �������� ����
 * ���� ������� Ȯ�强 �ִ� ������ ������ �� �ʿ䰡 ����?
 */
public class LatentAbilityContainer
{
    //Managers.ContentsManager.AbilityContainer.AddAbility(new LatentAbility(2, playerStat)); ���UI ���� �� ���ʿ��� ȣ���� �ڵ� 
    List<LatentAbility> abilities = new List<LatentAbility>();
    public void ApplyAllAbility(PlayerStat stat)
    {
        Debug.Log(abilities.Count);
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
        return true;
    }
    public void ClearAbilities()
    {
        abilities.Clear();
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
