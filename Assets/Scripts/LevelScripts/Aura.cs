using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Aura : MonoBehaviour
{
    /*  
        Name: Aura.cs
        Description: This script controls the auras of the player and mages, auras can apply status effects if other units are nearby

    */
    [Header("Script Settings")]
    private SphereCollider auraCollider;
    private List<StatusEffect> auraEffects = new List<StatusEffect>();
    private bool isAppliedByEnemy;
    private bool isAppliedToEnemy;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        auraCollider = this.GetComponent<SphereCollider>();
        DisableAura();
    }

    public void EnableAura(List<StatusEffect> effects, bool fromEnemy, bool toEnemy)
    {
        auraCollider.enabled = true;
        auraEffects = effects;
        isAppliedByEnemy = fromEnemy;
        isAppliedToEnemy = toEnemy;
    }

    public void EnableAura(bool fromEnemy, bool toEnemy)
    {
        auraCollider.enabled = true;
        isAppliedByEnemy = fromEnemy;
        isAppliedToEnemy = toEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit otherUnit = other.GetComponent<Unit>();

        if(otherUnit != null)
        {
            if(isAppliedByEnemy && isAppliedToEnemy && otherUnit.IsEnemy())
            {
                SpreadEffects(other);
            }
            else if(!isAppliedByEnemy && !isAppliedToEnemy && !otherUnit.IsEnemy())
            {
                SpreadEffects(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        RemoveEffects(other);
    }

    private void SpreadEffects(Collider other)
    {
        Ieffectable otherStatus = other.GetComponent<Ieffectable>();
        if(otherStatus != null)
        {
            for(int i = 0; i < auraEffects.Count; i++)
            {
                otherStatus.ApplyEffect(auraEffects[i]);
            }
        }
    }
    private void RemoveEffects(Collider other)
    {
        Ieffectable otherStatus = other.GetComponent<Ieffectable>();
        if(otherStatus != null)
        {
            for(int i = 0; i < auraEffects.Count; i++)
            {
                otherStatus.StartDecayEffect(auraEffects[i], auraEffects[i].effectLifetime);
            }
        }
    }

    public void DisableAura()
    {
        auraCollider.enabled = false;
    }

    public void SetAuraEffect(List<StatusEffect> effects)
    {
        auraEffects = effects;
    }
}
