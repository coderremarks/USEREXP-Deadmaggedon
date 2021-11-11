using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Dummy : Unit
{

    public DummySpawner spawner;
    [SerializeField] private Material _whiteMat;
    [SerializeField] private Material _defaultMat;
    [SerializeField] private SpriteRenderer _sr;

    public override void InitializeStats()
    {
        base.InitializeStats();
       // anim.SetBool("isAlive", true);
        

    }
    public override void DamageHealth(float damage, string originName = null)
    {

        if (invincible == false)
        {
            if (GetComponent<Renderer>().isVisible == true && originName == "Player")
            {
                PlayerManager.instance.playerCamera.CameraShake(0.05f, 0.075f, 0f);
                //  StartCoroutine(GameManager.instance.WaitRealTime(StartTimeImpact, EndTimeImpact, 0.0125f));
            }



            base.DamageHealth(damage);
         
        }

        _sr.material = _whiteMat;
        Invoke("ResetDummyMaterial", 0.025f);
        var particle = ParticlePool.instance.GetObject(0);
        particle.transform.position = this.transform.position;
    }
    void ResetDummyMaterial()
    {
        _sr.material = _defaultMat;
    }
    public override void Death()
    {
        base.Death();
        //anim.SetBool("isAlive", false);

        Invoke("Despawn", 1);



    }
    public void Despawn()
    {
        spawner.RespawnDummy();

        DummyPool.instance.ReturnToPool(this);
    }

    public override void OnEnable()
    {
        base.OnEnable();
       // ChatBubble.Create(transform, new Vector3(0, 2), "aa");
    }
}
