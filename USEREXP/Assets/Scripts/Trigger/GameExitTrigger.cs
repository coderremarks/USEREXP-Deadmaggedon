using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitTrigger : MonoBehaviour
{
    public bool activated = false;
    public TutorialPurpose triggerTutorialPurpose;
    public Transform cutsceneCameraTransform = null;
    public GameObject currentSpeaker = null;
    public float cutsceneCameraZoom = 0;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D hit)
    {
        if (hit.gameObject.CompareTag("Player") && activated == false)
        {
            if (this.gameObject.transform.position.x < hit.gameObject.transform.position.x)
            {
                activated = true;
                MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().HideAll();
                Vector3 tempVal;
                if (cutsceneCameraTransform != null)
                {
                    tempVal = cutsceneCameraTransform.position;
                }
                else
                {
                    tempVal = Vector3.zero;
                }
                CutsceneManager.instance.PlayCutsceneTutorial(triggerTutorialPurpose, duration, tempVal, cutsceneCameraZoom);//, cutsceneTheatrics, theatricsTrigger);
                if (currentSpeaker != null)
                {
                    DialogueManager.instance.ActivateConversation(currentSpeaker, triggerTutorialPurpose);
                }
              
                TutorialManager.instance.StartInitialFunction(triggerTutorialPurpose);
                //TutorialManager.instance.StartInitialFunction(triggerTutorialPurpose);
                //MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().StartInitialFunction(triggerTutorialType);

            }
        }
       
    }

}
