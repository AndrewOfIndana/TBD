using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TowerController : MonoBehaviour, Ieffectable, Idamageable
{
    /*  
        Name: TowerController.cs
        Description: This script contains and handles the variables used for the behaviour of a tower unit
        
    */
    /*[Header("Components")]*/
    private TowerBehaviour towerBehaviour;

    [Header("GameObject Reference")]
    public Animator animator;
    public Image healthBar; 
    public SpriteRenderer thisSprite; 
    public BoxCollider thisCollider; 

    [Header("Stats")]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    private Stats stat;
    private float attack;
    private float health; 
    private float attackRate;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        towerBehaviour = this.GetComponent<TowerBehaviour>();
    }
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit(Stats newStats)
    {
        stat = newStats;
        attack = newStats.unitAttack; 
        health = newStats.unitHealth;
        attackRate = newStats.unitAttackRate;
        thisSprite.sprite = newStats.unitSprite;
        thisCollider.size =  newStats.unitSize;
        healthBar.fillAmount = health/newStats.unitHealth;
    }
    /*-  Resets the units stats when applying a status effect -*/
    private void ResetUnit(int index)
    {
        attack = attack * statusEffects[index].GetStatusBonus(BuffedStats.attack);
        health = health * statusEffects[index].GetStatusBonus(BuffedStats.health);
        attackRate = attackRate * statusEffects[index].GetStatusBonus(BuffedStats.attackRate);
    }
    /*-  Starts the unit's behaviour and movement -*/
    public void StartController()
    {
        towerBehaviour.StartBehaviour(); //Starts the troop's Behaviour
    }

    /*---      FUNCTIONS     ---*/
    /*-  Handles taking a status effect takes StatusEffect for the applied effect -*/
    public void ApplyEffect(StatusEffect appliedEffect)
    {
        statusEffects.Add(appliedEffect);

        if(statusEffects.Count != statusEffects.Distinct().Count())
        {
            statusEffects.Remove(appliedEffect);
        }
        else
        {
            int effectIndex = (statusEffects.Count - 1);
            BuffUnit(effectIndex);
            StartCoroutine(RemoveEffect(appliedEffect, effectIndex, (appliedEffect.effectLifetime * 2))); //Calls UpdateTarget IEnumerator at 1 second
        }
    }
    /*-  Buffs the units stats when applying a status effect -*/
    private void BuffUnit(int effectIndex)
    {
        attack = GetBuffedStat(attack, stat.unitAttack, effectIndex, BuffedStats.attack);
        health = GetBuffedStat(health, health, effectIndex, BuffedStats.health);
        attackRate = GetBuffedStat(attackRate, stat.unitAttackRate, effectIndex, BuffedStats.attackRate);
    }
    /*-  Applies the bonus to a unit's stat -*/
    private float GetBuffedStat(float buffedStat, float baseStat, int effectIndex, BuffedStats buffedVariable)
    {
        if(statusEffects.ElementAtOrDefault(effectIndex) != null)
        {
            return buffedStat * statusEffects[effectIndex].GetStatusBonus(buffedVariable);
        }
        else if(statusEffects.ElementAtOrDefault(effectIndex) == null)
        {
            return baseStat;
        }
        return baseStat;
    }
    /*-  Handles removing a status effect takes StatusEffect for the removed effect, the index of the removed effect, and a float for the time time -*/
    public IEnumerator RemoveEffect(StatusEffect decayedEffect, int effectIndex, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        statusEffects.Remove(decayedEffect);
        BuffUnit(effectIndex);
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health/stat.unitHealth; //Resets healthBar

        //if health is less than or equal to 0
        if(health <= 0)
        {
            this.gameObject.SetActive(false); 
        }
    }

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Get Stats -*/
    public Stats GetStats()
    {
        return stat;
    }
    /*-  Get attack -*/
    public float GetAttack()
    {
        return attack;
    }
    /*-  Get attack rate -*/
    public float GetAttackRate()
    {
        return attackRate;
    }
}
