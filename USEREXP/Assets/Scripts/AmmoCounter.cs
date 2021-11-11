using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmmoCounter : MonoBehaviour
{
    public Text ammoText;
    public Image ammoImage;
    public Image notifImage;
    public Slider AmmoBar;
    public Coroutine reloadCoroutine;
    public float currentReloadTime;
    private void OnDisable()
    {
        notifImage.enabled = false;
        AmmoBar.gameObject.SetActive(false);
        StopAllCoroutines();
    }
    public void SetReloadTime(float reloadTime)
    {
        
        AmmoBar.maxValue = reloadTime;
        AmmoBar.value = reloadTime;
        currentReloadTime = reloadTime;
        AmmoBar.gameObject.SetActive(true);
        reloadCoroutine = StartCoroutine(reloadTimeBar(reloadTime));
        
    }



    public IEnumerator reloadTimeBar(float reloadTime)
    {
        //yield return new WaitForSeconds(reloadTime);
        while (currentReloadTime > 0)
        {
            currentReloadTime -= 0.1f;// * Time.deltaTime;
   
            AmmoBar.value = currentReloadTime;

            yield return new WaitForSeconds(0.1f);
            if (currentReloadTime <= 0)
            {
               // Debug.Log("FIIIIIIIIIIIIIIIIINNISH");
                AmmoBar.gameObject.SetActive(false);
                yield return null;
            }
        }
     
        //if (currentReloadTime <= 0)
        //{
        //    Debug.Log("FIIIIIIIIIIIIIIIIINNISH");
        //    AmmoBar.gameObject.SetActive(false);
        //}
        //else if (currentReloadTime > 0)
        //{
        //    Debug.Log("MSADMLASK;DMASL;DMAS;LDMASDMA");

        //    currentReloadTime -= 0.1f;
        //    AmmoBar.value = currentReloadTime;
        //    reloadCoroutine = StartCoroutine(reloadTimeBar());
        //}

    }
    public void SetAmmoImage(Sprite damageSourceSprite)
    {
        ammoImage.sprite = damageSourceSprite;
    }
    public void UpdateAmmo()
    {
        GameObject equippedWeapon = PlayerManager.instance.player.weaponSlots[PlayerManager.instance.player.equippedWeapon].gameObject;
        if (equippedWeapon.GetComponent<MeleeWeapon>() != null)
        {
            ammoText.text = "00";
        }
        else if (equippedWeapon.GetComponent<RangedWeapon>() != null)
        {
            int ammoAmount = equippedWeapon.GetComponent<RangedWeapon>().currentAmmoInMag;
            ammoText.text = ammoAmount.ToString();
        }
        //if (equippedWeapon.k//.IsSubclassOf(typeof(MeleeWeapon)))
        //{
        //    ammoText.text = "00";
        //}
        //else if (equippedWeapon.GetType().IsSubclassOf(typeof(RangedWeapon)))
        //{
        //    int ammoAmount = equippedWeapon.GetComponent<RangedWeapon>().currentAmmoInMag;
        //    ammoText.text = ammoAmount.ToString();
        //}
    }

    public void PlayNotif()
    {
        notifImage.enabled = true;
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().hudanim.SetTrigger("Reloading");
        
        //MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().hudanim.Play("ReloadingUI");//.SetTrigger("Reloading");
    }

    public void StopNotif()
    {
        notifImage.enabled = false;
       // MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().hudanim.Stop("ReloadingUI");
    }
}
