using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public string userUnit;
    [SerializeField] public bool inUse;
    [SerializeField] public bool canUse;
    [SerializeField] public Vector2 offset;
    [SerializeField] public Transform firePoint;
    [SerializeField] public Sprite damageSourceSprite;
    [SerializeField] public float damage;
    [SerializeField] public float firerate;
    [SerializeField] public float range;
    public Animator anim;
    public AudioSource audsrc;
    public AudioSource relsrc;
    public virtual void Equip(string owner)
    {


    }

    public virtual void Unequip()
    {

    }
    
    public virtual void OnEnable()
    {
        inUse = false;
        canUse = true;
    }

    public virtual void OnDisable()
    {

    }

    public virtual void PrimaryUse(int team)
    {

    }

    public virtual void SecondaryUse(int team, Action preFunction = null, Action callback = null)
    {

    }
    public IEnumerator UsageCoolDown()
    {

        yield return new WaitForSeconds(1f / (firerate / 60f));
        inUse = false;
    }

}
