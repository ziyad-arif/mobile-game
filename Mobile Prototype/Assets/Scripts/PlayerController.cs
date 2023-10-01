using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float meterTimePassed = 0f;

    public float speed;
    public float cannonDistance;
    public float recoilStrength;
    public float currentHealth;
    public float maxHealth;
    public float reloadTime;
    public float bulletDamage;
    public bool canInstantiate = true;
    public FloatingJoystick joystick;
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject joystickBackground;
    public Slider slider;
    public ManageGame gameManager;
    public Rigidbody2D playerRb;
    public SoundManager soundManager;
    public Animator animator;
    public Image reloadMeter;
    
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Activates joystick animation
        if (joystickBackground.activeInHierarchy && gameManager.canChangeJoystickState)
        {
            animator.SetBool("JoystickUsed", true);
        }
        // Controls direction of cannon
        if (joystick.Direction != Vector2.zero)
        {
            transform.position = player.transform.position + (Vector3)joystick.Direction.normalized * cannonDistance;            
            transform.up = (Vector3)joystick.Direction;
        }
        if (!canInstantiate)
        {
            reloadMeter.gameObject.SetActive(true);
            meterTimePassed += Time.fixedDeltaTime;
            reloadMeter.fillAmount = 1 - (meterTimePassed / reloadTime);
        }
        else
        {
            reloadMeter.gameObject.SetActive(false);
            reloadMeter.fillAmount = 1;
            meterTimePassed = 0;
        }
    }

    public void Shoot()
    {
        if (canInstantiate)
        {
            playerRb.AddForce((transform.position - player.transform.position).normalized * -1 * recoilStrength, ForceMode2D.Impulse);
            Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
            soundManager.Shoot();
            StartCoroutine(ShootDelay());
            if (gameManager.canChangeShootState)
            {
                animator.SetBool("ShootUsed", true);
            }           
        }
        else
        {
            soundManager.CannotShoot();
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
