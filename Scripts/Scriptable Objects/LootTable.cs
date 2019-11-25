using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public PowerUp thisLoot;
    //change to float if you want <1% drop rate
    public int lootChance;
}

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public Loot[] loots;

    public PowerUp LootPowerup()
    {
        //Use float here for lower than 1% chance
        int cumProb = 0;
        //Or make it higher here (but will be more complex)
        int currentProb = Random.Range(0, 100);
        for(int i =0;i<loots.Length; i++)
        {
            cumProb = +loots[i].lootChance;
            if(currentProb<= cumProb)
            {
                return loots[i].thisLoot;
            }
        }
        return null;
    }
}
