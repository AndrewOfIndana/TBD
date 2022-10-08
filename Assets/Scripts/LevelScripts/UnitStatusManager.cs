using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusManager : MonoBehaviour, Ieffectable
{
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    public void ApplyEffect(StatusEffect effect)
    {
        statusEffects.Add(effect);
    }
    public void RemoveEffect(int statusIndex)
    {
        if(statusEffects[statusIndex] != null)
        {
            statusEffects.Remove(statusEffects[statusIndex]);
        }
    }
}
