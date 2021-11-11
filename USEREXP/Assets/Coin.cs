using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float projectileDecayTime;
    private void OnEnable()
    {
        StartCoroutine(DecayTimer(projectileDecayTime));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.gameObject.GetComponent<Player>() != null)
    //    {

    //        PlayerManager.instance.BuildingMaterials += 1;
    //        CoinPool.instance.ReturnToPool(this);
    //    }



    //}
    public void Update()
    {
        transform.Translate(new Vector2(0,1)* 1f *Time.deltaTime);
    }
    public IEnumerator DecayTimer(float decayTime)
    {
        float count = decayTime;
        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }

 
        // ProjectilePool.instance.ReturnToPool(this);
        CoinPool.instance.ReturnToPool(this);
    }



}
