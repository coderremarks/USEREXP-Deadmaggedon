using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InteractibleObject : MonoBehaviour
{
    public int team;
    [SerializeField] public bool isAlive;
    public float currentHealth;
    public float maxHealth;
    public Action<float, string> onTakeDamage;

    public virtual void Start()
    {
        onTakeDamage += DamageHealth;
    }
    public virtual void DamageHealth(float damage, string originName = null)
    {

        currentHealth -= damage;
        CheckHealth();


    }

    public virtual void OnEnable()
    {
       

    }

    public virtual void OnDisable()
    {
        
    }

    public virtual void CheckHealth()
    {


        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            isAlive = false;
            Death();
        }
    }
    public virtual void Death()
    {
       
    }
}
