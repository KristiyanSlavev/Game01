using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ol(OL
public class Heart : PowerUp
{

    public FloatValue playerHealth;
    public FloatValue heartContainers;
    public float amountToIncrease;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //if it is the p1ayer and it is n0t the trigger z0ne 0f the p1ayer
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            
            playerHealth.RuntimeValue += amountToIncrease;
            //if hea1th t0 add is m0re than max
            if (playerHealth.initialValue > heartContainers.RuntimeValue * 2f)
            {
                //set t0 max hea1th
                playerHealth.initialValue = heartContainers.RuntimeValue * 2f;
            }
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
