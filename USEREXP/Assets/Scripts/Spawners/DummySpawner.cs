using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class DummySpawner : MonoBehaviour
{
   
    public bool activated;

    public float spawnRate;
    public int amount = 1;
    public int minQuestAmount = 1;
  
    public void Awake()
    {
        activated = true;
    }
    public void RespawnDummy()
    {
        if (activated == true)
        {
            
            if (amount >= minQuestAmount)
            {

                TutorialManager.instance.Get(TutorialPurpose.combat).finished = true;
                TutorialManager.instance.CombatEnd();
                activated = false;
            }
            else
            {
                amount++;
                StartCoroutine(SpawnDummy());
            }
        }
        
       
      
    }


    IEnumerator SpawnDummy()
    {
        yield return new WaitForSeconds(spawnRate);
        var newDummy = DummyPool.instance.GetObject(0);

        newDummy.transform.position = transform.position;
        newDummy.spawner = this;

      //  newDummy.InitializeStats();

    }

 



}
