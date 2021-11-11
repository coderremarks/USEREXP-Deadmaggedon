using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GenericPersistentSingleton<T> : MonoBehaviour where T : Component
{
    //private static T _instance;
    //public static T instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            //nstance = new T();
    //        }
    //        return _instance;
    //    }
        
    //}

    //private void OnDestroy()
    //{
    //    if (_instance == this)
    //    {
    //        _instance = null;
    //    }
    //}

    //public virtual void Awake()
    //{
    //    if (_instance == null)
    //    {
    //        _instance = this as T;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(this);
    //    }
    //}
}
