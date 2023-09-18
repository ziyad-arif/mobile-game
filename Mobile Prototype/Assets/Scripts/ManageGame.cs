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
    public float spawnWidth;
    public float spawnHeight;
    public float lerpDuration;
    public Vector2 middleValue;
    public Vector2 endValue;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI popupText;
    public GameObject circlePrefab;
    public GameObject dartPrefab;
    public GameObject player;
    public float spawnDistance;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameTime());
        SpawnCircles();
        StartCoroutine(PopupText(0.7f, middleValue));      
    }

    private IEnumerator PopupText(float power, Vector2 end)
    {
        float timeElapsed = 0;
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
        if (end == middleValue)
        {
            StartCoroutine(PopupText(2.3f, endValue));
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
