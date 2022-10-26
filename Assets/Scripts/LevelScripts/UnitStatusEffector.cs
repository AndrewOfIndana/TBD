using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusEffector : MonoBehaviour
{
    public StatusEffect[] effects;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Ieffectable>() != null)
        {
            Ieffectable otherStatus = other.GetComponent<Ieffectable>();
            for(int i = 0; i < effects.Length; i++)
            {
                otherStatus.ApplyEffect(effects[i]);
                otherStatus.StartDecayEffect(effects[i], effects[i].effectLifetime);
            }
        }
    }
}
