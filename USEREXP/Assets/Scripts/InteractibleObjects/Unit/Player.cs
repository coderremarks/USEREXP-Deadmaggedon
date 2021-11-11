using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player : Unit
{

    public GameObject mouseCursor;

    //[SerializeField] public Vector2 movementDirection;

    [SerializeField] protected int blinkAmount;
    public CharacterController characterController;

   
    [SerializeField] protected float invincibilityTime;
    [SerializeField] protected float invincibilityBlinkTime;
    [SerializeField] protected float invincibilityUIRestTime;



    //public static event Action onUseTool; //Pre Version 0.55 no static event in all actions
    //public static Action onReloadTool;

   // public static event Func<>
    // Start is called before the first frame update
   

    public override void Start()
    {
        //onReloadFinished += test;
        base.Start();
        
        // MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().healthBar.SetMaxHealth(maxHealth);

       


       // onCallbackReloadTool += CallbackReloadTool;
        weaponSlots[equippedWeapon].Equip(gameObject.name);
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.SetAmmoImage(weaponSlots[equippedWeapon].damageSourceSprite);
 
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo();
     
        blinkAmount = 3;
        invincible = false;
        invincibilityTime = 1f;
        invincibilityBlinkTime = 0.05f;
        invincibilityUIRestTime = 0.075f;
        PlayerManager.instance.playerCamera.CameraFollow(rb.position);
        
        //   Cursor.SetCursor(mouseCursor, new Vector2(mouseCursor.width/2f,mouseCursor.height/2f), CursorMode.ForceSoftware); //move to player manager
       

    }

    public override void OnEnable()
    {
        base.OnEnable();
        base.InitializeStats();
        invincible = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        weaponSlots[equippedWeapon] = PlayerManager.instance.currentlyEquippedWeapon.GetComponent<Weapon>();
        //onReloadTool += MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.PlayNotif;
        //onUseTool += MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        //onReloadTool -= MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.PlayNotif;
        //onUseTool -= MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo;
    }
    public override void DamageHealth(float damage, string originName = null)
    {
        if (invincible == false && currentHealth > 0)
        {
            audsrc.PlayOneShot(audsrc.clip);
            currentHealth -= damage;
            MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().healthBar.SetHealth(currentHealth);
            //PlayerManager.instance.playerCamera.CameraShake(0.5f,0.25f,10f);
            PlayerManager.instance.playerCamera.CameraBounceIn(6.5f,3f,invincibilityTime);
            CheckHealth();
            StartCoroutine(InvincibilityTime());
        }

    }
    public override void PrimaryUseTool(int team)
    {
        base.PrimaryUseTool(team);
       // onUseTool?.Invoke();

    }

    public  void CallbackReloadTool()
    {
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.StopNotif();
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo();
   

        
    }
    IEnumerator InvincibilityTime()
    {
        invincible = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.075f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(0.03f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;


        for (int currentBlinkAmount = 0; currentBlinkAmount < blinkAmount; currentBlinkAmount++)
        {

            gameObject.GetComponent<SpriteRenderer>().enabled = true;


            yield return new WaitForSeconds(invincibilityBlinkTime);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //  chosenDirection.arrowsUI.SetActive(false);


            yield return new WaitForSeconds(invincibilityUIRestTime);
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(invincibilityTime);

        invincible = false;

    }
    public override void Death()
    {
        //base.Death();

        MenuManager.instance.GetCanvas(MenuType.GameOver).GetComponent<GameOverCanvas>().CauseOfDeathText.text = "Player Died";
        GameManager.instance.GameOver();

    }

    public override void Flip(float mouseX)
    {
        if (mouseX < 0.5 && isFacingRight || mouseX > 0.5 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            //  transform.Rotate(new Vector3(0, 180, 0));
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
            weaponSlots[equippedWeapon].gameObject.GetComponent<SpriteRenderer>().flipY = !weaponSlots[equippedWeapon].gameObject.GetComponent<SpriteRenderer>().flipY;
            /* Vector3 setScale = weaponSlots[equippedWeapon].gameObject.transform.localScale;
             setScale.Set(weaponSlots[equippedWeapon].gameObject.transform.localScale.x, weaponSlots[equippedWeapon].gameObject.transform.localScale.y * -1f, weaponSlots[equippedWeapon].gameObject.transform.localScale.z);
             weaponSlots[equippedWeapon].gameObject.transform.localScale = setScale;*/

        }
    }


    // Update is called once per frame
    void Update()
    {

        //if (RoundManager.instance.isPlaying == true && canMove)
        //{
        //    //Character Input
        //    movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //    movementDirection.Normalize();

        //    //Aim with mouse

        //    Vector2 mousePos = PlayerManager.instance.playerCamera.camera.ScreenToWorldPoint(Input.mousePosition);



        //    if (weaponSlots[equippedWeapon])
        //    {


        //        //weaponSlots[equippedWeapon].gameObject.transform.right= new Vector2(mousePos.x,mousePos.y) - (Vector2) rb.transform.position;
        //        mousePos -= (Vector2)rb.transform.position;

        //        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        //        weaponSlots[equippedWeapon].gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        //        if (Input.GetMouseButton(0))
        //        {

        //            PrimaryUseTool(team);
        //            if (weaponSlots[equippedWeapon].gameObject.GetComponent<RangedWeapon>()!= null && weaponSlots[equippedWeapon].gameObject.GetComponent<RangedWeapon>().currentAmmoInMag > 0 )
        //            {
        //                PlayerManager.instance.playerCamera.CameraRecoil(0.005f, 0.1f,0f);
        //            }
        //            // MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo(weaponSlots[equippedWeapon]);
        //            //MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.UpdateAmmo(weaponSlots[equippedWeapon].gameObject);

        //        }
          
        //        if (Input.GetKey(KeyCode.R))
        //        {
                  
        //            SecondaryUseTool(team, onReloadTool, CallbackReloadTool);
        //           // MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.PlayNotif();


        //        }
               

        //    }
        //    Flip(Camera.main.ScreenToViewportPoint(Input.mousePosition).x);
        //}
  
            


    }
    //private void FixedUpdate()
    //{
    //    if (canMove)
    //    {

    //        if (movementDirection != new Vector2(0f, 0f))
    //        {
    //            rb.MovePosition(rb.position + (movementDirection * Time.deltaTime * movementSpeed));

    //        }

    //        PlayerManager.instance.playerCamera.CameraFollow(rb.position);
    //    }

        
   
    //}

 
}
