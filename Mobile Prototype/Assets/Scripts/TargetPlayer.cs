using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TargetPlayer : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;

    public float dartSpeed;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetPosition = player.transform.position - transform.position;
        rb.velocity = targetPosition.normalized * dartSpeed;
        transform.up = targetPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("Triangle"))
        {
            Destroy(gameObject);
        }
        else if (gameObject.CompareTag("Square"))
        {
            Destroy(gameObject);
        }       
    }
}
