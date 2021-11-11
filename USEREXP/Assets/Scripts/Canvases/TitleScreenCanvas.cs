using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleScreenCanvas : MenuCanvas
{
    public Button playButton;
    public Button quitButton;
    protected override void Start()
    {
        base.Start();

    }

    public override void Preinitialize()
    {
        base.Preinitialize();
        //playButton.interactable = true;
        //quitButton.interactable = true;
        //EnemySpawner.Instance.HideEnemies();
    }

    public void OnEnable()
    {
        playButton.interactable = true;
        quitButton.interactable = true;
    }

    public void Play()
    {

        playButton.interactable = false;
        GameManager.instance.StartGame();
        

    }

    public void Quit()
    {
        quitButton.interactable = false;
        Debug.Log("Application quit");
        Application.Quit();
    }
}
