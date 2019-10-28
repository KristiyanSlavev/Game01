using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfFullHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        InitHearts();
    }

    public void InitHearts()
    {
        for(int i = 0; i< heartContainers.initialValue; i++)
        {
            //Make hearts active and set them full
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }

    public void UpdateHearts()
    {
        //hearts and halves
        float tempHealth = playerCurrentHealth.RuntimeValue / 2;
        //loop over all heart containers and compare; 
        for(int i =0; i<heartContainers.initialValue; i++)
        {
            if (i <= tempHealth-1)
            {
                //full Heart
                hearts[i].sprite = fullHeart;
            } else if (i >= tempHealth)
            {
                //empty Heart
                hearts[i].sprite = emptyHeart;
            }
            else
            {
                //half full Hear
                hearts[i].sprite = halfFullHeart;
            }
        }
    }
}
