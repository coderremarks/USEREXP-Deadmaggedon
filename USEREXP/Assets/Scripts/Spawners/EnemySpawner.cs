using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public bool activated;

    public float spawnRate;
    public int amount = 0;
    public int maxAmount = 2;

    public void Awake()
    {
        activated = true;
    }
    public void RespawnEnemy()
    {
        if (activated == true)
        {

            if (amount < maxAmount)
            {
          
                amount++;
                StartCoroutine(SpawnEnemy());
            }
            else
            {
                activated = false;
                
            }
        }



    }


    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(spawnRate);
        var newDummy = EnemyPool.instance.GetObject(0);
    
        newDummy.onDeath += TutorialManager.instance.EnemyDied;
        RoundManager.instance.currentHostileBotCount += 1;
        newDummy.gameObject.SetActive(true);
        newDummy.transform.position = transform.position;
        

        newDummy.InitializeStats();

    }
}
