using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //������ ���̺� ���� ����
    public int damage;
    public string[] AttackAnimNames; // �ش� ����� ������ �� �ִ� ��� �ִϸ��̼� 
    public float[] AtkDelayTimes; // ���� �ִϸ��̼� �� ����ɶ� ������ �ð�(�ִϸ��̼� �� ���� �޶������� üũ �ʿ�) 
    public BoxCollider Area { get; private set; } //���� ���� �ݶ��̴�


    private void Start()
    {
        //���� �����͵� ���̺��� �ҷ����� ������� �����ϱ� 
        damage = 10;
        Area = GetComponent<BoxCollider>();
        Area.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        print(string.Format("{0} ������Ʈ�� {1} �� �浹�߽��ϴ�.", gameObject.name, other.gameObject.name));

        Stat onwerStat = GetComponentInParent<Stat>(); //���� ���� ���
        int critical = UnityEngine.Random.Range(0, 100);
        GameObject hitob = other.gameObject;
        if (hitob)
        {
            IDamageInteraction damageable = hitob.GetComponent<IDamageInteraction>();
            if (damageable != null)
            {
                if (critical < 50)
                {
                    damageable.TakeDamage(damage * 1.5f);
                }
                else
                {
                    damageable.TakeDamage(damage);
                }

            }
        }
    }
}
