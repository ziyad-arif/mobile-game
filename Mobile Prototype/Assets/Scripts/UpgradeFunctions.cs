using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class UpgradeFunctions : MonoBehaviour
{
    public ManageGame gameManager;
    public GameObject upgradeMenu;
    public PlayerController playerController;
    public Animator upgradeAnimator;
    public float reloadDecreaseAmount;
    public int bounceIncrease = 1;
    public float bulletSpeed;
    public int bulletSpeedIncrease = 5;
    public int recoilStrengthIncrease = 10;
    public int bouncesAllowed;   
    public int piercesAllowed;
    public void Exit()
    {
        upgradeAnimator.SetTrigger("Exit");
        Time.timeScale = 1;
        Invoke(nameof(ResetUpgradeMenu), 0.3f);
    }

    private void ResetUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
        upgradeMenu.transform.localScale = Vector3.one;
    }
    public void BouncingBullets()
    {
        bouncesAllowed += bounceIncrease;
    }

    public void DecreasedReload()
    {
        playerController.reloadTime -= reloadDecreaseAmount;
    }

    public void Pierce()
    {
        piercesAllowed++;
    }
    public void BulletSpeed()
    {
        bulletSpeed += bulletSpeedIncrease;
    }
    public void IncreasedRecoil()
    {
        playerController.recoilStrength += recoilStrengthIncrease;
    }
}
