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
    List<LatentAbility> abilities = new List<LatentAbility>();

    public void Init()
    {
        Debug.Log(abilities.Count);
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].Apply();
        }
    }
    public bool AddAbility(LatentAbility ability)
    {
        if (ability == null) return false;
        abilities.Add(ability);
        ability.Apply();
        return true;
    }
    public void ClearAbilities()
    {
        abilities.Clear();
    }

}
public class LatentAbility
{
    PlayerStat Stat;
    public Data.DataSet_LatentAbility Data;
    public LatentAbility(int id, Stat stat)
    {
        Data.DataSet_LatentAbility data = null;
        Managers.DataLoder.DataCache_LatentAbility.TryGetValue(id, out data);
        if (data != null&&stat!=null)
        {
            Data = data;
            Stat = stat as PlayerStat;
        }
    }
    public void Apply()
    {
        switch (Data.type)
        {
            case global::Data.LatentAbilityValueType.MaxHpUp:
                Stat.MaxHp += Data.value;
                break;
            case global::Data.LatentAbilityValueType.MaxSpUp:
                Stat.MaxSp += Data.value;
                break;
            case global::Data.LatentAbilityValueType.DamageUp:
                Stat.Attack += Data.value;
                break;
            case global::Data.LatentAbilityValueType.HpRegen:
                Stat.Hp += Data.value;
                break;
        }
    }
}
/*
 * �̱������θ��� ���� ����� ������ ����?
 * 
 * ������ ��� Ȯ���� �����ؾ� �ϴ°��?
 * 
 * 
 */
