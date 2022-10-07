using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitController
{
    /*  
        Name: IUnitController.cs
        Description: An interface that adds the abstracts function TakeDamage, which allows any object to get damaged

    */
    /*---      FUNCTIONS     ---*/
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    void TakeDamage(float damage);
}
