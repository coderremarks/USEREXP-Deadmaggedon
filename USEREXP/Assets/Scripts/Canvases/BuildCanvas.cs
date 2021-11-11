using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class BuildCanvas : MenuCanvas
{
    public BlueprintScriptableObject lastSelectedStructure;
    public TextMeshProUGUI cancelReminderText;
    public BuildingMaterials buildingMaterials;
    public GameObject endDayConfirmation;
    public WaveTimerBar waveTimerBar;
   public TextMeshProUGUI SpikesCost;
    public TextMeshProUGUI BarrelCost;
    public TextMeshProUGUI SurvivorCost;
    public BlueprintScriptableObject spikes;
    public BlueprintScriptableObject barrel;
    public BlueprintScriptableObject survivor;
    public GameObject barrelSelection;
    public GameObject survivorSelection;
    public List<Button> buttonList = new List<Button>();
    public List<BlueprintScriptableObject> buttonTemplates = new List<BlueprintScriptableObject>();
    public void OnEnable()
    {
        buildingMaterials.UpdateCounter(PlayerManager.instance.BuildingMaterials);
        SpikesCost.text = spikes.cost.ToString();
        BarrelCost.text = barrel.cost.ToString();
        SurvivorCost.text = survivor.cost.ToString(); 
        barrelSelection.SetActive(true);
        survivorSelection.SetActive(true);
        cancelReminderText.gameObject.SetActive(false);
    }
    public void PauseBut()
    {
        if (MenuManager.instance.GetCanvas(MenuType.Pause).gameObject.activeSelf == false)
        {
            MenuManager.instance.ShowCanvas(MenuType.Pause);
        }
        else if (MenuManager.instance.GetCanvas(MenuType.Pause).gameObject.activeSelf == true)
        {
            RoundManager.instance.isPlaying = true;
            MenuManager.instance.GetCanvas(MenuType.Pause).GetComponent<PauseCanvas>().Hide();
        }
    }
    public void Confirm()
    {
        endDayConfirmation.SetActive(false);
    
        if (GameManager.instance.firstPlay == false)
        {
            RoundManager.instance.EndDay();
        }
        else if (GameManager.instance.firstPlay == true)
        {
            TutorialManager.instance.StartWave();
     



        }
    }
    public void ResetBldg()
    {
        Time.timeScale = 1f;
        BuilderController.instance.chosenStructureTemplate.gameObject.SetActive(false);
        BuilderController.instance.chosenStructureBlueprint = null;
        MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().lastSelectedStructure = null;
        RoundManager.instance.StopAllCoroutines();
    }
    public void Cancel()
    {
        endDayConfirmation.SetActive(false);
        Time.timeScale = 1f;
       
    }
    public void ChooseStructure(BlueprintScriptableObject p_structure)
    {


        if (PlayerManager.instance.BuildingMaterials >= p_structure.cost)
        {
            if (lastSelectedStructure != p_structure)//&& PlayerManager.instance.BuildingMaterials >= cost
            {
                //PlayerManager.instance.player.GetComponent<BuilderController>().chosenStructure = p_structure;
                BuilderController.instance.ChangeChosenStructure(p_structure);
                lastSelectedStructure = p_structure;
                cancelReminderText.gameObject.SetActive(true);
            }
            else if (lastSelectedStructure == p_structure)
            {
                BuilderController.instance.CancelChosenStructure();

                //commented because these are in builder controller
                //lastSelectedStructure = null;
                //cancelReminderText.gameObject.SetActive(false);
            }
           
        }
        else if (PlayerManager.instance.BuildingMaterials < p_structure.cost)
        {
            buildingMaterials.Insufficient();
        }
        //for (int i = 0; i < buttonList.Count; i++)
        //{
        //    for (int blueprinti = 0; blueprinti < buttonList.Count; blueprinti++)
        //    {
        //        if (buttonList[i].name == buttonTemplates[blueprinti].name)
        //        {
        //            if (PlayerManager.instance.BuildingMaterials < buttonTemplates[blueprinti].cost)
        //            {

        //            }
        //        }
        //    }
        //}



    }

    public void EndDay()
    {

        endDayConfirmation.SetActive(true);
        Time.timeScale = 0f;
        //BuilderController.instance.chosenStructureTemplate.gameObject.SetActive(false);
        //BuilderController.instance.chosenStructureBlueprint = null;
        //MenuManager.instance.GetCanvas(MenuType.Build).GetComponent<BuildCanvas>().lastSelectedStructure = null;
        //if (GameManager.instance.firstPlay == false)
        //{
        //    RoundManager.instance.EndDay();
        //}
        //else if (GameManager.instance.firstPlay == true)
        //{
        //    TutorialManager.instance.StartWave();
            
            
        //}
    }
}
