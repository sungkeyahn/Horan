using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    //리지드 바디 기반 유닛 이동 및 회전 
    public Vector3 MoveDir { get; private set; }
    public float MoveSpeed { get; private set; }
    public bool MoveActive { get; set; }


    Rigidbody rigid;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        MoveActive = true;
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void SetMoveDir(Vector3 dir,float speed=0)
    {
        MoveDir = dir;
        MoveSpeed = speed;
    }
    public void MoveByPower(Vector3 dir, float power)
    {
        rigid.velocity = dir * power;
    }


    protected void Move()
    {
        if (!MoveActive) return;
        LookDir();
        rigid.velocity = MoveDir * MoveSpeed + Vector3.up * rigid.velocity.y;
    }
    protected void LookDir()
    {
        if (MoveDir != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(MoveDir);
            rigid.rotation = targetAngle;            
        }
    }


}
