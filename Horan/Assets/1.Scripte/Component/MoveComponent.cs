using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    //리지드 바디 기반 유닛 이동 및 회전 
    public Vector3 MoveDir = Vector3.zero;
    public Vector3 RotDir = Vector3.zero;
    public float MoveSpeed = 0;
    public bool MoveActive = true;


    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Move();
    }

    public void SetMove(Vector3 dir, Vector3 rot , float speed=0)
    {
        MoveDir = dir;
        RotDir = rot;
        MoveSpeed = speed;
    }
    public void SetMove(float speed = 0)
    {
        MoveSpeed = speed;
    }
    public void MoveByPower(Vector3 dir, float power)
    {
        rigid.velocity = dir * power;
    }


    protected void Move()
    {
        if (!MoveActive) return;
        if (MoveDir != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(RotDir);
            rigid.rotation = targetAngle;            
        }
        rigid.velocity = MoveDir * MoveSpeed + Vector3.up * rigid.velocity.y;
    }



}
