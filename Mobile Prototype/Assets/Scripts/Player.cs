using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject cannon;
    public float circleDamage;
    public float triangleDamage;
    public float boundaryDamage;
    public ParticleSystem shatter;

    private PlayerController playerController;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        playerController = cannon.GetComponent<PlayerController>();
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
        else if (collision.gameObject.CompareTag("Boundary"))
        {
            playerController.slider.value -= boundaryDamage;
            rb.AddForce((Vector2)transform.position - collision.GetContact(0).point, ForceMode2D.Impulse);
        }
    }

    public void PlayShatter()
    {
        Debug.Log(shatter.isPlaying);
        shatter.Stop();
        shatter.Play();
        Debug.Log(shatter.isPlaying);
    }
}
