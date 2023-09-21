using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class ManageGame : MonoBehaviour
{
    private float secondsElapsed = 0;
    private bool spawned = false;
    private float xPos;
    private float yPos;
    private const float SLOWPOWER = 0.7f;
    private const float FASTPOWER = 2.3f;

    public Dictionary<int, int> waveNumbers = new Dictionary<int, int> 
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
    public static int level;
    public float spawnWidth;
    public float spawnHeight;
    public float lerpDuration;
    public float spawnDistance;
    public bool shot = false;
    public Vector2 middleValue;
    public Vector2 endValue;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI popupText;
    public GameObject circlePrefab;
    public GameObject dartPrefab;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        switch (level)
        {
            case 1:
                Tutorial();
                Debug.Log("ran");
                break;
        }
        StartCoroutine(GameTime());
        SpawnCircles();    
    }

    private void Tutorial()
    {
        StartCoroutine(PopupText(SLOWPOWER, middleValue, "Tap and hold anywhere to use the joystick", 2));
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
        switch (eventNum)
        {
            case 1:
                if (end == middleValue)
                {
                    StartCoroutine(PopupText(FASTPOWER, endValue, text, eventNum));
                }
                break;
            case 2:
                if (Input.touchCount > 0)
                {
                    StartCoroutine(PopupText(FASTPOWER, endValue, text, eventNum));
                    StartCoroutine(PopupText(SLOWPOWER, middleValue, "Tap the shoot button to shoot a projectile", eventNum));
                }
                break;
            case 3:
                if (shot)
                {
                    StartCoroutine(PopupText(FASTPOWER, endValue, text, eventNum));
                }
                break;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned && waveNumbers.Count > 0)
        {
            if (waveNumbers.ElementAt(0).Key == secondsElapsed)
            {
                SpawnWave(waveNumbers.ElementAt(0).Value);
                spawned = true;
                waveNumbers.Remove(waveNumbers.ElementAt(0).Key);
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
