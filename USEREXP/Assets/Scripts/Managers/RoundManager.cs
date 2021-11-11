using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Experimental.Rendering.Universal;
public class RoundManager : MonoBehaviour
{
    
    [SerializeField] public static RoundManager instance;
    public bool isPlaying;
    public int wave;
    public float dayTime;
    public float nightTime;
    public float nightTimeIncreaseRate;
    public List<int> buildingMaterialsAmount = new List<int>();
    public GameObject buildingHammerPrefab;

    public GameObject goalObject;
    public Transform playerSpawnPoint;

    public string objectPoolSceneName;

    public  IEnumerator runningSpawnCoroutine;
    public IEnumerator runningWaveCoroutine;
    public int currentHostileBotCount;
   
    public int maxHostileBotCount;
    public int hostileBotIncrease;
    public float hostileBotSpawnRate;

    public List<Transform> hostileBotSpawnPoints = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    public void Initialize()
    {
   
        MenuManager.instance.InitializeAllCanvas();
        
        playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform;
        GameObject[] spawnPointsFound = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        LightManager.instance.globalLight = GameObject.Find("GL2D").GetComponent<Light2D>();

        for (int i = 0; i < spawnPointsFound.Length; i++)
        {
            
            hostileBotSpawnPoints.Add(spawnPointsFound[i].transform);
        }
        goalObject = GameObject.Find("Mayor");
        wave = 0;
        dayTime = 30f;
        nightTime = 30f; //30
        nightTimeIncreaseRate = 15f;
        currentHostileBotCount = 0;
        maxHostileBotCount = 11;

        PlayerManager.instance.currentlyEquippedWeapon = Instantiate(ObjectManager.instance.weaponList[0]).GetComponent<Weapon>().gameObject;
        SceneManager.MoveGameObjectToScene(PlayerManager.instance.currentlyEquippedWeapon, SceneManager.GetSceneByName(objectPoolSceneName));
        LightManager.instance.globalLight.intensity = 1f;
    }
    public void Deinitialize()
    {
       
        //for (int i = 0; i < menuCanvasList.Count; i++)
        //{
        //    menuCanvasList.
        //}
        for (int i = 0; i < hostileBotSpawnPoints.Count;)
        {
            hostileBotSpawnPoints.RemoveAt(0);
        }
        StopAllCoroutines();
        PlayerManager.instance.playerCamera.StopAllCoroutines();
        nightTime = 0;
        PlayerManager.instance.BuildingMaterials = 0;


    }
    // Update is called once per frame
    void Update()
    {

    }
    public void RoundManagerPreStart()
    {
    

        isPlaying = true;
        PlayerSpawn();
        //MenuManager.instance.ShowCanvas(MenuType.HUD);
        PlayerManager.instance.ActivateCrosshair();
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionPointer.Show(goalObject.transform.position);
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionPointer.Initialize();

        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().waveTimerBar.UpdateWaveCounter(RoundManager.instance.wave);
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().waveTimerBar.setMaxValue(nightTime);
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().healthBar.SetMaxHealth(PlayerManager.instance.player.maxHealth);
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionHealth?.UpdateHealth(Mathf.CeilToInt(goalObject.GetComponent<Mayor>().currentHealth));
        PlayerManager.instance.playerCamera = Camera.main.GetComponent<CameraMovement>();
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo();
        MenuManager.instance.ShowCanvas(MenuType.HUD);
        LightManager.instance.intensityRate = 0.5f/(nightTime / 2f);

        //Time.timeScale = 1f;
    }

    public void PostStart()
    {
        isPlaying = true;
        MenuManager.instance.ShowCanvas(MenuType.DayActions);
        PlayerManager.instance.ActivateNormalPointer();
       
        //Time.timeScale = 1f;
    }
    public void StartNight(float delay = 0)
    {




        Invoke("RoundManagerPreStart",delay);
    

        StartCoroutine(CalmBeforeTheStorm());
        
        wave++;
       
        


    }

