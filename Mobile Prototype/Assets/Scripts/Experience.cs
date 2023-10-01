using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    private Slider xpSlider;
    private SoundManager soundManager;
    private ManageGame gameManager;

    public List<float> xpValues;
    public List<GameObject> xpPrefabs;

    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<ManageGame>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        xpSlider = GameObject.Find("Canvas/XP Bar").GetComponent<Slider>();
    }

    private void Update()
    {
        if (gameManager.gameOver)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            soundManager.CollectXP();
            Destroy(gameObject);
            foreach (var item in xpPrefabs)
            {
                if (item == gameObject)
                {
                    xpSlider.value += xpValues[xpPrefabs.IndexOf(item)];
                }
            }
        }
    }
}
