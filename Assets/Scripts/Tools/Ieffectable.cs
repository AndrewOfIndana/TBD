using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ieffectable
{
    /*  
        Name: Ieffectable.cs
        Description: An interface that allows any object to receive a status effect

    */
    /*---      FUNCTIONS     ---*/
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    void ApplyEffect(StatusEffect effect);
    IEnumerator RemoveEffect(StatusEffect effect, int index, float time);
}
