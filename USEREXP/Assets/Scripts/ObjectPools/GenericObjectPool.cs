using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class ObjectPool<T>
{
    [SerializeField] public T prefab;
    [SerializeField] public List<T> activeObjectsList = new List<T>();
    [SerializeField] public Queue<T> objectsQueue = new Queue<T>();
    
}

public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
{

    public static GenericObjectPool<T> instance;
    public List<ObjectPool<T>> objectPool = new List<ObjectPool<T>>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
       

    }
    //find object
    public int FindObjectPoolIndex(T obj)
    {
        
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (obj.gameObject.name == objectPool[i].prefab.gameObject.name)
            {
               // Debug.Log("type TYPE " + obj.gameObject.name + " _ " + objectPool[i].prefab.gameObject.name);
                return i;
            
                
            }

        }
        return -1;


    }
    //Get Object
    public T GetObject(int objectPoolIndex)
    {
      
        //  int chosenObjectPoolIndex = objectPool.IndexOf(obj);
        if (objectPool[objectPoolIndex].objectsQueue.Count <= 0)
        {
            AddObject(objectPoolIndex);
        }
        //else if (objectPool[chosenObjectPoolIndex].objectsQueue.Count > 0)
        //{

        //}
        if (objectPool[objectPoolIndex].objectsQueue.Count > 0)
        {
            var objectPooled = objectPool[objectPoolIndex].objectsQueue.Dequeue();
            objectPooled.gameObject.SetActive(true);
            objectPool[objectPoolIndex].activeObjectsList.Add(objectPooled);
            return (objectPooled);
        }
        return null;
        //if (objectPool[objectPoolIndex].objectsQueue.Count <= 0)
        //{
        //    AddObject(objectPoolIndex);
        //}
        //if (objectPool[objectPoolIndex].objectsQueue.Count > 0)
        //{
        //    var objectPooled = objectPool[objectPoolIndex].objectsQueue.Dequeue();
        //    objectPooled.gameObject.SetActive(true);

        //    objectPool[objectPoolIndex].activeObjectsList.Add(objectPooled);

        //    return (objectPooled);
        //}
        //return null;
    }

    public T GetObject(T obj)
    {

        int chosenObjectPoolIndex = FindObjectPoolIndex(obj);
      //  int chosenObjectPoolIndex = objectPool.IndexOf(obj);
        if (objectPool[chosenObjectPoolIndex].objectsQueue.Count <= 0)
        {
            AddObject(chosenObjectPoolIndex);
        }
        //else if (objectPool[chosenObjectPoolIndex].objectsQueue.Count > 0)
        //{

        //}
        if (objectPool[chosenObjectPoolIndex].objectsQueue.Count > 0)
        {
            var objectPooled = objectPool[chosenObjectPoolIndex].objectsQueue.Dequeue();
            objectPooled.gameObject.SetActive(true);
            objectPool[chosenObjectPoolIndex].activeObjectsList.Add(objectPooled);
            return (objectPooled);
        }
        return null;
    }

     //Add Object
    public void AddObject(int objectPoolIndex)
    {
      
        var addedObject = Instantiate(objectPool[objectPoolIndex].prefab);
        SceneManager.MoveGameObjectToScene(addedObject.gameObject, SceneManager.GetSceneByName(RoundManager.instance.objectPoolSceneName));
        addedObject.gameObject.SetActive(true);
        objectPool[objectPoolIndex].objectsQueue.Enqueue(addedObject);
        addedObject.gameObject.name = objectPool[objectPoolIndex].prefab.gameObject.name;


    }
   
    //Return to pool
    public void ReturnToPool(T obj)
    {
        int objectIndex = FindObjectPoolIndex(obj);
        
        objectPool[objectIndex].activeObjectsList.Remove(obj);
        obj.gameObject.SetActive(false);
        objectPool[objectIndex].objectsQueue.Enqueue(obj);
       


    }

    public void ReturnAllToPool()
    {
        for (int objectPoolIndex = 0; objectPoolIndex < objectPool.Count; objectPoolIndex++)
        {
            for (int objectInstanceIndex = 0; objectInstanceIndex < objectPool[objectPoolIndex].activeObjectsList.Count; )
            {
                
                objectPool[objectPoolIndex].activeObjectsList[0].gameObject.SetActive(false);
                objectPool[objectPoolIndex].objectsQueue.Enqueue(objectPool[objectPoolIndex].activeObjectsList[0]);
                objectPool[objectPoolIndex].activeObjectsList.RemoveAt(0);
                
            }
        }
    }
    public void ClearAllToPool()
    {
        Time.timeScale = 0;
        if (objectPool.Count > 0)
        {
            for (int objectPoolIndex = 0; objectPoolIndex < objectPool.Count; objectPoolIndex++)
            {
                if (objectPool[objectPoolIndex].objectsQueue.Count > 0)
                {
                    for (int objectInstanceIndex = 0; objectInstanceIndex < objectPool[objectPoolIndex].objectsQueue.Count;)
                    {
                        var targetObject = GetObject(objectPoolIndex);
                        targetObject.gameObject.SetActive(true);
                        objectPool[objectPoolIndex].activeObjectsList.Add(targetObject);
                       
                        //var targetObject = objectPool[objectPoolIndex].objectsQueue.Dequeue();
                        //objectPool[objectPoolIndex].activeObjectsList.Add(targetObject);
                    }
                }
                

                if (objectPool[objectPoolIndex].activeObjectsList.Count > 0)
                {
                    for (int objectInstanceIndex = 0; objectInstanceIndex < objectPool[objectPoolIndex].activeObjectsList.Count;)
                    {

                        var targetObject = objectPool[objectPoolIndex].activeObjectsList[0];
                        targetObject.gameObject.SetActive(false);
                        objectPool[objectPoolIndex].activeObjectsList.RemoveAt(0);
                        Destroy(targetObject);
                        //Destroy(targetObject.gameObject);
                       // objectInstanceIndex++;
                    }

                  
                }
             


            }
        }
        Time.timeScale = 1;

    }
  
    
}
