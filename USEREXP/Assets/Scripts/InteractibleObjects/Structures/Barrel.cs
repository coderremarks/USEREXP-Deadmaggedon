using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Barrel : Structure
{
    public ParticleSystem explosionParticle;
    public Collider2D[] nearbyObjects;
    public float detectionRadius;
    public float explosionDamage;
    
    
    public override void Death()
    {
  
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        explosionParticle.Play();
        nearbyObjects = Physics2D.OverlapCircleAll(this.transform.position, detectionRadius); //Detecting objects around this ai
                                                                                              // Debug.DrawRay(weaponSlots[equippedWeapon].firePoint.position, weaponSlots[equippedWeapon].firePoint.right * (weaponSlots[equippedWeapon].range + this.transform.localScale.x / 4), Color.red);
                                                                                              //Attack if something is within its attack range (this on in update)
                                                                                              // Debug.Log("PHASE 1: " + hit.collider + " - " + Vector2.Distance(this.transform.position, hit.collider.transform.position) + " - " + detectionRadius);
        foreach (Collider2D detectedObject in nearbyObjects) //Lists down the nearest instance of the targets unit/structure/goal
        {
            if (detectedObject.CompareTag("Player") || detectedObject.CompareTag("Enemy") || detectedObject.CompareTag("Ally") || detectedObject.CompareTag("Structure"))
            {


                if (Vector2.Distance(transform.position, detectedObject.transform.position) < detectionRadius)//if the last saved unitTarget's position is less than the newly found unit, save the newly found unit as the new unitTarget
                {
                    detectedObject.GetComponent<InteractibleObject>().DamageHealth(explosionDamage);
                    // anim.SetFloat("unitDistance", Vector2.Distance(transform.position, unitTarget.transform.position));
                }


            }

        }
        anim.SetTrigger("Explode");
        Invoke("DelayDestroy", 2f);

       // Destroy(this.gameObject);
    }

    public void DelayDestroy()
    {
        Destroy(this.gameObject);
    }
   



}
