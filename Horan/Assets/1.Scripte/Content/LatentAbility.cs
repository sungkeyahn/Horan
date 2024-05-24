using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 추가적으로 고려해야하는 점
 * 1. 씬 이동시 데이터 넘기기 -> 장비 장착같은 경우에는 세이브데이터에서 관리 함으로 상관없음
 * 하지만 잠재능력같은 경우는 휘발성 데이터 임으로 
 * 다음맵 입장시 캐릭터에게 다시 적용시켜 줄 필요가 있음
 * -> 그래서 컨텐츠 매니저에서 컨테이너를 가지고 있고 다음씬 넘어가면 적용 시키는 방식으로 처리?
 * 2. 어빌리티가 플레이어의 스텟에만 관련이 있을 지 아닐지 확실하게 정해지지 않음
 * 따라서 어느정도 확장성 있는 구조로 만들어야 할 필요가 있음?
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
 * 싱글톤으로만들어서 몬스터 드랍시 데이터 참조?
 * 
 * 몬스터의 드랍 확률을 참조해야 하는경우?
 * 
 * 
 */
