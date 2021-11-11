using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MeleeWeapon : Weapon
{
    public Particle prefab;

 
    public override void Equip(string owner)
    {
        userUnit = owner;
     //   Unit.onUseTool += Slash;
       

    }

    public override void Unequip()
    {
   //     Unit.onUseTool -= Slash;

    }
    public override void PrimaryUse(int team)
    {
        Slash(team);
    }

    public override void SecondaryUse(int team, Action preFunction, Action callback = null)
    {
     //   Reload(team);
    }
    public void Update()
    {
        //Debug.DrawRay(firePoint.transform.position, firePoint.transform.right * range, Color.green);
    }
    void Slash(int userTeam)
    {
        if (canUse == true && inUse == false)
        {
            inUse = true;
         //   LayerMask playerLayer = LayerMask.NameToLayer("Player");
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range,gameObject.transform.parent.gameObject.GetComponent<Unit>().targetLayerMask);
        
            if (hit.collider != null)
            {
           
              
                if (hit.collider.transform.root.gameObject.GetComponent<InteractibleObject>().team != userTeam)
                {

                    audsrc.PlayOneShot(audsrc.clip);
                    hit.collider.gameObject.GetComponent<InteractibleObject>().onTakeDamage?.Invoke(damage,userUnit);
                    var newParticle = ParticlePool.instance.GetObject(prefab);
                    newParticle.transform.position = hit.collider.gameObject.transform.position;
                }
              

                
              
            }
            StartCoroutine(UsageCoolDown());
        }
    }
}
