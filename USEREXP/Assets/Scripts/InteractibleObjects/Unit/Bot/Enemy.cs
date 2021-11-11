using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Enemy : Bot
{
  
    public GameObject playerTarget;
    public GameObject goalTarget;
    public Action onDeath;
    public bool attacking;
    public bool dying;
    public void Awake()
    {
        //botTag = "Ally";
        //playerTarget = PlayerManager.instance.player.gameObject;
        //goalTarget = RoundManager.instance.goalObject;
        //defaultTarget = goalTarget;
        
        //navAgent = GetComponent<NavMeshAgent>();
        //navAgent.updateRotation = false;
        //navAgent.updateUpAxis = false;
        //weaponSlots[equippedWeapon].Equip(this);
    }
    // Start is called before the first frame update
    public override void Start()
    {

        //base.Start();
    

        weaponSlots[equippedWeapon].Equip(gameObject.name);
        weaponSlots[equippedWeapon].canUse = true;
        bloodParticle = this.GetComponent<ParticleSystem>();

        botTag = "Ally";
        playerTarget = PlayerManager.instance.player.gameObject;
        goalTarget = RoundManager.instance.goalObject;
        defaultTarget = goalTarget;

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
       


    }
    public override void DamageHealth(float damage, string originName = null)
    {


        if (GetComponent<Renderer>().isVisible == true && originName == "Player")
        {
            PlayerManager.instance.playerCamera.CameraShake(0.05f, 0.075f, 0f);
          //  StartCoroutine(GameManager.instance.WaitRealTime(StartTimeImpact, EndTimeImpact, 0.0125f));
        }
        


        base.DamageHealth(damage);

    }

    public override void InitializeStats()
    {

        base.InitializeStats();
        
        anim.SetInteger("State", 2);
        //anim.SetBool("isAlive", true);
        isAlive = true;
        canMove = true;
        attacking = false;
        weaponSlots[equippedWeapon].Equip(gameObject.name);
        //anim.SetBool("isAlive",true);
        //default target is goal
        if (goalTarget != null)
        {
            currentTarget = goalTarget.transform;
        }
       //if (currentTarget != null)
       // {
       //     navAgent.SetDestination(currentTarget.position);
       // }
   

    }
    // Update is called once per frame
    protected override void Update()
    {
        if (isAlive == true)
        {
            if (canMove == true)
            {
                //base.Update();
                //ATTACK
                hit = Physics2D.Raycast(weaponSlots[equippedWeapon].firePoint.position, weaponSlots[equippedWeapon].firePoint.right, weaponSlots[equippedWeapon].range + this.transform.localScale.x / 4, targetLayerMask);
                nearbyObjects = Physics2D.OverlapCircleAll(this.transform.position, detectionRadius); //Detecting objects around this ai
                Debug.DrawRay(weaponSlots[equippedWeapon].firePoint.position, weaponSlots[equippedWeapon].firePoint.right * (weaponSlots[equippedWeapon].range + this.transform.localScale.x / 4), Color.red);
                //Attack if something is within its attack range (this on in update)
    
                if (hit.collider != null && Vector2.Distance(this.transform.position, hit.collider.transform.position) < detectionRadius)
                {
              
                    if (weaponSlots[equippedWeapon].canUse == true && hit.collider.gameObject.GetComponent<InteractibleObject>().team != team)
                    {
                       // Debug.Log("PHASE 3: " + "Should be attacking");
                        //UseTool(team);
                        PrimaryUseTool(team);
                        //anim.SetTrigger("Attack");
                        if (attacking == false)
                        {
                            anim.SetInteger("State", 4);
                            attacking = true;
                            Invoke("ResetAttacking", 0.5f);
                        }
                     
                    }
               


                }
                //AIMING/LOOKING
                //if it has a target (this one is in update)
                if (currentTarget != null)
                {
                    if (CheckIfCurrentTargetStillInRange() == true || defaultTarget != null && defaultTarget.transform.position == currentTarget.transform.position)
                    {

                        directionToTargetPosition = (Vector2)currentTarget.position - (Vector2)transform.position;

                        directionToTargetPosition.Normalize();

                        weaponSlots[equippedWeapon].gameObject.transform.right = Vector3.Slerp(weaponSlots[equippedWeapon].gameObject.transform.right, directionToTargetPosition, lookSpeed * Time.deltaTime);
                        //weaponSlots[equippedWeapon].gameObject.transform.right = directionToTargetPosition;
                        Flip(directionToTargetPosition.x);
                    }




                }


                FindNearestBotTarget(this.transform.position);

                //MOVEMENT
                
                if (currentTarget != null)
                {
                    if (Vector2.Distance(this.transform.position, playerTarget.transform.position) <= detectionRadius) //player is targetted (2nd priority) 
                    {

                        currentTarget = playerTarget.transform;
                        navAgent.SetDestination(currentTarget.position);

                    }

                    else if (Vector2.Distance(this.transform.position, currentTarget.position) > detectionRadius && currentTarget != goalTarget.transform)//Check If CurrentTarget not within Range anymore
                    {

                        currentTarget = null;
                        if (botTarget != null)
                        {
                            botTarget = null;
                        }
                    }
                    if (botTarget != null)
                    {

                        if (Vector2.Distance(this.transform.position, botTarget.transform.position) <= unitPriorityDistance) //player is targetted (1st priority) 
                        {
                            // Debug.Log("FIGHT");
                            currentTarget = botTarget.transform;
                            navAgent.SetDestination(botTarget.transform.position);

                        }
                    }



                    if (currentTarget.gameObject.activeSelf == false)
                    {
                        currentTarget = null;
                        if (botTarget != null)
                        {
                            botTarget = null;
                        }
                    }

                }

                if (currentTarget == null)
                {
                    if (Vector2.Distance(this.transform.position, playerTarget.transform.position) <= detectionRadius) //player is targetted if not 
                    {

                        currentTarget = playerTarget.transform;
                        navAgent.SetDestination(currentTarget.position);

                    }
                    else if (goalTarget != null)//goal is default target  (lowest priority)
                    {
                        currentTarget = goalTarget.transform;
                        navAgent.SetDestination(currentTarget.position);
                    }

                    if (botTarget != null) //if unit target exists
                    {
                        if (Vector2.Distance(this.transform.position, botTarget.transform.position) <= unitPriorityDistance || currentTarget == goalTarget.transform) //if nearest unit is within the priority distance, chase unit
                        {
                            currentTarget = botTarget.transform;
                            navAgent.SetDestination(currentTarget.position);
                        }
                    }

                }
            }

            if (attacking == false)
            {
                if (navAgent.velocity != Vector3.zero)
                {
                    //Debug.Log("AI: MOVING");
                    anim.SetInteger("State", 3);
                    //anim.SetBool("Walking", true);
                }
                else if (navAgent.velocity == Vector3.zero)
                {
                    //Debug.Log("AI: NOT MOVING");
                    anim.SetInteger("State", 2);
                    //anim.SetBool("Walking", false);
                }
            }
          
        }
    }

    public void ResetAttacking()
    {
        attacking = false;
    }
    public void MoveDelay()
    {
      

        InitializeStats();
        if (currentTarget != null)
        {
            navAgent.SetDestination(currentTarget.position);
        }

    }
    public override void Death()
    {
      
       base.Death();
            
            navAgent.isStopped = true;
            audsrc.PlayOneShot(audsrc.clip);
            // PlayerManager.instance.BuildingMaterials += 1; //ZOMBIE REWARD

            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            anim.SetInteger("State", 0);
            //anim.SetBool("isAlive", false) ;
           
        if (isAlive == false && dying == false)
        {
            dying = true;
            Invoke("RealDeath", 1);
            //  gameObject.SetActive(false);
        }


    }
    public void RealDeath()
    {
        gameObject.SetActive(false);
        RoundManager.instance.currentHostileBotCount--;
        EnemyPool.instance.ReturnToPool(this);
        var newCoin = CoinPool.instance.GetObject(0);
        newCoin.gameObject.transform.position = transform.position;

        PlayerManager.instance.BuildingMaterials += 1; //ZOMBIE REWARD
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().buildMaterials.UpdateCounter(PlayerManager.instance.BuildingMaterials);


        onDeath?.Invoke();

    }
    public void StartTimeImpact()
    {
        Time.timeScale = 0f;
    }
    public void EndTimeImpact()
    {
        Time.timeScale = 1f;
    }

    public override void OnEnable()
    {
        dying = false;
        //RoundManager.instance.currentHostileBotCount+=1;
        onTakeDamage += DamageHealth;
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        isAlive = false;
        canMove = false;
        //anim.SetTrigger("Spawned");
        anim.SetInteger("State", 1);
        
        Invoke("MoveDelay", 0.75f);
      
    }
    public override void OnDisable()
    {
        onTakeDamage -= DamageHealth;
  


        
        
    }
   
}
