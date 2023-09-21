using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float cannonDistance;
    public float recoilStrength;
    public float currentHealth;
    public float maxHealth;
    public float reloadTime;
    public bool canInstantiate = true;
    public FloatingJoystick joystick;
    public GameObject player;
    public GameObject bulletPrefab;
    public Slider slider;
    public ManageGame gameManager;

    private Rigidbody2D playerRb;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (joystick.Direction != Vector2.zero)
        {
            transform.position = player.transform.position + (Vector3)joystick.Direction.normalized * cannonDistance;            
            transform.up = (Vector3)joystick.Direction;
        }
    }

    public void ShootClick()
    {
        if (canInstantiate)
        {
            playerRb.AddForce((transform.position - player.transform.position).normalized * -1 * recoilStrength, ForceMode2D.Impulse);
            Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
            StartCoroutine(ShootDelay());
        }
        gameManager.shot = true;
    }

    private IEnumerator ShootDelay()
    {
        canInstantiate = false;
        yield return new WaitForSeconds(reloadTime);
        canInstantiate = true;
    }
}
