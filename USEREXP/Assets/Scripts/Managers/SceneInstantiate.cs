using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class SceneInstantiate: MonoBehaviour
{

    public static SceneInstantiate instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
       
   
    }

  
    public IEnumerator AsyncLoadScene(string name, Action onCallBack = null)
    {
        AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        while (!asyncLoadScene.isDone)
        {
            // loading bar =  asyncLoadScene.progress

            yield return null;
        }
        if (onCallBack != null)
            onCallBack?.Invoke();
    }

    public IEnumerator AsyncUnloadScene(string name, Action onCallBack = null)
    {
        AsyncOperation unasych = SceneManager.UnloadSceneAsync(name);

        while (!unasych.isDone)
        {

            yield return null;
        }
        if (onCallBack != null)
            onCallBack?.Invoke();
    }
}
