using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.Rendering.Universal;
public class RangedWeapon : Weapon
{
    public int fractures;
    [SerializeField] public Projectile prefab;
    [SerializeField] public bool inReload;
  //  [SerializeField] public bool canReload;
    [SerializeField] public float reloadTime;
    [SerializeField] public int currentAmmoInMag;
    [SerializeField] public int AmmoInMagCapacity;
    public int spread;

    [SerializeField] public float projectileSpread;
    [SerializeField] public float projectileSpeed;
    [SerializeField] public float projectileDecayTime;


    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        Unequip();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        inReload = false;
        currentAmmoInMag = AmmoInMagCapacity;
    }
    public override void Equip(string owner)
    {
        userUnit = owner;
        //Unit.onUseTool+= Shoot;
        //Unit.onReloadTool += Reload;

    }

    public override void Unequip()
    {
        //Unit.onUseTool -= Shoot;
        //Unit.onReloadTool -= Reload;
    }

    public override void PrimaryUse(int team)
    {
        Shoot(team);
    }

    public override void SecondaryUse(int team, Action preFunction = null, Action callback = null)
    {
        Reload(team, preFunction, callback);
    }

    public void Shoot(int userTeam)
    {

        if (currentAmmoInMag > 0 && canUse == true && inUse == false && inReload == false)
        {
            inUse = true;
            StartCoroutine(UsageCoolDown());
            currentAmmoInMag -= 1;
            audsrc.PlayOneShot(audsrc.clip);
            for (int i = 0; i <fractures; i++)
            {
                float offset = UnityEngine.Random.Range(-spread, spread);
                var newProjectileInstance = ProjectilePool.instance.GetObject(prefab);
                newProjectileInstance.projectileDamage = damage;
                newProjectileInstance.userTeam = userTeam;
                
                newProjectileInstance.gameObject.transform.position = firePoint.transform.position;
                //newProjectileInstance.transform.rotation = Quaternion.LookRotation(firePoint.transform.right) * Quaternion.Euler(new Vector3(0, offset, 0));

                newProjectileInstance.gameObject.transform.right = firePoint.transform.right;
                newProjectileInstance.transform.Rotate(0,0,offset);
                newProjectileInstance.StartCoroutine(newProjectileInstance.DecayTimer(projectileDecayTime));
                newProjectileInstance.gameObject.GetComponent<ConstantForce2D>().force = newProjectileInstance.transform.right * projectileSpeed;
                newProjectileInstance.originName = userUnit;
            }

            firePoint.GetComponent<Light2D>().enabled = true;
            StartCoroutine(flashOver());
            //Object pool
        }
   
    }

    public IEnumerator flashOver()
    {
        yield return new WaitForSeconds(0.05f);
        firePoint.GetComponent<Light2D>().enabled = false;
    }


    public IEnumerator Reloading(Action callback = null)
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmoInMag = AmmoInMagCapacity;
        inReload = false;
        canUse = true;
        
        callback?.Invoke();
        //userUnit?.CallbackReloadTool(currentAmmoInMag.ToString());
        
    }
    public void Reload( int userTeam, Action preFunction = null ,Action callback = null)
    {
        
        if (inReload == false &&  inUse == false && canUse == true)
        {
     
            inReload = true;
            canUse = false;
            preFunction?.Invoke();
            if (relsrc != null)
            {
                relsrc.PlayOneShot(relsrc.clip);
            }
       
            StartCoroutine(Reloading(callback));
            if (anim != null)
            {
                anim.SetTrigger("Reloading");
            }
            
      
        }

    }


}