    public void EndNight()
    {

       
       Debug.Log("End Night");
        StopCoroutine(runningSpawnCoroutine);
   

        maxHostileBotCount += hostileBotIncrease;
        nightTime += nightTimeIncreaseRate;
        StartCoroutine(CheckIfNoEnmies());
        //Day time actions

    }

    public void PromptDay()
    {
        StartCoroutine(GameManager.instance.WaitRealTime(GameManager.instance.FadeInOut,PostStart,1.1f));

        PlayerPool.instance.ReturnAllToPool();
         
    }
    public IEnumerator StartTrial()
    {
        
        runningSpawnCoroutine = HostileBotSpawner();

        yield return new WaitForSeconds(1f);
        runningWaveCoroutine = NightTimer(TutorialManager.instance.MayorStart);
        StartCoroutine(runningWaveCoroutine);
        StartCoroutine(runningSpawnCoroutine);
    }

    public void StartDay()
    {
        Debug.Log("Start day");
        MenuManager.instance.ShowCanvas(MenuType.Build);
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().waveTimerBar.UpdateWaveCounter(RoundManager.instance.wave);
        BuilderController.instance.enabled = true;
        PlayerManager.instance.playerCamera.enabled = false;
        PlayerManager.instance.playerCamera.camera.gameObject.transform.position = RoundManager.instance.goalObject.transform.position + new Vector3(0f, 0f, -10f);
        StartCoroutine(DayTimer());

    }

    public void EndDay()
    {
              Time.timeScale = 1f;
        BuilderController.instance.chosenStructureTemplate.gameObject.SetActive(false);
        BuilderController.instance.chosenStructureBlueprint = null;
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().lastSelectedStructure = null;
        RoundManager.instance.StopAllCoroutines();
        StartNight();


    }
    public IEnumerator CalmBeforeTheStorm()
    {
        runningSpawnCoroutine = HostileBotSpawner();
       
        yield return new WaitForSeconds(1f);
        runningWaveCoroutine = NightTimer(EndNight);
        StartCoroutine(runningWaveCoroutine);
        StartCoroutine(runningSpawnCoroutine);

    }

