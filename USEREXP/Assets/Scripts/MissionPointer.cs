using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MissionPointer : MonoBehaviour
{
 
    [SerializeField] private Image _missionPointerImage;
    [SerializeField] private RectTransform _missionDistanceTransform;
    [SerializeField] private RectTransform _missionPointerTransform;
    [SerializeField] private RectTransform _missionHealthTransform;
    [SerializeField] private Text _missionHealthText;
    [SerializeField] private Sprite _arrowSprite;
    [SerializeField] private Sprite _missionIconSprite;
    public bool isWarning;
    [SerializeField] private Sprite _warningIconSprite;
    [SerializeField] private Vector2 _targetPosition;


    public float blinkAmount;
    public float invincibilityBlinkTime;
    public float invincibilityUIRestTime;
    public void Initialize()
    {
        isWarning = false;
        blinkAmount = 3;
        invincibilityBlinkTime = 0.15f;
        invincibilityUIRestTime = 0.3f;
    //_targetPosition = RoundManager.instance.goalObject.transform.position;//new Vector2(200f, 45f);
    //Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _missionHealthTransform.gameObject.SetActive(false);
    }

    public void Show(Vector3 targetPosition)
    {
        gameObject.SetActive(true);
        _missionHealthTransform.gameObject.SetActive(true);
        this._targetPosition = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {


        float borderSize = 25f;
        Vector2 targetPositionScreenPoint = PlayerManager.instance.playerCamera.camera.WorldToScreenPoint(_targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

        if (isOffScreen) //outside screen
        {
            if (isWarning == false)
            {
                RotatePointer();
                _missionPointerImage.sprite = _arrowSprite;
                //_missionPointerImage.color = Color.green;
                _missionPointerImage.enabled = true;

                _missionHealthTransform.gameObject.SetActive(false);
                _missionDistanceTransform.gameObject.SetActive(true);
            }
            else
            {
                _missionPointerTransform.localEulerAngles = Vector3.zero;
                _missionDistanceTransform.localEulerAngles = Vector3.zero;
   
            }
 

            Vector2 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= borderSize)
            {
                cappedTargetScreenPosition.x = borderSize;
            }
            if (cappedTargetScreenPosition.x >= Screen.width - borderSize)
            {
                cappedTargetScreenPosition.x = Screen.width - borderSize;
            }
            if (cappedTargetScreenPosition.y <= borderSize)
            {
                cappedTargetScreenPosition.y = borderSize;
            }
            if (cappedTargetScreenPosition.y >= Screen.height - borderSize)
            {
                cappedTargetScreenPosition.y = Screen.height - borderSize;
            }
            Vector2 pointerWorldPosition = (cappedTargetScreenPosition);
            _missionPointerTransform.position = pointerWorldPosition;
       
         

           // _missionPointerDistance.GetComponent<RectTransform>().position = pointerWorldPosition - new Vector2(0, 40);


        }
        else
        {
            if (isWarning ==false)
            {
                //_missionPointerImage.sprite = _missionIconSprite;
                //_missionPointerImage.color = Color.green;
     
            }
        
            _missionPointerImage.enabled = false;
            _missionHealthTransform.gameObject.SetActive(true);
            _missionDistanceTransform.gameObject.SetActive(false);


            Vector2 pointerWorldPosition = (targetPositionScreenPoint);
            //_missionPointerTransform.position = pointerWorldPosition;
            //_missionPointerTransform.localEulerAngles = Vector3.zero;
            //_missionDistanceTransform.localEulerAngles = Vector3.zero;
            _missionHealthTransform.position = pointerWorldPosition + new Vector2(0, 200f);
            //_missionHealthText.gameObject.GetComponent<RectTransform>().position = pointerWorldPosition + new Vector2(0,5);
            _missionHealthTransform.localEulerAngles = Vector3.zero;

            // _missionPointerDistance.GetComponent<RectTransform>().position = pointerWorldPosition - new Vector2(0, 40);
            // _missionPointerDistance.GetComponent<RectTransform>().position = pointerWorldPosition + new Vector2(60, 0);

        }
        _missionDistanceTransform.GetComponent<Text>().text = Mathf.RoundToInt(Vector2.Distance(PlayerManager.instance.playerCamera.camera.transform.position, _targetPosition)).ToString() + "m";


    }

    private void RotatePointer()
    {
        Vector2 originPosition = PlayerManager.instance.playerCamera.camera.transform.position;
        Vector2 dir = (_targetPosition - originPosition).normalized;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        _missionPointerTransform.localEulerAngles = new Vector3(0f, 0f, angle);
        _missionDistanceTransform.localEulerAngles = new Vector3(0f, 0f, -angle);

    }

    public void Warn(int health)
    {
        isWarning = true;
        _missionHealthText.text = health.ToString();
        _missionPointerImage.sprite = _warningIconSprite;
       // _missionPointerImage.color = Color.white;
        _missionPointerTransform.sizeDelta.Set(80,80);
        
        StartCoroutine(Warning());
    }

    IEnumerator Warning()
    {
    
        
        
        for (int currentBlinkAmount = 0; currentBlinkAmount < blinkAmount; currentBlinkAmount++)
        {

         
            _missionPointerImage.enabled = false;
            if (_missionHealthTransform.gameObject.activeSelf == false)
            {
                _missionHealthText.gameObject.SetActive(false);
            }
            
            yield return new WaitForSeconds(invincibilityBlinkTime);
            _missionPointerImage.enabled = true;
            if (_missionHealthTransform.gameObject.activeSelf == false)
            {
                _missionHealthText.gameObject.SetActive(true);
            }
            
            //  chosenDirection.arrowsUI.SetActive(false);


            yield return new WaitForSeconds(invincibilityUIRestTime);
        }
        _missionPointerImage.enabled = true;
        _missionHealthText.gameObject.SetActive(false);
        _missionPointerTransform.sizeDelta.Set(60, 60);
        yield return new WaitForSeconds(1);
        isWarning = false;
    }
}
