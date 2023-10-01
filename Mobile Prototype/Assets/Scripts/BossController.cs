using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{   
    private TargetPlayer targetPlayer;
    private ManageGame gameManager;

    public float dartShipTimeTillDartSpawn = 3f;
    public List<GameObject> enemyPrefabs;
    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<ManageGame>();
        if (gameObject.name.Contains("Dartship"))
        {
            gameManager.bossText.text = "DARTSHIP";
            targetPlayer = gameObject.GetComponent<TargetPlayer>();
            StartCoroutine(Dartship());
            gameManager.bossHealth = gameManager.maxBossHealth = targetPlayer.healthBar.value;
        }
    }

    IEnumerator Dartship()
    {
        while (true)
        {
            Instantiate(enemyPrefabs[0]);
            yield return new WaitForSeconds(dartShipTimeTillDartSpawn);
            Debug.Log("spawn");
        }
    }
    // Update is called once per frame
    void Update()
    {
        gameManager.bossHealth = targetPlayer.healthBar.value;
    }
}
