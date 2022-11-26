using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSetter : MonoBehaviour
{
    /*  
        Name: EnemySpawner.cs
        Description: This script sets an already existing tower in a level

    */
    public Stats towerStats;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        TowerController tower = this.GetComponent<TowerController>();

        //if this enemy exist
        if(tower != null)
        {
            tower.SetUnit(towerStats); //Sets enemy type and stats based on random number generator
            tower.StartController(); //Starts the enemy controller
        }
    }
}
