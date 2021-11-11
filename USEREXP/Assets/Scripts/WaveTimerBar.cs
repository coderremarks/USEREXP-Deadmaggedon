using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WaveTimerBar : MonoBehaviour
{
    public TextMeshProUGUI waveCounter;
    public Slider timeSlider;
   
    public void UpdateWaveCounter(int wave)
    {
        waveCounter.text = wave.ToString();
    }
    public void timeCount(float roundTime)
    {
        timeSlider.value = roundTime;

    }

    public void setMaxValue(float maxRoundTime)
    {
        timeSlider.maxValue = maxRoundTime;
        timeSlider.value = maxRoundTime;
    }
}
