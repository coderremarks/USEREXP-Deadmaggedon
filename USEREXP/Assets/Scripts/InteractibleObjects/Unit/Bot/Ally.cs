using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Ally : Bot
{
    // Start is called before the first frame update
    public override void Start()
    {
        botTag = "Enemy";
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        weaponSlots[equippedWeapon].Equip(gameObject.name);
        defaultTarget = null;
        base.Start();
        InitializeStats();
 

      
        

    }

    public override void InitializeStats()
    {
        base.InitializeStats();

    }

    public override void OnEnable()
    {

    
        InitializeStats();
    }
    public override void OnDisable()
    {

    }
    // Update is called once per frame
    protected override void Update()
    {
        if (isAlive == true)
        {
            base.Update();
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

    public override void Death()
    {
        base.Death();
      //  AllyPool.instance.ReturnToPool(this);

    }
}
