using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TargetPlayer : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private SoundManager soundManager;
    private GameObject skulls;
    private PlayerController playerController;
    private ManageGame gameManager;

    public List<GameObject> xpPrefabs;
    public Slider healthBar;
    public Sprite defeatedSkull;
    public float speed;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<ManageGame>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        player = GameObject.Find("Player");
        playerController = GameObject.Find("Player/Cannon").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        skulls = GameObject.Find("Canvas/Skulls");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetPosition = player.transform.position - transform.position;
        rb.velocity = targetPosition.normalized * speed;
        transform.up = targetPosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            soundManager.EnemyDeath();
            if (gameObject.CompareTag("Triangle"))
            {
                Instantiate(xpPrefabs[0], transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
            if (healthBar != null)
            {
                healthBar.value -= playerController.bulletDamage;
                if (healthBar.value == 0)
                {
                    if (gameObject.CompareTag("Square") || gameObject.CompareTag("Triangle"))
                    {
                        Instantiate(xpPrefabs[0], transform.position, Quaternion.identity);
                    }
                    else if (gameObject.CompareTag("Brute"))
                    {
                        Instantiate(xpPrefabs[1], transform.position, Quaternion.identity);
                        foreach (Transform item in skulls.transform)
                        {
                            if (item.gameObject.name.Contains("Skull"))
                            {
                                item.gameObject.GetComponent<SpriteRenderer>().sprite = defeatedSkull;
                                item.gameObject.name = "Defeated";
                                break;
                            }
                        }
                    }
                    else if (gameObject.CompareTag("Boss"))
                    {
                        Instantiate(xpPrefabs[2], transform.position, Quaternion.identity);
                        gameManager.bossDefeated = true;
                        gameManager.bossHealth = -1;
                    }
                    Destroy(gameObject);
                }
            }                        
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            soundManager.EnemyDeath();
            if (gameObject.CompareTag("Triangle"))
            {
                gameObject.SetActive(false);
            }
            else if (gameObject.CompareTag("Square"))
            {
                Destroy(gameObject);
            }           
        }   
    }
}
