using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSerchComponent : MonoBehaviour
{
    public MonsterController mainTarget;
    List<MonsterController> close_monsters = new List<MonsterController>();
    float MainTargetDistance=float.MaxValue;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            close_monsters.Add(other.GetComponent<MonsterController>());
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            close_monsters.Remove(other.GetComponent<MonsterController>());
        }
    }

    public void SelectMainTarget(float RotAngle = 180f)
    {
        if (close_monsters.Count>0)
        {
            mainTarget = null;
            MainTargetDistance = float.MaxValue;
            for (int i = 0; i < close_monsters.Count; i++)
            {
                if (close_monsters[i] == null) return;
                Vector3 dir = close_monsters[i].transform.position - transform.position;
                if (Vector3.Angle(transform.forward, dir) <= RotAngle/2 && Vector3.Distance(close_monsters[i].transform.position, transform.position) < MainTargetDistance) //감지 시야각 
                {
                    mainTarget = close_monsters[i];
                }
            }
        }
    }

}
