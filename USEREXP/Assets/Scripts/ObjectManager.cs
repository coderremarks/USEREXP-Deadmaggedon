using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;
    
    public List<GameObject> weaponList = new List<GameObject>();
    public List<GameObject> structureList = new List<GameObject>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
