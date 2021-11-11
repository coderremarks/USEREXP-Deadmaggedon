using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleProp : MonoBehaviour
{
    public float decay;

    public void Awake()
    {
       
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        Color newColor = this.gameObject.GetComponent<SpriteRenderer>().material.color;
        newColor.a = 1;
        this.gameObject.GetComponent<SpriteRenderer>().material.color = newColor;
        StartCoroutine(DecayTimer(decay));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator DecayTimer(float decayTime)
    {
        yield return new WaitForSeconds(decayTime);
        for (float i = 1; i >= 0.1f; i -= 0.1f)
        {
            Color newColor = this.gameObject.GetComponent<SpriteRenderer>().material.color;
            newColor.a = i;
            this.gameObject.GetComponent<SpriteRenderer>().material.color = newColor;
            yield return new WaitForSeconds(0.1f);
        }

        ParticlePropPool.instance.ReturnToPool(this);
    }
}
