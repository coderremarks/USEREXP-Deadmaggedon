using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;
    public GameObject chatBubble;
    public GameObject coin;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
    }
}
