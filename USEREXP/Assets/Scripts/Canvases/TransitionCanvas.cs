using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCanvas : MenuCanvas
{
    public Animator transition;
    public Animator cutscene;
    public GameObject dialogue;
    protected override void Start()
    {
        base.Start();
    }

    public void FadeIn()
    {
        transition.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        transition.SetTrigger("FadeOut");
    }
}
