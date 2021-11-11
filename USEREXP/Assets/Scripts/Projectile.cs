using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Projectile : MonoBehaviour
{
    public string originName;
    public int userTeam;
    public float projectileDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("HIT: " + collision.gameObject.name);
        if (collision.gameObject.GetComponent<InteractibleObject>() != null)
        {
           // Debug.Log("HIT2");
            if (collision.gameObject.GetComponent<InteractibleObject>().team != userTeam)
            {
               // Debug.Log("HIT3");
                collision.GetComponent<InteractibleObject>().onTakeDamage?.Invoke(projectileDamage, originName);
                Debug.Log(collision.gameObject.name);
                ProjectilePool.instance.ReturnToPool(this);
            }
        }
      
        else
        {
           // Debug.Log("IT HIT BUT ITS NOT SEEING ITS INTERACTIBLE");
            if (collision.gameObject.GetComponent<SpriteRenderer>() != null)
            {

                if (collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder > gameObject.GetComponent<SpriteRenderer>().sortingOrder)
                {
                    ProjectilePool.instance.ReturnToPool(this);
                }
            }
            else if (collision.gameObject.GetComponent<TilemapRenderer>() != null)
            {
                if (collision.gameObject.GetComponent<TilemapRenderer>().sortingOrder > gameObject.GetComponent<SpriteRenderer>().sortingOrder)
                {
                    ProjectilePool.instance.ReturnToPool(this);
                }
            }
        }
        


    }
 
    public IEnumerator DecayTimer(float decayTime)
    {
        yield return new WaitForSeconds(decayTime);
        ProjectilePool.instance.ReturnToPool(this);
    }


    private void OnDisable()
    {
        this.gameObject.GetComponent<TrailRenderer>().Clear();
    }

}
