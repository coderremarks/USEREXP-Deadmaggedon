using UnityEngine;

public enum CutsceneType
{
    press,
    movie,
}
public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayCutsceneTutorial(TutorialPurpose chosenTutorial, float duration = 0, Vector3 cutsceneCameraTransform = default(Vector3), float cutsceneCameraZoom = 0)
    {
 
        //if (cutsceneTheatrics == null)
        //{
        //    MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(chosenTutorial);
        //}
        if (cutsceneCameraTransform != null && cutsceneCameraTransform != default(Vector3))
        {
         
            StartCoroutine(PlayerManager.instance.playerCamera.MovieCamera(cutsceneCameraTransform, cutsceneCameraZoom, duration,chosenTutorial));

        }
      
        //if (cutsceneTheatrics != null)
        //{
        //    cutsceneTheatrics.SetTrigger(theatricsTrigger);//Play();
        //}



    }
 


  



}
