using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject cannon;
    public float circleDamage;
    public float triangleDamage;

    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = cannon.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Circle"))
        {
            playerController.slider.value -= circleDamage;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Triangle"))
        {
            playerController.slider.value -= triangleDamage;
        }
    }
}
