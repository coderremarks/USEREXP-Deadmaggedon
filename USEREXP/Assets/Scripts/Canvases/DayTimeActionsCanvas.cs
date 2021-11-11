using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class DayTimeActionsCanvas : MenuCanvas
{

    public List<Action> rewardsList = new List<Action>();
    public int chosenRewardIndex = 0;
    [SerializeField] private Button selectButton;
    public void OnEnable()
    {
        selectButton.interactable = true;

    }
    public override void Initialize()
    {
       // Debug.Log("DAY TIME INITIALIZED");
        rewardsList.Add(BuildingMaterialsReward);
        rewardsList.Add(WeaponReward);
        rewardsList.Add(SurvivorReward);
        rewardsList.Add(FortifyReward);
     //   Debug.Log("DAY TIME INITIALIZED END");
    }

    public void ChooseReward()
    {
        rewardsList[chosenRewardIndex]?.Invoke();
        selectButton.interactable = false;
    }

    public void UpdateChosenRewardIndex(int index)
    {
        chosenRewardIndex = index ;
    }
    public void BuildingMaterialsReward()
    {
        int chosenIndex = UnityEngine.Random.Range(0,RoundManager.instance.buildingMaterialsAmount.Count);
        PlayerManager.instance.BuildingMaterials += RoundManager.instance.buildingMaterialsAmount[chosenIndex];
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeOut();
        RoundManager.instance.StartNight();
    }

    public void WeaponReward()
    {
        int chosenIndex = UnityEngine.Random.Range(0, ObjectManager.instance.weaponList.Count);
        Weapon newWeapon = Instantiate(ObjectManager.instance.weaponList[chosenIndex]).GetComponent<Weapon>();
        newWeapon.gameObject.SetActive(false);
        Destroy(PlayerManager.instance.currentlyEquippedWeapon);
        PlayerManager.instance.currentlyEquippedWeapon = newWeapon.gameObject;
        //SceneManager.MoveGameObjectToScene(newWeapon.gameObject, SceneManager.GetSceneByName(RoundManager.instance.objectPoolSceneName));
        /*MOVE THIS TO ON START OF EVERY MATCH
        PlayerManager.instance.currentlyEquippedWeapon = newWeapon;
        newWeapon.transform.position = PlayerManager.instance.player.gripPoint.position;

        PlayerManager.instance.player.weaponSlots[PlayerManager.instance.player.equippedWeapon].Equip(PlayerManager.instance.player);*/
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeOut();
        
        RoundManager.instance.StartNight();
    }

    public void SurvivorReward()
    {
        int rollResult = UnityEngine.Random.Range(0, 100);
        if (rollResult >= 80)// SURVIVOR RATE
        {
            Debug.Log("Ally");
          //  AllyPool.instance.GetObject();
        }
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeOut();
        RoundManager.instance.StartDay();
    }
    public void FortifyReward()
    {
        PlayerManager.instance.player.GetComponent<CharacterController>().enabled = false;
        BuilderController.instance.enabled = true;
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().FadeOut();
        RoundManager.instance.StartDay();
    }

   
}
