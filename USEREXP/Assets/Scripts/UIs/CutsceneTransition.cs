using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTransition : MonoBehaviour
{
    public Animator transition;
    public void In()
    {
        transition.SetTrigger("FadeIn");
    }

    public void Out()
    {
        transition.SetTrigger("FadeOut");
    }
}
