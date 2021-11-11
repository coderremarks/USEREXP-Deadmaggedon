using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public enum TutorialPurpose
{
    movement,
    health,
    reload,
    combat,
    combatTest,
    build,
    buildcost,
    waveTimer,
    mayor,
    mayortwo,
    mayorthree,
    graduate,
    none,
}

[System.Serializable]
public class Tutorial
{
    public TutorialPurpose tutorialPurpose;
    
    public string animationTriggerGame;
    public bool finished;
    [SerializeField] public Action initialFunction;
    [SerializeField] public Action endFunction;
}
public class TutorialManager : MonoBehaviour
{
    [SerializeField] public static TutorialManager instance;
    public Animator movementCamera;
    public GameObject movementGate;
    public GameObject combatGate;
    public GameObject mayorGate;
    public GameObject graduateGate;
    [SerializeField] public List<Tutorial> tutorials;
    public DummySpawner EnemyRespawner;
    public List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public int killAmountMission = 0;
    public GameObject tutorialBot;
 
    public GameObject mayorCam;
    public GameObject mayorBotCam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        RegisterActions();
     
    }

    
    public Tutorial Get(TutorialPurpose chosenTutorial)
    {
     
        for (int i = 0; i < tutorials.Count; i++)
        {

            if (tutorials[i].tutorialPurpose == chosenTutorial)
            {
                return tutorials[i];
            }
        }
        return null;
    }
    public void Choose(TutorialPurpose chosenTutorial)
    {
       
        for (int i = 0; i < tutorials.Count; i++)
        {

            if (tutorials[i].tutorialPurpose == chosenTutorial)
            {
                MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(chosenTutorial);
                if (tutorials[i].animationTriggerGame != "")
                {
                    this.gameObject.GetComponent<Animator>().SetTrigger(tutorials[i].animationTriggerGame);
                }
                if (tutorials[i].endFunction != null)
                {
                    tutorials[i].endFunction?.Invoke();
                }
            }
        }
    }

    public void StartInitialFunction(TutorialPurpose chosenTutorial)
    {

        for (int i = 0; i < tutorials.Count; i++)
        {
            if (tutorials[i].tutorialPurpose == chosenTutorial)
            {

                tutorials[i].initialFunction?.Invoke();
            }
        }
    }

    public void StartEndFunction(TutorialPurpose chosenTutorial)
    {

        for (int i = 0; i < tutorials.Count; i++)
        {
            if (tutorials[i].tutorialPurpose == chosenTutorial)
            {

                tutorials[i].endFunction?.Invoke();
            }
        }
    }
    public void RegisterActions()
    {

        Get(TutorialPurpose.movement).initialFunction += TutorialManager.instance.MovementStart;
        Get(TutorialPurpose.movement).endFunction += TutorialManager.instance.MovementEnd;
        Get(TutorialPurpose.combat).initialFunction += TutorialManager.instance.CombatStart;
        Get(TutorialPurpose.combat).endFunction += TutorialManager.instance.CombatEnd;
        Get(TutorialPurpose.combatTest).initialFunction += TutorialManager.instance.CombatTestStart;
        Get(TutorialPurpose.combatTest).endFunction += TutorialManager.instance.CombatTestEnd;
        Get(TutorialPurpose.build).initialFunction += TutorialManager.instance.BuildStart;
        Get(TutorialPurpose.build).endFunction += TutorialManager.instance.BuildEnd;
        Get(TutorialPurpose.buildcost).initialFunction += TutorialManager.instance.BuildcostStart;
        Get(TutorialPurpose.buildcost).endFunction += TutorialManager.instance.BuildcostEnd;
        Get(TutorialPurpose.mayor).initialFunction += TutorialManager.instance.MayorStart;
   
        Get(TutorialPurpose.mayor).endFunction += TutorialManager.instance.MayorEnd;
        Get(TutorialPurpose.mayortwo).endFunction += TutorialManager.instance.MayorTwoEnd;
        Get(TutorialPurpose.mayorthree).endFunction += TutorialManager.instance.MayorThreeEnd;
        CharacterController.onReloadTool += Reload;

 
        Get(TutorialPurpose.graduate).initialFunction += GameManager.instance.EndTutorial;
    }
    public void OnEnable()
    {
        Invoke("StartTutorialManager",0.1f);
    }

    public void OnDisable()
    {
        Get(TutorialPurpose.movement).initialFunction -= TutorialManager.instance.MovementStart;
        Get(TutorialPurpose.movement).endFunction -= TutorialManager.instance.MovementEnd;
        Get(TutorialPurpose.combat).initialFunction -= TutorialManager.instance.CombatStart;
        Get(TutorialPurpose.combat).endFunction -= TutorialManager.instance.CombatEnd;
        Get(TutorialPurpose.combatTest).initialFunction -= TutorialManager.instance.CombatTestStart;
        Get(TutorialPurpose.combatTest).endFunction -= TutorialManager.instance.CombatTestEnd;
        Get(TutorialPurpose.build).initialFunction -= TutorialManager.instance.BuildStart;
        Get(TutorialPurpose.build).endFunction -= TutorialManager.instance.BuildEnd;
        Get(TutorialPurpose.buildcost).initialFunction -= TutorialManager.instance.BuildcostStart;
        Get(TutorialPurpose.buildcost).endFunction -= TutorialManager.instance.BuildcostEnd;
        Get(TutorialPurpose.mayor).initialFunction -= TutorialManager.instance.MayorStart;

        Get(TutorialPurpose.mayor).endFunction -= TutorialManager.instance.MayorEnd;
        Get(TutorialPurpose.mayortwo).endFunction -= TutorialManager.instance.MayorTwoEnd;
        Get(TutorialPurpose.mayorthree).endFunction -= TutorialManager.instance.MayorThreeEnd;
        CharacterController.onReloadTool -= Reload;


        Get(TutorialPurpose.graduate).initialFunction -= GameManager.instance.EndTutorial;
    }

    public void StartTutorialManager()
    {
       // Debug.Log("tutorial STAAAAAAART");
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().HideAll();


        CutsceneManager.instance.PlayCutsceneTutorial(TutorialPurpose.movement, 0, movementCamera.transform.position, 8);//, cutsceneTheatrics, theatricsTrigger);

        DialogueManager.instance.ActivateConversation(RoundManager.instance.goalObject, TutorialPurpose.movement);


        TutorialManager.instance.StartInitialFunction(TutorialPurpose.movement);
        

    }
    public void OnDestroy()
    {
     //   Player.onReloadTool -= Reload;
    }

    public void MakeItPlayable()
    {
        PlayerManager.instance.player.canMove = true;
        PlayerManager.instance.playerCamera.gameObject.transform.position = PlayerManager.instance.playerCamera.defaultPos;
        PlayerManager.instance.playerCamera.camera.orthographicSize = PlayerManager.instance.playerCamera.defaultZoom;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().StopAllCoroutines();
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().goNotification.gameObject.SetActive(false);
        //MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionPointer.Hide();
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().cutscene.SetTrigger("FadeOut");
        Cursor.visible = true;
        MenuManager.instance.ShowCanvas(MenuType.HUD);
        TutorialCamera.instance.focusObject = null;
        TutorialCamera.instance.enabled = false;
        PlayerManager.instance.player.characterController.enabled = true;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).gameObject.SetActive(true);
        PlayerManager.instance.player.Flip(0.6f);
        //if (chosenTutorialPurpose != TutorialPurpose.none)
        //{
        //    MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(chosenTutorialPurpose);
        //}
        //if (chosenTutorialPurpose == TutorialPurpose.movement)
        //{
        //    MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().goNotification.StartBlink();
        //}
    }

    public void MovementStart()
    {
        PlayerManager.instance.player.characterController.enabled = false;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).gameObject.SetActive(false);
      
        movementGate.GetComponent<Animator>().SetTrigger("Close");
        combatGate.GetComponent<Animator>().SetTrigger("Close");
        mayorGate.GetComponent<Animator>().SetTrigger("Close");
        graduateGate.GetComponent<Animator>().SetTrigger("Close");
        PlayerManager.instance.BuildingMaterials = 95;
        for (int i = 0; i < enemySpawners.Count; i++)
        {
            enemySpawners[i].RespawnEnemy();
        }
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().barrelSelection.SetActive(false);
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().survivorSelection.SetActive(false);
        BuilderController.instance.onWKey = false;
        BuilderController.instance.onAKey = false;
        BuilderController.instance.onSKey = false;
        BuilderController.instance.onDKey = false;
        BuilderController.instance.moveTutorialDone = true;
    }

    public void MovementEnd()
    {


        
        movementCamera.SetTrigger("MayorToZombies");
        PlayerManager.instance.playerCamera.enabled = false;
        TutorialCamera.instance.focusObject = movementCamera.gameObject;
        TutorialCamera.instance.enabled = true;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(TutorialPurpose.movement);
        //Debug.Log("Moveend");
        mayorGate.GetComponent<Door>().shakable = false;
        DialogueManager.instance.shook = false;
        Invoke("MakeItPlayable", 8f);
        Invoke("MovementAfterEnd", 8.2f);


    }

    public void MovementAfterEnd()
    {
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().anim.SetTrigger("Movement");
    }
    public void CombatStart()
    {
        PlayerManager.instance.player.characterController.enabled = false;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).gameObject.SetActive(false);

        Debug.Log("CombatStart");
    }

    public void CombatEnd()
    {
        Debug.Log("CombatEnd");
        PlayerManager.instance.playerCamera.enabled = false;
        movementGate.GetComponent<Animator>().SetTrigger("Open");
   
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(TutorialPurpose.combat);
        tutorialBot.GetComponent<Animator>().SetTrigger("CombatToCombatTest");
        Invoke("MakeItPlayable", 0f);
        Invoke("CombatAfterEnd", 0.1f);

    }

    public void CombatAfterEnd()
    {
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().HideAll();
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().goNotification.gameObject.SetActive(true);
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().goNotification.StartBlink();
    }
    public void Reload()
    {
       
        if (Get(TutorialPurpose.combat).finished == true)
        {
        
            Get(TutorialPurpose.reload).finished = true;
     
            ReloadEnd();
            CharacterController.onReloadTool -= Reload;
            MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().HideAll();
        }
    }

    public void EnemyDied()
    {

        killAmountMission++;
        if (killAmountMission >= 5)
        {
            //CombatTestEnd(); CALL END FUNCTION!!!
            CombatTestQuestEnd();
            EnemyPool.instance.ClearAllToPool();
        }
      
    }

    public void ReloadEnd()
    {
        combatGate.GetComponent<Animator>().SetTrigger("Open");

        
       // CutsceneManager.instance.PlayCutsceneTutorial(TutorialPurpose.combatTest, 16f, tutorialBot.transform.position + new Vector3(0, 0, -10), 8f, tutorialBot.GetComponent<Animator>(), "CombatTest");
        Invoke("AfterReloadEnd", 16f);
    }

    public void AfterReloadEnd()
    {
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().goNotification.StartBlink();
    }
    public void CombatTestStart()
    {
        Debug.Log("CombatTestStart");
        combatGate.GetComponent<Animator>().SetTrigger("Close"); 
        PlayerManager.instance.player.characterController.enabled = false;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).gameObject.SetActive(false);

        Get(TutorialPurpose.reload).finished = true;

    }

    public void CombatTestEnd()
    {
        Debug.Log("CombatTestEnd");
        //movementCamera.SetTrigger("MayorToZombies");
        PlayerManager.instance.playerCamera.enabled = false;

        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(TutorialPurpose.combatTest);
        Invoke("MakeItPlayable", 0f);

       
    }

    public void CombatTestQuestEnd()
    {
        Debug.Log("CombatQuestEnd");
        mayorGate.GetComponent<Animator>().SetTrigger("Open");
        graduateGate.GetComponent<Animator>().SetTrigger("Close");
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().HideAll();
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().goNotification.gameObject.SetActive(true);
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().goNotification.StartBlink();
        tutorialBot.GetComponent<Dummy>().anim.SetTrigger("CombatQuestEndToMayor");
        
    }
    public void BuildStart()
    {
        Debug.Log("BuildStart");
        PlayerManager.instance.player.characterController.enabled = false;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).gameObject.SetActive(false);
        MenuManager.instance.InitializeAllCanvas();
        Get(TutorialPurpose.combatTest).finished = true;
        RoundManager.instance.nightTime = 30f;
        RoundManager.instance.maxHostileBotCount = 3;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().StopAllCoroutines();
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().goNotification.gameObject.SetActive(false);

        graduateGate.GetComponent<Animator>().SetTrigger("Close");
        
    }

    public void BuildEnd()
    {
        Debug.Log("BuildEnd");
        // movementCamera.SetTrigger("MayorToZombies"); //camera dramatics
        PlayerManager.instance.playerCamera.enabled = false;
        //TutorialCamera.instance.focusObject = movementCamera.gameObject;
        //TutorialCamera.instance.enabled = true;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(TutorialPurpose.build);


        BuilderController.instance.enabled = true;

        PlayerManager.instance.player.canMove = true;
        PlayerManager.instance.playerCamera.gameObject.transform.position = PlayerManager.instance.playerCamera.defaultPos;
        PlayerManager.instance.playerCamera.camera.orthographicSize = PlayerManager.instance.playerCamera.defaultZoom;

        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().cutscene.SetTrigger("FadeOut");
        Cursor.visible = true;

        TutorialCamera.instance.focusObject = null;
        TutorialCamera.instance.enabled = false;

        MenuManager.instance.GetCanvas(MenuType.Tutorial).gameObject.SetActive(true);


        PlayerManager.instance.player.characterController.enabled = false;
        MenuManager.instance.ShowCanvas(MenuType.Build);
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().barrelSelection.SetActive(false);
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().survivorSelection.SetActive(false);
        PlayerManager.instance.playerCamera.enabled = false;
        PlayerManager.instance.playerCamera.camera.gameObject.transform.position = RoundManager.instance.goalObject.transform.position + new Vector3(0f, 0f, -10f);

        BuilderController.instance.onWKey = false;
        BuilderController.instance.onAKey = false;
        BuilderController.instance.onSKey = false;
        BuilderController.instance.onDKey = false;
        BuilderController.instance.moveTutorialDone = false;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().anim.SetTrigger("BuildMove");
    }

    public void BuildcostStart()
    {
        BuilderController.instance.moveTutorialDone = true;
        BuildcostEnd();
    }

    public void BuildcostEnd()
    {
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(TutorialPurpose.buildcost);

    }

 
    public void StartWave()
    {
        Time.timeScale = 1f;
        BuilderController.instance.chosenStructureTemplate.gameObject.SetActive(false);
        BuilderController.instance.chosenStructureBlueprint = null;
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().lastSelectedStructure = null;
        RoundManager.instance.StopAllCoroutines();
        StartCoroutine(RoundManager.instance.StartTrial()); // start game
        BuilderController.instance.enabled = false;
        PlayerManager.instance.player.characterController.enabled = true;
        MenuManager.instance.ShowCanvas(MenuType.HUD);
        MenuManager.instance.GetCanvas(MenuType.Tutorial).gameObject.SetActive(true);
        MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(TutorialPurpose.mayor);
    }

    public void MayorStart()
    {

        Debug.Log("MayorStart");
      
        RoundManager.instance.StopCoroutine(RoundManager.instance.runningWaveCoroutine);
        RoundManager.instance.StopCoroutine(RoundManager.instance.runningSpawnCoroutine);

        StartCoroutine(MayorPreStart());
    }

    public IEnumerator MayorPreStart()
    {
    
        while (RoundManager.instance.currentHostileBotCount > 0)
        {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        PlayerManager.instance.player.characterController.enabled = false;
        EnemyPool.instance.ClearAllToPool();
        ParticlePropPool.instance.ReturnAllToPool();
        RoundManager.instance.currentHostileBotCount = EnemyPool.instance.objectPool[0].activeObjectsList.Count;

        CutsceneManager.instance.PlayCutsceneTutorial(TutorialPurpose.mayor, 0, mayorBotCam.transform.position, 10);//, cutsceneTheatrics, theatricsTrigger);
        PlayerManager.instance.player.characterController.enabled = false;
        MenuManager.instance.GetCanvas(MenuType.Tutorial).gameObject.SetActive(false);

        DialogueManager.instance.ActivateConversation(tutorialBot, TutorialPurpose.mayor);
    }
    public void MayorEnd()
    {
        Get(TutorialPurpose.mayor).finished = true;
        Debug.Log("MayorEnd");
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionPointer.Hide();

        

        RoundManager.instance.currentHostileBotCount = EnemyPool.instance.objectPool[0].activeObjectsList.Count;

        
        // MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(TutorialPurpose.mayor);
        // Invoke("MakeItPlayable", 0f);
        MayorTwoStart();
    }

    public void MayorTwoStart()
    {
        PlayerManager.instance.player.characterController.enabled = false;
        Debug.Log("MayorTwoStart");
        //CutsceneManager.instance.PlayCutsceneTutorial(TutorialPurpose.mayor, 3f, TutorialManager.instance.mayorCam.transform.position, 8f);
        //MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().HideAll();
        DialogueManager.instance.ActivateConversation(RoundManager.instance.goalObject, TutorialPurpose.mayortwo);
        //Invoke("AfterMayorStartTwo", 0.5f);

    }

    public void MayorTwoEnd()
    {
        Debug.Log("MayorTwoEnd");
        MayorThreeStart();
    }

    public void MayorThreeStart()
    {
        PlayerManager.instance.player.characterController.enabled = false;
        Debug.Log("MayorThreeStart");
        DialogueManager.instance.ActivateConversation(tutorialBot, TutorialPurpose.mayorthree);
        PlayerManager.instance.BuildingMaterials = 0;

    }

    public void MayorThreeEnd()
    {
        Debug.Log("MayorThreeEnd");
        Invoke("MakeItPlayable", 0f);
        graduateGate.GetComponent<Animator>().SetTrigger("Open");
        Invoke("CombatAfterEnd", 0.1f);
    }


    


}