    IEnumerator CheckIfNoEnmies()
    {
        while (RoundManager.instance.currentHostileBotCount > 0)
        {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionPointer.Hide();
        EnemyPool.instance.ClearAllToPool();
        ParticlePropPool.instance.ReturnAllToPool();
        currentHostileBotCount = EnemyPool.instance.objectPool[0].activeObjectsList.Count;
        PlayerManager.instance.BuildingMaterials += 10;
        int chosenIndex = 0;
        for (int i = 0; i < ObjectManager.instance.weaponList.Count; i++)
        {
            if (ObjectManager.instance.weaponList[i].name == PlayerManager.instance.currentlyEquippedWeapon.GetComponent<Weapon>().name)
            {
      
                chosenIndex = i;
            }

        }
        //Weapon newWeapon = Instantiate(ObjectManager.instance.weaponList[chosenIndex]).GetComponent<Weapon>();
        PlayerManager.instance.currentlyEquippedWeapon.gameObject.SetActive(false);


        PromptDay();

    }

    IEnumerator NightTimer(Action onTimerEnd)
    {
        float counter = nightTime;
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().waveTimerBar.setMaxValue(nightTime);
        while (counter > 0)
        {
            //Debug.Log(roundTime - counter);
            yield return new WaitForSeconds(1f);
            counter--;
            //Update ui;
            MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().waveTimerBar.timeCount(counter);
            if (counter > nightTime/2)
            {
                LightManager.instance.globalLight.intensity -= LightManager.instance.intensityRate;
            }
            else if(counter <= nightTime/2)
            {
                LightManager.instance.globalLight.intensity += LightManager.instance.intensityRate;
            }
        }
        //EndNight();
        onTimerEnd.Invoke();
    }

    IEnumerator DayTimer()
    {
        float counter = dayTime;
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().waveTimerBar.setMaxValue(dayTime);
        while (counter > 0)
        {
            //Debug.Log(roundTime - counter);
            yield return new WaitForSeconds(1f);
            counter--;
            //Update ui;
            MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().waveTimerBar.timeCount(counter);

        }
        
        
        EndDay();

    }
    public void PlayerSpawn()
    {
        BuilderController.instance.enabled = false;
        var newPlayer = PlayerPool.instance.GetObject(0);//Instantiate(PlayerManager.instance.prefab);
        
        newPlayer.transform.position = playerSpawnPoint.position;
        newPlayer.gameObject.name = "Player";
        PlayerManager.instance.player = newPlayer.GetComponent<Player>();
        SceneManager.MoveGameObjectToScene(newPlayer.gameObject, SceneManager.GetSceneByName(objectPoolSceneName));
        //Destroy(PlayerManager.instance.currentlyEquippedWeapon);

        PlayerManager.instance.currentlyEquippedWeapon.transform.parent = PlayerManager.instance.player.gameObject.transform;
        PlayerManager.instance.currentlyEquippedWeapon.transform.localPosition = PlayerManager.instance.player.gripPoint.localPosition;
        
        //WEAPON PORTION
        PlayerManager.instance.player.weaponSlots[PlayerManager.instance.player.equippedWeapon] = PlayerManager.instance.currentlyEquippedWeapon.GetComponent<Weapon>();
        
        PlayerManager.instance.player.weaponSlots[PlayerManager.instance.player.equippedWeapon].Equip(PlayerManager.instance.player.gameObject.name);
        if (PlayerManager.instance.player.isFacingRight)
        {
            PlayerManager.instance.player.weaponSlots[PlayerManager.instance.player.equippedWeapon].gameObject.GetComponent<SpriteRenderer>().flipY = false;

        }
        else if (!PlayerManager.instance.player.isFacingRight)
        {
            PlayerManager.instance.player.weaponSlots[PlayerManager.instance.player.equippedWeapon].gameObject.GetComponent<SpriteRenderer>().flipY = true;

        }
        //PlayerManager.instance.player.weaponSlots[PlayerManager.instance.player.equippedWeapon].gameObject.GetComponent<SpriteRenderer>().flipY = false;

        PlayerManager.instance.currentlyEquippedWeapon.gameObject.SetActive(true);
        PlayerManager.instance.player.characterController.enabled = true;
        PlayerManager.instance.player.InitializeStats();
    }
    IEnumerator HostileBotSpawner()
    {
        //Debug.Log("SPAWNING");
        yield return new WaitForSeconds(hostileBotSpawnRate);
        if (currentHostileBotCount < maxHostileBotCount)
        {
            RoundManager.instance.currentHostileBotCount += 1;
            //Randomize and choose what is the bot type
            int chosenBotTypeIndex = 0;// UnityEngine.Random.Range(0, EnemyPool.instance.objectPool.Count);
            var newlySpawnedHostileBot = EnemyPool.instance.GetObject(chosenBotTypeIndex);
            
            //Randomize and choose what is the bot spawn
            int chosenBotSpawnIndex = UnityEngine.Random.Range(0, hostileBotSpawnPoints.Count);
            newlySpawnedHostileBot.gameObject.transform.TransformPoint(Vector3.zero);
            newlySpawnedHostileBot.gameObject.transform.position = hostileBotSpawnPoints[chosenBotSpawnIndex].position;

            newlySpawnedHostileBot.gameObject.transform.right = hostileBotSpawnPoints[chosenBotSpawnIndex].right;

           // newlySpawnedHostileBot.InitializeStats();
        

        }
        runningSpawnCoroutine = HostileBotSpawner();
        StartCoroutine(runningSpawnCoroutine);
    }
}
