using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Unit : InteractibleObject
{
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public UnitStats unitTemplate;

    [SerializeField] public bool isFacingRight;
    public Transform gripPoint;
    public bool canMove;
    [SerializeField] protected bool invincible;

    public LayerMask targetLayerMask;
    [SerializeField] public int equippedWeapon;
    [SerializeField] public Weapon[] weaponSlots;
    [SerializeField] public int amountOfSlots;


    [SerializeField] public float movementSpeed;
    public Animator anim;

    public AudioSource audsrc;
    //public static event Action<int> onUseTool; //Pre Version 0.55 no static event in all actions
    //public static event Action<int> onReloadTool;
    //public static event Action<string> onCallbackReloadTool;

    public override void OnEnable()
    {
        base.OnEnable();
        InitializeStats();
    }
    public virtual void InitializeStats()
    {
        canMove = true;
        team = unitTemplate.team;
        isAlive = unitTemplate.isAlive;
        maxHealth = unitTemplate.maxHealth;
        currentHealth = maxHealth;
        movementSpeed = unitTemplate.movementSpeed;

        

    
        //weaponSlots = new GameObject[amountOfSlots];
        //if (amountOfSlots > 0)
        //{
        //    equippedWeapon = 0; //First Weapon is default equipped
        //}

      
    }

    public virtual void PrimaryUseTool(int team)
    {
        weaponSlots[equippedWeapon].PrimaryUse(team);

       // onUseTool?.Invoke(team);
        
    }

    public virtual void SecondaryUseTool(int team, Action preFunction = null, Action callback = null)
    {
        weaponSlots[equippedWeapon].SecondaryUse(team, preFunction, callback);
        //  onReloadTool?.Invoke(team);
    }

    public virtual void CallbackReloadTool(string text)
    {
        
    }

    public virtual void Flip(float directionX)
    {
        if (directionX < 0.5 && isFacingRight || directionX > 0.5 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
          
            gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
            if (weaponSlots[equippedWeapon] != null)
            {

                weaponSlots[equippedWeapon].GetComponent<SpriteRenderer>().flipY = !weaponSlots[equippedWeapon].GetComponent<SpriteRenderer>().flipY;
            }
            

        }
    }

    public override void Death()
    {
        base.Death();
        var bloodPuddle = ParticlePropPool.instance.GetObject(0);
        bloodPuddle.gameObject.transform.position = this.transform.position;
        bloodPuddle.gameObject.transform.localScale = this.transform.localScale + new Vector3(1.5f,1.5f,1.5f);
    }
}
