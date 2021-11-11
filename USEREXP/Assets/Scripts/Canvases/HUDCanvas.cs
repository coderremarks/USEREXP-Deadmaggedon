using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDCanvas : MenuCanvas
{
    [SerializeField] private Button _pauseBut;
    public LowAmmo lowAmmo;
    public AmmoCounter ammoCounter;
    public HealthBar healthBar;
    public WaveTimerBar waveTimerBar;
    public HealthSprites missionHealth;
    public MissionPointer missionPointer;
    public Animator hudanim;
    public Notification notification;
    public BuildingMaterials buildMaterials;
    // public Animation hudanim;

    private void OnEnable()
    {
        buildMaterials.UpdateCounter(PlayerManager.instance.BuildingMaterials);
    }

    public override void Initialize()
    {
        ammoCounter.StopNotif();
        waveTimerBar.setMaxValue(RoundManager.instance.nightTime);
           
    }

    protected override void Start()
    {
        base.Start();
        
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().ammoCounter.StopNotif();
        //  hudanim = GetComponent<Animation>();
        //Debug.Log("HUD ANIM SET ASKDMALKDMAK");
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

}
