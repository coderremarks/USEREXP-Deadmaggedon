using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BuildingMaterials : MonoBehaviour
{
    public TextMeshProUGUI buildingMatsText;

    public void UpdateCounter(int p_buildingMats)
    {
        buildingMatsText.text = p_buildingMats.ToString();
    }

    private void OnDisable()
    {
        buildingMatsText.color = new Color32(255, 255, 255, 255);
        StopAllCoroutines();
    }
        
    public void Insufficient()
    {
        StartCoroutine(Blinking());
    }
    public IEnumerator Blinking()
    {
        float counter = 3;
      
        while (counter > 0)
        {
          
            yield return new WaitForSeconds(0.15f);
            
            buildingMatsText.color = new Color32(255, 0, 0, 255);
            yield return new WaitForSeconds(0.15f);
            buildingMatsText.color = new Color32(255, 255, 255, 255);
            counter--;

        }
      
    }
}
