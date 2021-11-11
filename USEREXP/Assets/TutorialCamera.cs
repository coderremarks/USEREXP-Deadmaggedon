using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    public static TutorialCamera instance;
    public GameObject focusObject;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
    }

    private void Update()
    {
        if (focusObject != null)
        {
            PlayerManager.instance.playerCamera.CameraFollow(focusObject.transform.position);
        }
       
    }
}
