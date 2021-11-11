using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Structure : InteractibleObject
{
    [SerializeField] public Material _whiteMat;
    [SerializeField] public Material defaultMat;
    [SerializeField] public SpriteRenderer _sr;
    public Animator anim;
    
    public override void Death()
    {
        Destroy(this.gameObject);
    }



}
