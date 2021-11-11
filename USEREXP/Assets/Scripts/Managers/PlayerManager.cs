using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] public static PlayerManager instance;

    public Player player;
    public CameraMovement playerCamera;
    public int HighestWave;
    public int BuildingMaterials;
    public Texture2D normalPointer, crossHair;
    public GameObject currentlyEquippedWeapon;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        HighestWave = PlayerPrefs.GetInt("Highscore");
        BuildingMaterials = 0;
    }

    public void ActivateNormalPointer()
    {
        Cursor.SetCursor(normalPointer, Vector2.zero, CursorMode.Auto);
    }

    public void ActivateCrosshair()
    {
        Cursor.SetCursor(crossHair, new Vector2(crossHair.width/2, crossHair.height/2), CursorMode.ForceSoftware);//Cursor.SetCursor(crossHair, new Vector2(crossHair.width/2, crossHair.height/2), CursorMode.ForceSoftware);
    }

 
   
}
