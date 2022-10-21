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
    /*-  Handles taking a status effect takes StatusEffect for the applied effect -*/
    void ApplyEffect(StatusEffect effect);
    /*-  Handles removing a status effect takes StatusEffect for the removed effect, the index of the removed effect, and a float for the time time -*/
    IEnumerator RemoveEffect(StatusEffect effect, int index, float time);
}
