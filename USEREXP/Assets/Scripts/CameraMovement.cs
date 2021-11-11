using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CameraMovement : MonoBehaviour
{
    public Camera camera;
    //Camera


    [SerializeField] public Vector3 defaultPos;
    public float defaultZoom;
    public bool isShaking, isBouncing = false;

    Coroutine runningCoroutine;
    public void Awake()
    {
        if (camera == null)
        {
            camera = this.gameObject.GetComponent<Camera>();
        }
        defaultZoom = camera.orthographicSize;
        //defaulPos
    }
    public void CameraFollow(Vector3 targetObjectPosition)
    {
        this.transform.position = targetObjectPosition + new Vector3(0f, 0f, -10);

    }

    private void LateUpdate()
    {

        //if (shakeTimeRemaining > 0 && isShaking == true)
        //{

        //    shakeTimeRemaining -= Time.deltaTime;
        //    float xAmount = Random.Range(-1f, 1f) * shakePower;
        //    float yAmount = Random.Range(-1f, 1f) * shakePower;

        //    PlayerManager.instance.playerCamera.gameObject.transform.position += new Vector3(xAmount, yAmount, 0f);
        //    shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        //    shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
        //    PlayerManager.instance.playerCamera.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f, 1f));
        //}



    }


    public void CameraBounceIn(float targetSize, float bounceAmount, float durationTime)
    {
       // Debug.Log("CALLING BOUNCE");
        StartCoroutine(BounceIn(targetSize, bounceAmount, durationTime));
    }

    IEnumerator ZoomIn(float targetSize, float durationTime)
    {
        float currentZoom = camera.orthographicSize;
        float zoomAmount = (camera.orthographicSize - targetSize);

        float zoomRate = -1f * (zoomAmount / durationTime * 0.1f);


        for (float i = Mathf.Abs(zoomAmount); i >= Mathf.Abs(zoomRate); i -= Mathf.Abs(zoomRate))
        {

            currentZoom += zoomRate;
            camera.orthographicSize = currentZoom;
            yield return new WaitForSeconds(Mathf.Abs(zoomRate));


        }

    }

    IEnumerator ZoomOut(float targetSize, float durationTime)
    {
        float currentZoom = camera.orthographicSize;
        //Debug.Log("current zoom: " + currentZoom);
        //Debug.Log("target size: " + targetSize);
        float zoomAmount = (camera.orthographicSize - targetSize);
        float zoomRate = -1f * (zoomAmount / durationTime * 0.1f);
        //Debug.Log("zoom rate: " + zoomRate);
        //Debug.Log("ZOOM AMOUNT: " + zoomAmount);
        for (float i = Mathf.Abs(zoomAmount); i >= Mathf.Abs(zoomRate); i -= Mathf.Abs(zoomRate))
        {
            //Debug.Log("aMOUNT OF ZOOM IN LEFT: " + zoomAmount);

            currentZoom += zoomRate;
            camera.orthographicSize = currentZoom;
            yield return new WaitForSeconds(Mathf.Abs(zoomRate));


        }

    }

    IEnumerator BounceIn(float targetSize, float bounceAmount, float durationTime)
    {
        float originSize = camera.orthographicSize;
        float bounceRate = (durationTime / bounceAmount * 0.1f) / 2f;
        //Debug.Log("OG: " + bounceRate*2f);
        //Debug.Log("EDITED: " + bounceRate);
        //Debug.Log("origin Size: " + originSize);
        float counter = durationTime;
        while (counter >= bounceRate && isBouncing == false)
        {

            isBouncing = true;
            //Debug.Log("COUNTER: " + counter);
            //Debug.Log(isBouncing);
            StartCoroutine(ZoomIn(targetSize, bounceRate / 0.1f));
            yield return new WaitForSeconds(bounceRate / 0.1f);
            camera.orthographicSize = Mathf.FloorToInt(camera.orthographicSize);
            StartCoroutine(ZoomOut(originSize, bounceRate / 0.1f));
            yield return new WaitForSeconds(bounceRate / 0.1f);
            camera.orthographicSize = Mathf.CeilToInt(camera.orthographicSize);
            counter -= bounceRate / 0.1f;
            isBouncing = false;

        }
      //  Debug.Log("BOUCNE ENDED");

    }

    void VerticalShake(float shakePower, float shakeFadeTime)
    {
        float yAmount = UnityEngine.Random.Range(-1f, 1f) * shakePower;

        PlayerManager.instance.playerCamera.gameObject.transform.position += new Vector3(0f, yAmount, 0f);
  

    }

    void HorizontalShake(float shakePower, float shakeFadeTime)
    {
        float xAmount = UnityEngine.Random.Range(-1f, 1f) * shakePower;
        PlayerManager.instance.playerCamera.gameObject.transform.position += new Vector3(xAmount, 0f, 0f);
      
    }

    void RotationShake(float shakeRotation, float shakeFadeTime, float rotationMultiplier)
    {
        shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
        PlayerManager.instance.playerCamera.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * UnityEngine.Random.Range(-1f, 1f));
    }

    IEnumerator Shaking(float shakePower, float shakeRotation, float rotationMultiplier, float shakeFadeTime, float duration)
    {

        isShaking = true;
        VerticalShake(shakePower, shakeFadeTime);
        HorizontalShake(shakePower, shakeFadeTime);
       
        RotationShake(shakePower, shakeFadeTime, rotationMultiplier);
        shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        yield return new WaitForSeconds(duration);
        runningCoroutine = StartCoroutine(Shaking(shakePower, shakeRotation, rotationMultiplier, shakeFadeTime, duration));

    }

    IEnumerator Recoil(float shakePower, float shakeFadeTime, float duration)
    {
        isShaking = true;
        VerticalShake(shakePower, shakeFadeTime);
        shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        yield return new WaitForSeconds(duration);
        runningCoroutine = StartCoroutine(Recoil(shakePower, shakeFadeTime, duration));
    }
    public void CameraShake(float length, float power, float rotationMultiplier)
    {
        defaultPos = camera.transform.position;
        StartShake(length, power, rotationMultiplier);
    }

    public void CameraRecoil(float length, float power, float rotationMultiplier)
    {
        defaultPos = camera.transform.position;
        StartRecoilShake(length, power, rotationMultiplier);
    }

    public void StartShake(float length, float power, float rotationMultiplier)
    {

         runningCoroutine = StartCoroutine(Shaking(power, power * rotationMultiplier,rotationMultiplier, power / length,0.01f));

        Invoke("StopShake", length);
    }
    public void StartRecoilShake(float length, float power, float rotationMultiplier)
    {

        runningCoroutine = StartCoroutine(Recoil(power, power / length, 0.01f));

        Invoke("StopShake", length);
    }
    public void StopShake()
    {
        StopCoroutine(runningCoroutine);
        ResetCamera();
    }
    public void ResetCamera()
    {
        isShaking = false;
        PlayerManager.instance.playerCamera.gameObject.transform.position = defaultPos;

    }

    public void StartCameraPress(Transform focusTransform, float zoom = 0f)
    {
        StopAllCoroutines();
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().cutscene.SetTrigger("FadeIn");
        PlayerManager.instance.playerCamera.gameObject.transform.position = focusTransform.position;
        if (zoom != 0f)
        {

            PlayerManager.instance.playerCamera.camera.orthographicSize = zoom;
        }

    }

    public void EndCameraPress(Transform focusTransform)
    {
        PlayerManager.instance.playerCamera.gameObject.transform.position = defaultPos;
        PlayerManager.instance.playerCamera.camera.orthographicSize = defaultZoom;
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().cutscene.SetTrigger("FadeOut");
    }
    public IEnumerator MovieCamera(float duration)
    {
        StopAllCoroutines();
        MenuManager.instance.HideAll();
        Cursor.visible = false;
        PlayerManager.instance.player.canMove = false;
       // PlayerManager.instance.playerCamera.enabled = false;
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().cutscene.SetTrigger("FadeIn");
  
        yield return new WaitForSecondsRealtime(duration);
        PlayerManager.instance.player.canMove = true;
        PlayerManager.instance.playerCamera.gameObject.transform.position = defaultPos;
        PlayerManager.instance.playerCamera.camera.orthographicSize = defaultZoom;
        //PlayerManager.instance.playerCamera.enabled = true;
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().cutscene.SetTrigger("FadeOut");
        Cursor.visible = true;
        MenuManager.instance.ShowCanvas(MenuType.HUD);
    }

    public IEnumerator MovieCamera(Vector3 focusTransform, float zoom, float duration = 0, TutorialPurpose chosenTutorialPurpose = TutorialPurpose.none)
    {
        StopAllCoroutines();
        MenuManager.instance.HideAll();
        Cursor.visible = false;
        PlayerManager.instance.player.canMove = false;
        // PlayerManager.instance.playerCamera.enabled = false;
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().cutscene.SetTrigger("FadeIn");
        PlayerManager.instance.playerCamera.gameObject.transform.position = focusTransform;
    

        PlayerManager.instance.playerCamera.camera.orthographicSize = zoom;
        

        yield return new WaitForSecondsRealtime(duration);
        if (duration > 0)
        {
            PlayerManager.instance.player.canMove = true;
            PlayerManager.instance.playerCamera.gameObject.transform.position = defaultPos;
            PlayerManager.instance.playerCamera.camera.orthographicSize = defaultZoom;
            //PlayerManager.instance.playerCamera.enabled = true;
            MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().cutscene.SetTrigger("FadeOut");
            Cursor.visible = true;
            MenuManager.instance.ShowCanvas(MenuType.HUD);
            if (chosenTutorialPurpose != TutorialPurpose.none)
            {
                MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(chosenTutorialPurpose);
            }
            if (chosenTutorialPurpose == TutorialPurpose.movement)
            {
                MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().goNotification.StartBlink();
            }
        }
        
        

    }
}
