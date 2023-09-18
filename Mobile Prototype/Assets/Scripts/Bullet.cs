using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool beenFired = false;

    private PlayerController playerController;
    private Vector2 relativeVector;
    public float bulletSpeed;
    public float distanceToDestroy;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GameObject.Find("Player").transform.Find("Cannon").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!beenFired)
        {
            relativeVector = (playerController.transform.position - playerController.player.transform.position).normalized;
            rb.velocity = relativeVector * bulletSpeed;
            beenFired = true;
        }
        if (Vector2.Distance(playerController.player.transform.position, transform.position) >= distanceToDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
