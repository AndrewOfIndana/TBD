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
    /*-  Handles applying a status effect for a unit takes a StatusEffect for the applied effect -*/
    void ApplyEffect(StatusEffect appliedEffect);
    /*-  Calls DecayEffect when called from another script -*/
    void StartDecayEffect(StatusEffect decayingEffect, float lifeTime);
    /*-  Handles the lifetime of a status effect, takes the StatusEffect that is decaying and float for the life time -*/
    IEnumerator DecayEffect(StatusEffect decayedEffect, float lifeTime);
    /*-  Handles removing a status effect from a unit takes the StatusEffect that will be removed  -*/
    void RemoveEffect(StatusEffect removedEffect);
}
