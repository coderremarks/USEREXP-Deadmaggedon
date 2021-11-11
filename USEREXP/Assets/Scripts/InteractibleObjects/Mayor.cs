using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mayor : InteractibleObject
{
    public Animator anim;
    [SerializeField] protected int blinkAmount;
    [SerializeField] protected bool invincible;
    [SerializeField] protected float invincibilityTime;
    [SerializeField] protected float invincibilityBlinkTime;
    [SerializeField] protected float invincibilityUIRestTime;
    public override void OnEnable()
    {
        base.OnEnable();

        anim.SetBool("isAlive", true);
    }

 
    public override void DamageHealth(float damage, string originName)
    {
        if (invincible == false)
        {
            base.DamageHealth(damage);
            //HELP MAYOR GOT DAMAGED UI
            PlayerManager.instance.playerCamera.CameraShake(0.5f, 0.25f, 10f);
            MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionHealth?.UpdateHealth(Mathf.CeilToInt(currentHealth));
            MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionPointer.enabled = true;
            MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionPointer?.Warn(Mathf.CeilToInt(currentHealth));
            StartCoroutine(InvincibilityTime());
        }

    }

    IEnumerator InvincibilityTime()
    {
        invincible = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.075f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(0.03f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;


        for (int currentBlinkAmount = 0; currentBlinkAmount < blinkAmount; currentBlinkAmount++)
        {

            gameObject.GetComponent<SpriteRenderer>().enabled = true;


            yield return new WaitForSeconds(invincibilityBlinkTime);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //  chosenDirection.arrowsUI.SetActive(false);


            yield return new WaitForSeconds(invincibilityUIRestTime);
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(invincibilityTime);

        invincible = false;

    }
    public override void Death()
    {
        anim.SetBool("isAlive", false);
        MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionHealth?.UpdateHealth(Mathf.CeilToInt(currentHealth));
        //MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().missionPointer?.Warn(Mathf.CeilToInt(currentHealth));
        var bloodPuddle = ParticlePool.instance.GetObject(0);
        bloodPuddle.gameObject.transform.position = this.transform.position;
        bloodPuddle.gameObject.transform.localScale = this.transform.localScale;
        MenuManager.instance.GetCanvas(MenuType.GameOver).GetComponent<GameOverCanvas>().CauseOfDeathText.text = "Mayor Died";
        GameManager.instance.GameOver();
        Destroy(this.gameObject);
    }


}
