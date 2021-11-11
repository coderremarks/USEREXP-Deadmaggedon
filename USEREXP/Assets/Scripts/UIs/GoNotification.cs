using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoNotification : MonoBehaviour
{
    public GameObject pointer;
    public GameObject text;
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void StartBlink()
    {
        StartCoroutine(Blinking());
    }
    // Start is called before the first frame update
    IEnumerator Blinking()
    {
        for (int currentBlinkAmount = 0; currentBlinkAmount < 3; currentBlinkAmount++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            pointer.SetActive(true);
            text.SetActive(true);
            yield return new WaitForSecondsRealtime(0.35f);
            pointer.SetActive(false);
            text.SetActive(false);
        }
        yield return new WaitForSeconds(1f);
    }
}
