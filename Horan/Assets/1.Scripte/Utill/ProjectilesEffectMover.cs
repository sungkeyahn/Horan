using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesEffectMover : MonoBehaviour
{
    public float speed = 15f;

    public void SetTarget(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction); // ȭ���� ���� ����
    }
    void FixedUpdate()
    {
        if (speed != 0)
        {
            //rb.AddForce(transform.forward * speed);
           // rb.velocity = transform.forward * speed;

            transform.position += transform.forward * (speed * Time.deltaTime);         
        }
    }
}
