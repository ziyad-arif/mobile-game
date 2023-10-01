using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Cinemachine;

public class ManageGame : MonoBehaviour
{
    private float secondsElapsed = 0;
    private float xPos;
    private float yPos;
    private int stars = 0;
    private bool spawned = false;
    private bool startedTime = false;
    private bool checkedProgressBar = false;
    private string pressShoot = "Tap the shoot button to shoot a projectile";
    private string killEnemies = "Destroy the enemies, and be mindful of your weapon's recoil";
    private List<Dictionary<int, List<int>>> wavesList = new();
    private Dictionary<int, List<int>> selectedWave = new();
    private SpriteRenderer playerSpriteRenderer;

    //Order: dart, boss dart, square, boss square,,circle
    public Dictionary<int, List<int>> waves1 = new() 
    {
        {1, new List<int>{1} },
        {5, new List<int>{2} },
        {15, new List<int>{3} },
        {20, new List<int>{2} },
        {30, new List<int>{3} },
        {40, new List<int>{4} },
        {50, new List<int>{4} },
        {60, new List<int>{4, 1} },
        {70, new List<int>{4} },
        {80, new List<int>{4} },
        {90, new List<int>{5} },
        {100, new List<int>{5} },
        {105, new List<int>{5} },
        {110, new List<int>{4} },
        {120, new List<int>{5, 2} },
        {130, new List<int>{4} },
        {140, new List<int>{6} },
        {150, new List<int>{6} },
        {160, new List<int>{6} },
        {170, new List<int>{6} },
        {180, new List<int>{6, 2} },
        {190, new List<int>{7} },
        {200, new List<int>{7} },
        {210, new List<int>{7, 1} },
        {220, new List<int>{8} },
        {230, new List<int>{8, 1} },
        {240, new List<int>{8} },
        {245, new List<int>{6} },
        {250, new List<int>{6} },
        {260, new List<int>{7} },
        {270, new List<int>{7} },
        {275, new List<int>{6, 1} },
        {285, new List<int>{6} },
        {295, new List<int>{6} },
        {300, new List<int>{6} },
        {310, new List<int>{8} },
        {320, new List<int>{8, 1} },
        {330, new List<int>{9} },
        {340, new List<int>{9} },
        {350, new List<int>{9} },       
        {360, new List<int>{-1} },
    };
    public Dictionary<int, List<int>> waves2 = new()
    {
        {1, new List<int>{1} },
        {5, new List<int>{2} },
        {15, new List<int>{3} },
        {20, new List<int>{2} },
        {30, new List<int>{3} },
        {40, new List<int>{4} },
        {50, new List<int>{4} },
        {60, new List<int>{4} },       
    };

