using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractibleObject
{

    public bool shakable;
    [SerializeField] protected bool invincible;

    public override void OnEnable()
    {
        shakable = true;
        invincible = true;
   
    }
    public override void DamageHealth(float damage, string originName)
    {
        //if (shakable == true)
        //{
        //    PlayerManager.instance.playerCamera.CameraShake(0.5f, 0.5f, 10f);

        //}
        if (invincible == false)
        {
            base.DamageHealth(damage);
            
     
           
        }

    }

    
    public override void Death()
    {
       
    
        Destroy(this.gameObject);
    }


}
