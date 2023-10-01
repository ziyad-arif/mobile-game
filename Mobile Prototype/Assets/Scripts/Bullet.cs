using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool beenFired = false;
    private bool addBounce = false;
    private int bounceCount = 0;
    private int pierceCount = 0;
    private PlayerController playerController;
    private UpgradeFunctions upgradeFunctions;
    private Vector2 relativeVector;
    private CircleCollider2D circleCollider;

    public PhysicsMaterial2D bounce;
    public float distanceToDestroy;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GameObject.Find("Player").transform.Find("Cannon").GetComponent<PlayerController>();
        upgradeFunctions = GameObject.Find("Upgrade Functions").GetComponent<UpgradeFunctions>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Makes bullet bounce if upgrade has been added
        if (upgradeFunctions.bouncesAllowed > bounceCount && !addBounce)
        {
            rb.sharedMaterial = bounce;
            gameObject.layer = 8;
            circleCollider.enabled = false;
            circleCollider.enabled = true;
            addBounce = true;
        }
        // Calculates bullet direction and sets velocity in that direction
        if (!beenFired)
        {
            relativeVector = (playerController.transform.position - playerController.player.transform.position).normalized;
            rb.velocity = relativeVector * upgradeFunctions.bulletSpeed;
            beenFired = true;
        }
        // Destroy bullet if it goes too far away
        if (Vector2.Distance(playerController.player.transform.position, transform.position) >= distanceToDestroy)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 7)
        {
            if (bounceCount < upgradeFunctions.bouncesAllowed)
            {
                bounceCount++;
            }
            else
            {
                rb.sharedMaterial = null;
                gameObject.layer = 0;
                circleCollider.enabled = false;
                circleCollider.enabled = true;
            }
        }
        else if (pierceCount < upgradeFunctions.piercesAllowed && !collision.gameObject.CompareTag("Circle"))
        {
            pierceCount++;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
