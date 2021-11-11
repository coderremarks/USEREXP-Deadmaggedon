using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CharacterController : MonoBehaviour
{
    [SerializeField] private Player _playerRef;
    [SerializeField] public Vector2 movementDirection;
    public bool isReloading;
    public static event Action onUseTool; //Pre Version 0.55 no static event in all actions
    public static event Action onReloadTool;

    //public void PrimaryUseTool(int team)
    //{
    //    playerRef.PrimaryUseTool(team);
    //    onUseTool?.Invoke();

    //}
    public void Start()
    {
        
        _playerRef = gameObject.GetComponent<Player>();
    }
    public void OnEnable()
    {
        isReloading = false;
        onReloadTool += MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.PlayNotif;
        onUseTool += MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo;
    }

    public void OnDisable()
    {
        onReloadTool -= MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.PlayNotif;
        onUseTool -= MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo;
    }
    // Update is called once per frame
    void Update()
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
        if (RoundManager.instance.isPlaying == true && _playerRef.canMove)
        {
           

            //Character Input
            movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movementDirection.Normalize();

            //Aim with mouse

            Vector2 mousePos = PlayerManager.instance.playerCamera.camera.ScreenToWorldPoint(Input.mousePosition);



            if (_playerRef.weaponSlots[_playerRef.equippedWeapon])
            {


                //weaponSlots[equippedWeapon].gameObject.transform.right= new Vector2(mousePos.x,mousePos.y) - (Vector2) rb.transform.position;
                mousePos -= (Vector2)_playerRef.rb.transform.position;

                float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
                _playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                if (Input.GetMouseButton(0))
                {

                    _playerRef.PrimaryUseTool(_playerRef.team);
                    onUseTool?.Invoke();
                    if (_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>() != null)
                    {
                        //CAMERA SHAKE FOR SHOOTING RECOIL
                        //if (_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().currentAmmoInMag > 0)
                        //{
                        //    //      PlayerManager.instance.playerCamera.CameraRecoil(0.005f, 0.1f, 0f);
                        //}
                        if (_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().inReload == false)
                        {
                            if (_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().currentAmmoInMag <= _playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().AmmoInMagCapacity * 0.3)//20% of max ammo
                            {
                                MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().lowAmmo.lowAmmoText.text = "Low Ammo";
                                MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().lowAmmo.gameObject.SetActive(true);
                            }
                            if (_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().currentAmmoInMag == 0)
                            {
                                //auto reload
                                if (_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().inReload == false && _playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().inUse == false && _playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().canUse == true)
                                {
                                    isReloading = true;
                                    MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().lowAmmo.gameObject.SetActive(false);
                                    MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.SetReloadTime(_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().reloadTime);
                                    _playerRef.SecondaryUseTool(_playerRef.team, onReloadTool, _playerRef.CallbackReloadTool);
                                    Invoke("isReloadingReset", _playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().reloadTime);
                                }
                                //MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().lowAmmo.lowAmmoText.text = "No Ammo";
                            }
                        }
                       

                      
                    }
                    

                    
                    // MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo(weaponSlots[equippedWeapon]);
                    //MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo(weaponSlots[equippedWeapon].gameObject);

                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>() != null)
                    {
                        //if (isReloading == false)
                        //{
                             if (_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().inReload == false && _playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().inUse == false && _playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().canUse == true)
                             {
                                isReloading = true;
                            MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().lowAmmo.gameObject.SetActive(false);
                            MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.SetReloadTime(_playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().reloadTime);
                                _playerRef.SecondaryUseTool(_playerRef.team, onReloadTool, _playerRef.CallbackReloadTool);
                                Invoke("isReloadingReset", _playerRef.weaponSlots[_playerRef.equippedWeapon].gameObject.GetComponent<RangedWeapon>().reloadTime);
                            }
                        
                            
                        //}

                    }
                   
                   
                    // MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.PlayNotif();


                }


            }
            _playerRef.Flip(Camera.main.ScreenToViewportPoint(Input.mousePosition).x);
        }
    }

    public void isReloadingReset()
    {
        isReloading = false;
    }

    private void FixedUpdate()
    {
        if (_playerRef.canMove)
        {

            if (movementDirection != new Vector2(0f, 0f))
            {
                _playerRef.transform.hasChanged = true;
                _playerRef.rb.MovePosition(_playerRef.rb.position + (movementDirection * Time.deltaTime * _playerRef.movementSpeed));
                
            }
            else if(movementDirection == new Vector2(0f, 0f))
            {

                _playerRef.transform.hasChanged = false;
            }
            if (_playerRef.transform.hasChanged)
            {
                if (_playerRef.anim.GetBool("Running") == false)
                {
                    _playerRef.anim.SetBool("Running", true);
                   
                }
            }
            else if(!_playerRef.transform.hasChanged)
            {
                if(_playerRef.anim.GetBool("Running") == true)
                {
                    _playerRef.anim.SetBool("Running", false);
        
                }
                
               
            }

            PlayerManager.instance.playerCamera.CameraFollow(_playerRef.rb.position);
        }



    }

}
