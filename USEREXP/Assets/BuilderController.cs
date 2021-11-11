using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class BuilderController : MonoBehaviour
{
    public static BuilderController instance;
    public Camera cam;
    Vector3 moveDirection = new Vector3(0f,0f,0f);
    Vector2 mousePosition;
    Vector2 mouseSavedPosition;
    public int chosenStructureCost;
    //public Structure chosenStructure;
    public Structure chosenStructureTemplate;
    public Structure chosenStructureBlueprint;
    [SerializeField] private Color _validPlacementColorTint;
    [SerializeField] private Color _invalidPlacementColorTint;
   // [SerializeField] private Color _defaultStructureColorTint;
    public bool isPlaceable = false;
    // [SerializeField] private Material _buildMat;

    float gridSize = 1;
    //Collider2D anotherCollider;
    //Ray2D buildRaycasted;
    RaycastHit2D hit;

    public bool moveTutorialDone = true;
    public bool onWKey = false; //Pre Version 0.55 no static event in all actions
    public bool onAKey = false;
    public bool onSKey = false;
    public bool onDKey = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }

    private void Start()
    {
        cam = PlayerManager.instance.playerCamera.camera;
        chosenStructureTemplate = Instantiate(chosenStructureTemplate);
        chosenStructureTemplate.gameObject.SetActive(false);
        chosenStructureTemplate.GetComponent<BoxCollider2D>().isTrigger = true;
        chosenStructureTemplate.GetComponent<SpriteRenderer>().sortingOrder = 99;
        chosenStructureTemplate.enabled = false;
    }
    //public void OnDrawGizmos()
    //{
    //    if (chosenStructureTemplate.gameObject.activeSelf == true)
    //    {
    //        Gizmos.DrawWireCube(GetNearestPointOnGrid(mousePosition), chosenStructureTemplate.GetComponent<BoxCollider2D>().size);
            
    //    }
    //}

   public bool IsMouseOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void OnEnable()
    {
        
    }

   
    private void Update()
    {
        //Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (MenuManager.instance.GetCanvas(MenuType.Pause).gameObject.activeSelf == false)
            {
                MenuManager.instance.ShowCanvas(MenuType.Pause);
            }
            else if (MenuManager.instance.GetCanvas(MenuType.Pause).gameObject.activeSelf == true)
            {
                Debug.Log("UNPAUSE");
                RoundManager.instance.isPlaying = true;
                MenuManager.instance.GetCanvas(MenuType.Pause).GetComponent<PauseCanvas>().Hide();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CancelChosenStructure();
        }
        if (GameManager.instance.firstPlay == true)
        {
            if (moveTutorialDone == false)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    onWKey = true;
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    onAKey = true;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    onSKey = true;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    onDKey = true;
                }
                if (onWKey == true && onAKey == true && onSKey == true && onDKey == true)
                {

                    TutorialManager.instance.StartInitialFunction(TutorialPurpose.buildcost);
                }
            }
           
        }

        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        moveDirection.Normalize();
        if (moveDirection != new Vector3(0f, 0f, 0f))
        {

            cam.transform.position += moveDirection * 15f * Time.deltaTime;
            // PlayerManager.instance.playerCamera.CameraFollow(moveDirection);
        }

        if (mousePosition != (Vector2)cam.ScreenToWorldPoint(Input.mousePosition))
        {
            if (Time.timeScale == 1f)
            {
                mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                if (chosenStructureTemplate.gameObject.activeSelf == true)
                {

                    chosenStructureTemplate.transform.position = GetNearestPointOnGrid(mousePosition);



                    BoxCollider2D chosenStructureBoxCollider = chosenStructureTemplate.GetComponent<BoxCollider2D>();
                    Collider2D[] objectsCollided = Physics2D.OverlapBoxAll((Vector2)GetNearestPointOnGrid(mousePosition), chosenStructureBoxCollider.size - new Vector2(0.1f, 0.1f), 0, LayerMask.GetMask("Structure") | LayerMask.GetMask("World") | LayerMask.GetMask("Team1") | LayerMask.GetMask("TutorialBot"));// 1 << 3 | 1 << 9);// LayerMask.GetMask("Structure"));
                                                                                                                                                                                                                                                                                                                     //Debug.Log("NUMBER OF STUFF: " + objectsCollided.Length);
                    if (objectsCollided.Length > 1)
                    {
                        Color colorTint = chosenStructureTemplate.GetComponent<SpriteRenderer>().material.color;
                        colorTint = _invalidPlacementColorTint;
                        chosenStructureTemplate.GetComponent<SpriteRenderer>().color = colorTint;


                        // Debug.Log(objectsCollided[1].gameObject.name);

                        isPlaceable = false;

                        // Debug.Log("COLLIDER HITTING ");
                    }
                    else if (objectsCollided.Length == 1)
                    {
                        Color colorTint = chosenStructureTemplate.GetComponent<SpriteRenderer>().material.color;
                        colorTint = _validPlacementColorTint;
                        chosenStructureTemplate.GetComponent<SpriteRenderer>().color = colorTint;
                        // Debug.Log("THERE NOTHING IN THIS PLACE");

                        isPlaceable = true;



                    }


                }
            }
            
        }


        if (Input.GetMouseButtonDown(0))
        {
          //  Debug.Log("MOUSE OVER UI: " + IsPointerOverUIElement());
            if ( IsMouseOverUi() == false)
           {
                if (chosenStructureTemplate.gameObject.activeSelf == true && isPlaceable == true)
                {
                   if (Time.timeScale == 1f)
                    {
                        PlayerManager.instance.BuildingMaterials -= chosenStructureCost;
                        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().buildingMaterials.UpdateCounter(PlayerManager.instance.BuildingMaterials);
                        Structure newStructure = StructurePool.instance.GetObject(chosenStructureBlueprint);
                        //  newStructure.GetComponent<BoxCollider2D>().isTrigger = false;
                        //   newStructure.GetComponent<SpriteRenderer>().sortingOrder = 5;
                        // newStructure.GetComponent<SpriteRenderer>().color = _defaultStructureColorTint;
                        //chosenStructure.GetComponent<SpriteRenderer>().material = chosenStructure.defaultMat;
                        newStructure.transform.position = GetNearestPointOnGrid(mousePosition);
                        newStructure.enabled = true;

                        // chosenStructure = null;

                        newStructure.gameObject.SetActive(true);
                        newStructure.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        chosenStructureTemplate.gameObject.SetActive(false);
                        chosenStructureBlueprint = null;
                        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().lastSelectedStructure = null;
                        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().cancelReminderText.gameObject.SetActive(false);
                  
                        

                    }

                }
            }
         

        }


    }
    public Vector3 GetNearestPointOnGrid(Vector3 p_position)
    {
        
        Vector3 modifiedPosition;
        modifiedPosition.x = Mathf.RoundToInt((p_position.x / gridSize) * gridSize);
        modifiedPosition.y = Mathf.RoundToInt((p_position.y / gridSize) * gridSize);
        modifiedPosition.z = Mathf.RoundToInt((p_position.z / gridSize) * gridSize);
        return modifiedPosition;
    }
    public void ChangeChosenStructure(BlueprintScriptableObject p_selectedStructure)
    {
    
        chosenStructureCost = p_selectedStructure.cost;
        chosenStructureBlueprint = p_selectedStructure.structure;


        chosenStructureTemplate.GetComponent<SpriteRenderer>().sprite = p_selectedStructure.structure.GetComponent<SpriteRenderer>().sprite;
        chosenStructureTemplate.gameObject.SetActive(true);
        chosenStructureTemplate.transform.position = GetNearestPointOnGrid(mousePosition);
        


      

    }

    public void CancelChosenStructure()
    {
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().cancelReminderText.gameObject.SetActive(false);
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().lastSelectedStructure = null;
        chosenStructureTemplate.gameObject.SetActive(false);
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().cancelReminderText.gameObject.SetActive(false);

        // StructurePool.instance.ReturnToPool(chosenStructure);
        // chosenStructure = null;
        Debug.Log("NOOOOOOOOOOOOOOOOOOOOO STRUCTURE");
    }
}
