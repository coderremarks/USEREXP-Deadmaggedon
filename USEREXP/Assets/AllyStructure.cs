using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyStructure : Structure
{
    public StructureStats structureTemplate;
    public string botTag;
    [SerializeField] protected Transform currentTarget;
    public GameObject defaultTarget;
    public GameObject botTarget;
    [SerializeField] public int equippedWeapon;
    [SerializeField] public Weapon[] weaponSlots;
    [SerializeField] public int amountOfSlots;
    public Collider2D[] nearbyObjects;
    public RaycastHit2D hit;
    public ParticleSystem bloodParticle;
    public float detectionRadius;
    public float unitPriorityDistance;
    [SerializeField] protected Vector2 directionToTargetPosition;
    public float lookSpeed;
    [SerializeField] public bool isFacingRight;
    public LayerMask targetLayerMask;

    // Start is called before the first frame update
    public override void Start()
    {
        botTag = "Enemy";
       
        defaultTarget = null;
        base.Start();
     //   InitializeStats();
        weaponSlots[equippedWeapon].Equip(gameObject.name);
        weaponSlots[equippedWeapon].canUse = true;
        bloodParticle = this.GetComponent<ParticleSystem>();





    }

    public virtual void InitializeStats()
    {
     

        team = structureTemplate.team;
        isAlive = structureTemplate.isAlive;
        maxHealth = structureTemplate.maxHealth;
        currentHealth = maxHealth;
     
    }

    public override void OnEnable()
    {


       InitializeStats();
    }
    public override void OnDisable()
    {

    }
    // Update is called once per frame
    public void Update()
    {
        if (isAlive == true)
        {
            checkCanShoot();
            //base.Update();
            //if it has a target (this one is in update)
            if (currentTarget != null)
            {


                currentTarget = null;



            }

            if (currentTarget == null)
            {
                if (botTarget != null) //if unit target exists
                {

                    currentTarget = botTarget.transform;


                }
            }
        }




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

    public virtual void PrimaryUseTool(int team)
    {
        weaponSlots[equippedWeapon].PrimaryUse(team);

        // onUseTool?.Invoke(team);

    }
    public void checkCanShoot()
    {

        hit = Physics2D.Raycast(weaponSlots[equippedWeapon].firePoint.position, weaponSlots[equippedWeapon].firePoint.right, weaponSlots[equippedWeapon].range + this.transform.localScale.x / 4, targetLayerMask);
        nearbyObjects = Physics2D.OverlapCircleAll(this.transform.position, detectionRadius); //Detecting objects around this ai
        Debug.DrawRay(weaponSlots[equippedWeapon].firePoint.position, weaponSlots[equippedWeapon].firePoint.right * (weaponSlots[equippedWeapon].range + this.transform.localScale.x / 4), Color.red);
        //Attack if something is within its attack range (this on in update)
        // Debug.Log("PHASE 1: " + hit.collider + " - " + Vector2.Distance(this.transform.position, hit.collider.transform.position) + " - " + detectionRadius);
        if (hit.collider != null && Vector2.Distance(this.transform.position, hit.collider.transform.position) < detectionRadius)
        {
            //  Debug.Log(hit.collider.gameObject.name + gameObject.name);
            //Debug.Log("PHASE 2: " + weaponSlots[equippedWeapon].canUse + " - " + hit.collider.gameObject.GetComponent<InteractibleObject>().team);
            if (weaponSlots[equippedWeapon].canUse == true && hit.collider.gameObject.GetComponent<InteractibleObject>().team != team && weaponSlots[equippedWeapon].gameObject.GetComponent<RangedWeapon>().currentAmmoInMag > 0)
            {
                Debug.Log("PHASE 3: " + "Should be attacking");
                //UseTool(team);
                PrimaryUseTool(team);

            }
            else if (weaponSlots[equippedWeapon].canUse == false || weaponSlots[equippedWeapon].gameObject.GetComponent<RangedWeapon>().currentAmmoInMag <= 0)
            {
                if (weaponSlots[equippedWeapon].gameObject.GetComponent<RangedWeapon>() != null)
                {
                    Debug.Log("RELOOOOOOOOOOOOOOOOOOOOOOOADING");
                    weaponSlots[equippedWeapon].gameObject.GetComponent<RangedWeapon>().Reload(team);
                }

            }


        }



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




    }


    protected void FindNearestBotTarget(Vector2 originPosition)
    {
        foreach (Collider2D detectedObject in nearbyObjects) //Lists down the nearest instance of the targets unit/structure/goal
        {
            if (detectedObject.CompareTag(botTag))
            {
                if (botTarget != null)
                {

                    if (Vector2.Distance(originPosition, botTarget.transform.position) > Vector2.Distance(originPosition, detectedObject.transform.position))//if the last saved unitTarget's position is less than the newly found unit, save the newly found unit as the new unitTarget
                    {
                        botTarget = detectedObject.gameObject;
                        // anim.SetFloat("unitDistance", Vector2.Distance(transform.position, unitTarget.transform.position));
                    }

                }
                else
                {
                    botTarget = detectedObject.gameObject;

                }
            }

        }
    }

    protected bool CheckIfCurrentTargetStillInRange()
    {
        foreach (Collider2D detectedObject in nearbyObjects) //Lists down the nearest instance of the targets unit/structure/goal
        {
            if (currentTarget == detectedObject.gameObject.transform)
            {
                return true;
            }
        }
        return false;
    }
    public override void DamageHealth(float damage, string originName = null)
    {
        base.DamageHealth(damage);
        _sr.material = _whiteMat;
        bloodParticle.Play();
        Invoke("AllyResetMaterial", 0.025f);
        /*var particle = ParticlePool.instance.GetObject(0);
        particle.transform.position = this.transform.position;*/

    }

    void AllyResetMaterial()
    {
        _sr.material = defaultMat;
    }

    public override void Death()
    {
       
        var bloodPuddle = ParticlePropPool.instance.GetObject(0);
        bloodPuddle.gameObject.transform.position = this.transform.position;
        bloodPuddle.gameObject.transform.localScale = this.transform.localScale + new Vector3(1.5f, 1.5f, 1.5f);
        StructurePool.instance.ReturnToPool(this);
        // base.Death();
    }
}
