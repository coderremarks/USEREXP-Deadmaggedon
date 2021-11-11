using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image bloodOverlay;
    [SerializeField] private Color _alphaColorBlood;
    public void SetHealth(float health)
    {
        slider.value = health;
        
        _alphaColorBlood.a = 1f - (slider.value / slider.maxValue);
        bloodOverlay.color = _alphaColorBlood;
    }


    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        _alphaColorBlood = bloodOverlay.color;
        _alphaColorBlood.a = 1f - (slider.value / slider.maxValue);
        
        bloodOverlay.color = _alphaColorBlood;
    }
 
}
