using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float decay;

    public void Awake()
    {

    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(DecayTimer(decay));
    }

    public IEnumerator DecayTimer(float decayTime)
    {
        yield return new WaitForSeconds(decayTime);
        ParticlePool.instance.ReturnToPool(this);
    }
}
