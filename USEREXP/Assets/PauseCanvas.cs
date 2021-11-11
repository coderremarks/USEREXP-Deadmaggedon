using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MenuCanvas
{
    public GameObject pauseMenu;

    [SerializeField] private Button _resume;
    [SerializeField] private Button _tryAgain;
    [SerializeField] private Button _toTitleScreen;

    public void OnEnable()
    {
        RoundManager.instance.isPlaying = false;
        _tryAgain.interactable = true;
        Time.timeScale = 0;
    }

    public void OnDisable()
    {
        
        Time.timeScale = 1;
    }

 

    public void Resume()
    {
        RoundManager.instance.isPlaying = true;
        gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        _tryAgain.interactable = false;
        gameObject.SetActive(false);
        GameManager.instance.RetryGame();
       // Invoke("RestartGameAfter", 0.5f);
    }

    //public void RestartGameAfter()
    //{
    //    _tryAgain.interactable = false;
    //}

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        Application.Quit();
        //GameManager.instance.RestartGame();
        
    }

   
}
