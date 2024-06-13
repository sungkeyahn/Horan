using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesEffectMover : MonoBehaviour
{
    public float speed = 15f;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);
    }
    void FixedUpdate()
    {
        if (speed != 0)
        {
            rb.velocity = transform.forward * speed;
            transform.position += transform.forward * (speed * Time.deltaTime);         
        }
    }
}
