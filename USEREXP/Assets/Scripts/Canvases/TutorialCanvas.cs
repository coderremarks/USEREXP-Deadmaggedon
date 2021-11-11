using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
[System.Serializable]
public class TutorialText
{
    public TutorialPurpose tutorialPurpose;
    public GameObject textTutorial;
    public string animationTriggerText;

}
public class TutorialCanvas : MenuCanvas
{
    [SerializeField] public List<TutorialText> tutorialText;
    public GoNotification goNotification;
    public Animator anim;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    


    public void HideAll()
    {
        for (int i = 0; i < tutorialText.Count; i++)
        {
            if (tutorialText[i].textTutorial != null)
            {
                tutorialText[i].textTutorial.SetActive(false);
            }
            
            
          
        }
    }

    //public TutorialText Get(TutorialPurpose chosenTutorial)
    //{
    //    HideAll();
    //    for (int i = 0; i < tutorialText.Count; i++)
    //    {

    //        if (tutorialText[i].tutorialPurpose == chosenTutorial)
    //        {
    //            return tutorialText[i];
    //        }
    //    }
    //    return null;
    //}
    public void Choose(TutorialPurpose chosenTutorial)
    {
        HideAll();
        for (int i = 0; i < tutorialText.Count; i++)
        {
     
            if (tutorialText[i].tutorialPurpose == chosenTutorial)
            {
                tutorialText[i].textTutorial.SetActive(true);
                if (tutorialText[i].animationTriggerText != "")
                {
                    this.gameObject.GetComponent<Animator>().SetTrigger(tutorialText[i].animationTriggerText);
                }
   
            }
        }
    }

   
}
