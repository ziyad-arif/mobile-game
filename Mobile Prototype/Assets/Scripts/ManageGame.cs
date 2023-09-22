using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class ManageGame : MonoBehaviour
{
    private float secondsElapsed = 0;
    private float xPos;
    private float yPos;
    private const float SLOWPOWER = 0.7f;
    private const float FASTPOWER = 2.3f;
    private Vector2 middleValue;
    private Vector2 endValue;
    private Vector2 initialTextPos;
    private bool tutorialOver = false;
    private bool spawned = false;
    private bool popupAnimationEnded = false;
    private string useJoystick = "Tap and hold anywhere to use the joystick";
    private string pressShoot = "Tap the shoot button to shoot a projectile";
    private string killEnemies = "Destroy the enemies, and be mindful of your weapon's recoil";
    private List<Dictionary<int, int>> wavesList1 = new();
    private Dictionary<int, int> selectedWave = new();

    public Dictionary<int, int> waves2 = new()
    {
        {1, 3},
        {10, 4},
        {20, 5},
        {30, 5},
        {45, 6},
        {50, 2},
        {60, 3},
        {70, 5},
    };
    public int circleCount;
    public static int level = 1;
    public float spawnWidth;
    public float spawnHeight;
    public float lerpDuration;
    public float spawnDistance;
    public float textHeight;
    public bool shot = false;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI popupText;
    public GameObject circlePrefab;
    public GameObject dartPrefab;
    public GameObject player;
    public RectTransform canvasTransform;
    // Start is called before the first frame update
    void Start()
    {
        wavesList1.Add(waves2);
        foreach (var item in wavesList1)
        {
            if (item.ToString().Contains(level.ToString()))
            {
                selectedWave = item;
            }
        }
        initialTextPos = popupText.transform.position;
        middleValue = canvasTransform.position + new Vector3(0, canvasTransform.rect.height * textHeight);
        endValue = new Vector2(canvasTransform.position.x, canvasTransform.position.y * 2.5f);
        switch (level)
        {
            case 1:
                StartCoroutine(PopupText(SLOWPOWER, middleValue, useJoystick, 0));
                break;
        }
        StartCoroutine(GameTime());
        SpawnCircles();    
    }

    private IEnumerator PopupText(float power, Vector2 end, string text, int eventNum)
    {
        float timeElapsed = 0;
        popupText.text = text;
        Vector2 startValue = popupText.transform.position;
        while (timeElapsed < lerpDuration)
        {
            float t = Mathf.Pow(timeElapsed / lerpDuration, power);
            popupText.transform.position = Vector2.Lerp(startValue, end, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        popupText.transform.position = end;
        yield return new WaitForSeconds(0.5f);
        if (end == middleValue && !tutorialOver)
        {
            popupAnimationEnded = true;
        }
        switch (eventNum)
        {
            case 1:
                if (end == middleValue)
                {
                    StartCoroutine(PopupText(FASTPOWER, endValue, text, eventNum));
                }
                break;
            case 2:
                popupText.transform.position = initialTextPos;
                StartCoroutine(PopupText(SLOWPOWER, middleValue, pressShoot, 0));
                break;
            case 3:
                popupText.transform.position = initialTextPos;
                StartCoroutine(PopupText(SLOWPOWER, middleValue, killEnemies, 1));
                break;
            default: break;
        }     
    }

    // Update is called once per frame
    void Update()
    {
        // Tutorial
        if (popupAnimationEnded)
        {
            if (Input.touchCount > 0)
            {
                popupAnimationEnded = false;
                StartCoroutine(PopupText(FASTPOWER, endValue, useJoystick, 2));
            }
            if (shot)
            {
                popupAnimationEnded = false;
                tutorialOver = true;
                StartCoroutine(PopupText(FASTPOWER, endValue, pressShoot, 3));
                SpawnWave(1);
            }          
        }
        

        if (!spawned && selectedWave.Count > 0)
        {
            if (selectedWave.ElementAt(0).Key == secondsElapsed)
            {
                SpawnWave(selectedWave.ElementAt(0).Value);
                spawned = true;
                selectedWave.Remove(selectedWave.ElementAt(0).Key);
            }          
        }       
    }

    private IEnumerator GameTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (secondsElapsed % 60 < 10)
            {
                timeText.text = $"{Mathf.Floor(secondsElapsed / 60)}:0{secondsElapsed % 60}";
            }
            else
            {
                timeText.text = $"{Mathf.Floor(secondsElapsed / 60)}:{secondsElapsed % 60}";
            }
            spawned = false;
            secondsElapsed += 1;
        }       
    }

    private void SpawnWave(int numEnemies)
    {
        float angle = Mathf.Deg2Rad * (360 / numEnemies);
        for (int i = 1; i < numEnemies + 1; i++)
        {
            Instantiate(dartPrefab, (Vector2)player.transform.position + new Vector2(spawnDistance * Mathf.Cos(angle * i), spawnDistance * Mathf.Sin(angle * i)), dartPrefab.transform.rotation);
        }       
    }

    private void SpawnCircles()
    {
        for (int i = 0; i < circleCount; i++)
        {
            xPos = Random.Range(-spawnWidth, spawnWidth);
            yPos = Random.Range(-spawnHeight, spawnHeight);
            Instantiate(circlePrefab, new Vector2(xPos, yPos), circlePrefab.transform.rotation);
        }
    }
}
