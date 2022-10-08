using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Idamageable
{
    /*  
        Name: Idamageable.cs
        Description: An interface that allows any object to get damaged

    */
    /*---      FUNCTIONS     ---*/
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    void TakeDamage(float damage);
}
