using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class LightManager : MonoBehaviour
{
    public static LightManager instance;
    public Light2D globalLight;//lightRenderer;
    public float intensity;
    public float intensityRate;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
