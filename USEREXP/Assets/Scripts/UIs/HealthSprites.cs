using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthSprites : MonoBehaviour
{
 
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateHealth(int health)
    {
        for (int i = 0; i<hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;

            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

        }
    }
}
