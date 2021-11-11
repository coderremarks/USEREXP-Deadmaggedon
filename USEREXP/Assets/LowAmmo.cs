using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LowAmmo : MonoBehaviour
{
    public TextMeshProUGUI lowAmmoText;
    private void OnDisable()
    {
        lowAmmoText.color = new Color32(255, 255, 255, 255);
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
    public void OnEnable()
    {
        StartCoroutine(Blinking());
    }
    public void Update()
    {
        
        Vector2 targetPositionScreenPoint = Input.mousePosition;//PlayerManager.instance.playerCamera.camera.WorldToScreenPoint();
        if (MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().lowAmmo.gameObject.activeSelf == true)
        {
            MenuManager.instance.GetCanvas(MenuType.HUD).GetComponent<HUDCanvas>().lowAmmo.gameObject.GetComponent<RectTransform>().position = targetPositionScreenPoint + new Vector2(0, 30);
        }
    }

    public IEnumerator Blinking()
    {
       

        yield return new WaitForSeconds(0.35f);

        lowAmmoText.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.35f);
        lowAmmoText.color = new Color32(255, 255, 255, 255);
        StartCoroutine(Blinking());

    }
}
