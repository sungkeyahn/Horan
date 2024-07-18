using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSerchComponent : MonoBehaviour
{
    float MainTargetDistance = float.MaxValue;
    public MonsterController mainTarget; 
    public List<MonsterController> close_monsters = new List<MonsterController>();
    public List<BreakableObject> close_objects = new List<BreakableObject>();


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            close_monsters.Add(other.GetComponent<MonsterController>());
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("BreakableObject"))
        {
            close_objects.Add(other.GetComponent<BreakableObject>());
            other.GetComponent<RimLightEffect>().RimLightEffectSwitch(false);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            close_monsters.Remove(other.GetComponent<MonsterController>());
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("BreakableObject"))
        {
            close_objects.Remove(other.GetComponent<BreakableObject>());
            other.GetComponent<RimLightEffect>().RimLightEffectSwitch(true);
        }
    }

    public void SelectMainTarget(float RotAngle = 180f)
    {
        mainTarget = null;
        if (close_monsters.Count>0)
        {
            mainTarget = null;
            MainTargetDistance = float.MaxValue;
            for (int i = 0; i < close_monsters.Count; i++)
            {
                if (close_monsters[i] == null) continue;
                if (close_monsters[i].Stat.hp <= 0) continue;
                Vector3 dir = close_monsters[i].transform.position - transform.position;
                if (Vector3.Angle(transform.forward, dir) <= RotAngle/2 && Vector3.Distance(close_monsters[i].transform.position, transform.position) < MainTargetDistance) //감지 시야각 
                {
                    mainTarget = close_monsters[i];
                }
            }
        }
    }

}
