using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelect;
    public GameObject canvas;

    public void Play()
    {
        canvas.SetActive(false);
        levelSelect.SetActive(true);
    }
    public void StartLevel()
    {
        SceneManager.LoadScene(1);
        ManageGame.level = int.Parse(EventSystem.current.currentSelectedGameObject.name);
    }      
}
