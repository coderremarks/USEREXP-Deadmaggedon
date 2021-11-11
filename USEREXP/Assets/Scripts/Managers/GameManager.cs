using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameManager instance;
    public bool firstPlay;
    public bool immediatelyRoundStart;
    [SerializeField] public MenuType defaultMenu;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Cursor.lockState = CursorLockMode.Confined;
        defaultMenu = MenuType.TitleScreen;
        StartCoroutine(SceneInstantiate.instance.AsyncLoadScene("UI", OnUILoaded));

    }

    // Start is called before the first frame update

    void Start()
    {



    
        // playing = false;
    }
    public void OnUILoaded()
    {
        
        MenuManager.instance.ShowCanvas(defaultMenu);
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeOut();
        if (firstPlay == false)
        {
            RoundManager.instance.objectPoolSceneName = "Workspace";
            // StartCoroutine(SceneInstantiate.instance.AsyncLoadScene(RoundManager.instance.objectPoolSceneName, OnGameLoaded));

        }
        else if (firstPlay == true)
        {
            RoundManager.instance.objectPoolSceneName = "Tutorial";


        }
   


    }
    //Tutorial
    //Immediately play (Unload tutorial, load game)
    public void OnTutorialLoaded()
    {

   
       // TutorialManager.instance.Initialize();





    }

    //Restart
    //Go back to menu (Reload Game)

    public void OnGameLoaded()
    {
      
        RoundManager.instance.Initialize();

        MenuManager.instance.ShowCanvas(defaultMenu);
        //  MenuManager.instance.GetCanvas(MenuType.Universal).GetComponent<TransitionCanvas>().FadeOut();
        MenuManager.instance.PreinitializeAllCanvas();
        



        
        if (immediatelyRoundStart == false)
        {
           
            RoundManager.instance.RoundManagerPreStart();
          //  RoundManager.instance.StartNight();
        }
        else if (immediatelyRoundStart == true)
        {
           // RoundManager.instance.RoundManagerPreStart(); //NEW CODE
            RoundManager.instance.StartNight();
        }
        Time.timeScale = 1f;

    }


    public void OnGameUnloaded()
    {



    }


    public void LoadSceneInitialize(MenuType selectedMenuType = MenuType.TitleScreen)
    {

        if (firstPlay == false)
        {
            RoundManager.instance.objectPoolSceneName = "Workspace";
            immediatelyRoundStart = true;
            // StartCoroutine(SceneInstantiate.instance.AsyncLoadScene(RoundManager.instance.objectPoolSceneName, OnGameLoaded));

        }
        else if (firstPlay == true)
        {
            RoundManager.instance.objectPoolSceneName = "Tutorial";
            immediatelyRoundStart = false;

        }
        defaultMenu = selectedMenuType;
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeOut();
        StartCoroutine(SceneInstantiate.instance.AsyncLoadScene(RoundManager.instance.objectPoolSceneName, OnGameLoaded));
    }

    public void UnloadSceneInitialize(MenuType selectedMenuType = MenuType.TitleScreen)
    {

        ClearObjectPools();
        //StopAllCoroutines();
        RoundManager.instance.Deinitialize();
        
        Time.timeScale = 1f;
        defaultMenu = selectedMenuType;
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeIn();
        StartCoroutine(WaitRealTime(UnloadScene, 1f));
    }

    public void UnloadScene()
    {
        StartCoroutine(SceneInstantiate.instance.AsyncUnloadScene(RoundManager.instance.objectPoolSceneName));
    }
    public void PreRetryGame()
    {
        UnloadSceneInitialize(MenuType.HUD);



    }
    public void PostRetryGame()
    {
      LoadSceneInitialize(MenuType.HUD);



    }

    public void RetryGame() //try again
    {
        StartCoroutine(WaitRealTime(PreRetryGame, PostRetryGame, 1f));
        immediatelyRoundStart = true;
        // StartCoroutine(DelayedWaitRealTime(PreRetryGame,1f,PostRetryGame, 1f));

    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeIn();
        
        StartCoroutine(WaitRealTime(PostStartGame, 1f));
       


    }
    public void PostStartGame()
    {
        LoadSceneInitialize(MenuType.HUD);
  

    }
    public void PreTutorial()
    {
        UnloadSceneInitialize(MenuType.HUD);



    }
    public void PostTutorial()
    {
        LoadSceneInitialize(MenuType.HUD);



    }


    public void StartTutorial()
    {
        
        StartCoroutine(SceneInstantiate.instance.AsyncLoadScene("Tutorial", OnTutorialLoaded)); //TEMPORARY
    }

    public void EndTutorial()
    {
        //RoundManager.instance.objectPoolSceneName = "Workspace";
        firstPlay = false;


        StartCoroutine(WaitRealTime(PreTutorial, PostTutorial, 1f));
        immediatelyRoundStart = true;
        //ClearObjectPools();
        //defaultMenu = MenuType.HUD;
        //StartCoroutine(SceneInstantiate.instance.AsyncUnloadScene(RoundManager.instance.objectPoolSceneName, OnGameUnloaded));

    }
    public void PreRestartGame()
    {
        UnloadSceneInitialize();
    }

    public void PostRestartGame()
    {
        MenuManager.instance.ShowCanvas(defaultMenu);
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeOut();
    }
    public void RestartGame()
    {
        
        StartCoroutine(WaitRealTime(PreRestartGame, PostRestartGame, 1f));
        immediatelyRoundStart = false;
    }

  
    public void GameOver()
    {
        ClearObjectPools();
        //StopAllCoroutines();
        MenuManager.instance.ShowCanvas(MenuType.GameOver);
       
        Time.timeScale = 0f;
        if (PlayerManager.instance.HighestWave < RoundManager.instance.wave)
        {
            PlayerManager.instance.HighestWave = RoundManager.instance.wave;
            PlayerPrefs.SetInt("Highscore",RoundManager.instance.wave);
        }
        MenuManager.instance.GetCanvas(MenuType.GameOver).GetComponent<GameOverCanvas>().UpdateScores(RoundManager.instance.wave,PlayerManager.instance.HighestWave) ;

    }

 
    public IEnumerator WaitRealTime(Action afterWaitPassedFunction, float timeDuration)
    {
        yield return new WaitForSecondsRealtime(timeDuration);
        afterWaitPassedFunction?.Invoke();
    }
    public IEnumerator WaitRealTime(Action beforeWaitPassedFunction, Action afterWaitPassedFunction,float timeDuration)
    {
        //Debug.Log(timeDuration);
        beforeWaitPassedFunction?.Invoke();
        yield return new WaitForSecondsRealtime(timeDuration);
      //  Debug.Log("Phase 2");
        afterWaitPassedFunction?.Invoke();
       // Debug.Log("FREEZE STOP");
    }

    public void FadeInOut()
    {
        StartCoroutine(GameManager.instance.WaitRealTime(MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeIn, MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeOut, 1f));

    }

    public void ClearObjectPools()
    {
        RoundManager.instance.StopAllCoroutines();
        PlayerPool.instance.ClearAllToPool();
        ProjectilePool.instance.ClearAllToPool();
        EnemyPool.instance.ClearAllToPool();
        ParticlePool.instance.ClearAllToPool();
        ParticlePropPool.instance.ClearAllToPool();
        DummyPool.instance.ClearAllToPool();
        CoinPool.instance.ClearAllToPool();
        StructurePool.instance.ClearAllToPool();
    }
}