    public List<Upgrade> upgrades = new();
    public int circleCount;
    public static int level = 1;
    public float timeTillStar;
    public float distanceForBoundaryToBeOnEdge = 0.5f;
    public float circlespawnWidth;
    public float circlespawnHeight;
    public float spawnDistance;
    public float progressBarWidth;
    public float bossBarAnimationTime;
    public float bossHealth;
    [HideInInspector] public float maxBossHealth;
    public float starAnimationTime = 1f;
    public bool shot = false;
    [HideInInspector] public bool canChangeShootState = true;
    [HideInInspector] public bool canChangeJoystickState = true;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public bool bossDefeated = false;
    public TextMeshProUGUI popupText;
    public TextMeshProUGUI bossText;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> bossPrefabs;
    public SpriteRenderer backgroundRenderer;
    public GameObject skullPrefab;  
    public GameObject player;
    public GameObject upgradePanel;
    public GameObject bottomLeftCorner;
    public GameObject topRightCorner;
    public List<GameObject> upgradeButtons = new();
    public GameObject loadingScreen;
    public Slider loadingBar;
    public RectTransform canvasTransform;
    public RectTransform skullsTransform;
    public Slider progressBar;
    public Slider xpBar;
    public Slider healthBar;
    public CinemachineVirtualCamera vcam;
    public Animator animatorPopup;
    public Animator animatorUpgrade;
    public Animator animatorStar;
    public Animator animatorPlayer;
    public UpgradeFunctions upgradeFunctions;
    public SoundManager soundManager;
    public List<Sprite> brokenPlayerSprites;
    public List<GameObject> starGameobjects;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        // Spawn Distance calculation based on screen size
        spawnDistance = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane)), player.transform.position);
        // Add <int, List<int>> dictionaries to a list
        wavesList.AddRange(new List<Dictionary<int, List<int>>> {waves1, waves2});
        selectedWave = wavesList[level - 1];
        timeTillStar = selectedWave.Keys.Max() / 3;
        // Skull position calculation
        MakeSkulls(0, timeTillStar);
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        // Setup Level
        switch (level)
        {
            case 1:
                
                break;
        }
        //SpawnCircles();    
    }

    void MakeSkulls(float secondsMin, float secondsMax)
    {
        foreach (var item in selectedWave)
        {
            if (item.Key > secondsMin && item.Key <= secondsMax)
            {
                foreach (var item1 in item.Value)
                {
                    if (item.Value.IndexOf(item1) % 2 != 0)
                    {
                        Instantiate(skullPrefab, skullsTransform.position + new Vector3(progressBarWidth * ((item.Key - secondsMin) / timeTillStar), 0), skullPrefab.transform.rotation, skullsTransform);
                    }
                }
            }
        }
    }

    void StopStarAnimation()
    {
        Vector2 position = Vector2.zero;
        starGameobjects[stars + 1].SetActive(true);
        switch (stars)
        {
            case 0:
                position = new Vector2(-24f, starGameobjects[stars].GetComponent<RectTransform>().anchoredPosition.y);                
                break;
            case 1:
                position = new Vector2(-28.5f, starGameobjects[stars].GetComponent<RectTransform>().anchoredPosition.y);
                starGameobjects[stars - 1].SetActive(false);
                break;
        }
        foreach (Transform item in skullsTransform)
        {
            Destroy(item.gameObject);
        }
        MakeSkulls(timeTillStar * (stars + 1), timeTillStar * (stars + 2));
        progressBar.value = 0;
        starGameobjects[stars].GetComponent<RectTransform>().anchoredPosition = position;
        animatorStar.enabled = false;       
        stars++;
        checkedProgressBar = false;
    }

    IEnumerator StopBossBar()
    {
        yield return new WaitForSeconds(bossBarAnimationTime);
        animatorStar.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            healthBar.value -= 10f;
        }
        // Changes player sprite based on health and triggers game over
        if (healthBar.value == 0)
        {            
            healthBar.gameObject.SetActive(false);
            canvasTransform.gameObject.SetActive(false);
            backgroundRenderer.color = Color.black;
            vcam.Follow = null;
            vcam.transform.position = new Vector3(0, 0, -10);
            backgroundRenderer.transform.position = new Vector3(backgroundRenderer.transform.position.x, backgroundRenderer.transform.position.y, -5);
            animatorPlayer.enabled = true;
            soundManager.audioSources[0].mute = true;
            Time.timeScale = 0;     
        }
        else if (healthBar.value < healthBar.maxValue * 0.25)
        {
            playerSpriteRenderer.sprite = brokenPlayerSprites[2];
        }
        else if (healthBar.value < healthBar.maxValue * 0.5)
        {
            playerSpriteRenderer.sprite = brokenPlayerSprites[1];
        }
        else if (healthBar.value < healthBar.maxValue * 0.75)
        { 
            playerSpriteRenderer.sprite = brokenPlayerSprites[0];
        } 

        // Makes boundaries
        SetBoundaries();

        // Increases stars by 1 when time is reached
        if (bossDefeated && !checkedProgressBar)
        {
            bossDefeated = false;
            checkedProgressBar = true;
            animatorStar.enabled = true;
            switch (stars)
            {
                case 1:                   
                    animatorStar.SetTrigger("GainTwoStar");
                    break;
            }
            Invoke(nameof(StopStarAnimation), starAnimationTime);
        }
        // Popup animation for tutorial
        if (level == 1)
        {
            if (animatorPopup.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animatorPopup.IsInTransition(0))
            {
                if (animatorPopup.GetCurrentAnimatorStateInfo(0).IsName("popup_exit_joystick"))
                {
                    animatorPopup.SetBool("JoystickFinished", true);
                    animatorPopup.SetBool("JoystickUsed", false);
                    canChangeJoystickState = false;
                }
                if (animatorPopup.GetCurrentAnimatorStateInfo(0).IsName("popup_exit_shoot"))
                {
                    animatorPopup.SetBool("AnyPopup", true);
                    animatorPopup.SetBool("ShootUsed", false);
                    canChangeShootState = false;
                }
            }           
            if (animatorPopup.GetCurrentAnimatorStateInfo(0).IsName("popup_entry"))
            {
                if (animatorPopup.GetBool("JoystickFinished"))
                {
                    popupText.text = pressShoot;
                }
                if (animatorPopup.GetBool("AnyPopup"))
                {
                    popupText.text = killEnemies;
                }              
            }
        }
        // If popup done, start the game time
        if (animatorPopup.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animatorPopup.IsInTransition(0) && animatorPopup.GetCurrentAnimatorStateInfo(0).IsName("popup_exit_any") && !startedTime)
        {
            StartCoroutine(GameTime());
            startedTime = true;
        }

        // If XP Bar is full, show upgrade screen and randomly assign upgrades
        if (xpBar.value == xpBar.maxValue)
        {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            upgradePanel.SetActive(true);
            List<Upgrade> initialUpgrades = new List<Upgrade>(upgrades);
            Upgrade selectedUpgrade;
            for (int i = 0; i < 3; i++)
            {
                selectedUpgrade = upgrades[UnityEngine.Random.Range(0, upgrades.Count)];
                upgradeButtons[i].transform.Find("Title").GetComponent<TextMeshProUGUI>().text = selectedUpgrade.title;
                upgradeButtons[i].transform.Find("Description").GetComponent<TextMeshProUGUI>().text = selectedUpgrade.description;
                upgradeButtons[i].transform.Find("Icon").GetComponent<Image>().sprite = selectedUpgrade.icon;
                UnityAction action = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), upgradeFunctions, selectedUpgrade.name);
                Debug.Log(action.Method);
                upgradeButtons[i].GetComponent<Button>().onClick.AddListener(action);
                upgrades.Remove(selectedUpgrade);
            }
            upgrades = initialUpgrades;
            xpBar.value = 0;
            Time.timeScale = 0;
        }
        
        // Progress Bar
        if (bossHealth != -1)
        {
            progressBar.value = bossHealth / maxBossHealth;
        }
        else if (selectedWave.Count > 0)
        {
            progressBar.value = (secondsElapsed - (stars * timeTillStar)) / timeTillStar;
        }

        // Spawn each wave of enemies
        if (!spawned && selectedWave.Count > 0)
        {
            if (secondsElapsed == timeTillStar)
            {
                SpawnBoss();
                spawned = true;
                selectedWave.Remove(selectedWave.ElementAt(0).Key);
            }
            else if (selectedWave.ElementAt(0).Key == secondsElapsed)
            {               
                SpawnWave(selectedWave.ElementAt(0).Value.Sum());
                spawned = true;
                selectedWave.Remove(selectedWave.ElementAt(0).Key);
            }
        }       
    }

    void SpawnBoss()
    {
        animatorStar.SetTrigger("BossBattle");
        animatorStar.enabled = true;
        skullsTransform.gameObject.SetActive(false);
        StartCoroutine(StopBossBar());
        Instantiate(bossPrefabs[level - 1], player.transform.position + new Vector3(spawnDistance, 0), bossPrefabs[level - 1].transform.rotation);
    }

    private IEnumerator GameTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            spawned = false;
            secondsElapsed += 1;
        }       
    }

    private void SpawnWave(int numEnemies)
    {
        float angle = Mathf.Deg2Rad * (360 / numEnemies);
        for (int j = 0; j < selectedWave.ElementAt(0).Value.Count; j++)
        {
            for (int i = 0; i < selectedWave.ElementAt(0).Value[j]; i++)
            {
                GameObject enemy = ObjectPool.SharedInstance.GetPooledObject();
                if (enemy)
                {
                    enemy.transform.position = (Vector2)player.transform.position + new Vector2(spawnDistance * Mathf.Cos(angle * i), spawnDistance * Mathf.Sin(angle * i));
                    enemy.SetActive(true);
                }
            }
        }              
    }

    private void SpawnCircles()
    {
        for (int i = 0; i < circleCount; i++)
        {
            xPos = UnityEngine.Random.Range(-circlespawnWidth, circlespawnWidth);
            yPos = UnityEngine.Random.Range(-circlespawnHeight, circlespawnHeight);
            Instantiate(enemyPrefabs[4], new Vector2(xPos, yPos), enemyPrefabs[4].transform.rotation);
        }
    }

    private void SetBoundaries()
    {
        bottomLeftCorner.transform.position =  Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        topRightCorner.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadSceneAsync("Level"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float value = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = value;
            yield return null;
        }
    }

    public void MainMenu()
    {
        StartCoroutine(LoadSceneAsync("MainMenu"));
    }
}
