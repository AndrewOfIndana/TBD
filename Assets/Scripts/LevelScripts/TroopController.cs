using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TroopController : MonoBehaviour, Ieffectable
{
    /*  
        Name: TroopController.cs
        Description: This script contains and handles the variables used for the both the behaviour and movement of a troop unit 

    */
    [Header("Script References")]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    private TroopBehaviour troopBehaviour;
    private TroopMovement troopMovement;

    [Header("GameObject References")]
    public Animator animator;
    public Image healthBar; 
    public SpriteRenderer thisSprite; 
    public BoxCollider thisCollider;

    /*[Header("Stats Variables")]*/
    [HideInInspector] public Stats stat;
    public float attack;
    public float health; 
    public float speed;
    public float attackRate;
    public float attackRange;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        troopMovement = this.GetComponent<TroopMovement>();
        troopBehaviour = this.GetComponent<TroopBehaviour>();
    }
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit(Stats newStats)
    {
        stat = newStats;
        attack = newStats.unitAttack;
        health = newStats.unitHealth;
        speed = newStats.unitSpeed;
        attackRate = newStats.unitAttackRate;
        attackRange = newStats.unitAttackRange;
        thisSprite.sprite = newStats.unitSprite;
        thisCollider.size =  newStats.unitSize;
        healthBar.fillAmount = health/newStats.unitHealth;
    }
    /*-  Starts the unit's behaviour and movement -*/
    public void StartController()
    {
        animator.speed = stat.unitWalkSpeed;
        troopBehaviour.StartBehaviour(); //Starts the troop's Behaviour
        troopMovement.StartMovement(); //Starts the troop's Movement
    }

    /*---      FUNCTIONS     ---*/
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
            StartCoroutine(RemoveEffect(appliedEffect, effectIndex, appliedEffect.effectLifetime)); //Calls UpdateTarget IEnumerator at 1 second
        }
    }
    /*-  Buffs the units stats when applying a status effect -*/
    private void BuffUnit(int effectIndex)
    {
        attack = GetBuffedStat(attack, stat.unitAttack, effectIndex, BuffedStats.attack);
        health = GetBuffedStat(health, health, effectIndex, BuffedStats.health);
        speed = GetBuffedStat(speed, stat.unitSpeed, effectIndex, BuffedStats.speed);
        attackRate = GetBuffedStat(attackRate, stat.unitAttackRate, effectIndex, BuffedStats.attackRate);
        attackRange = GetBuffedStat(attackRange, stat.unitAttackRange, effectIndex, BuffedStats.attackRange);
    }
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
    public IEnumerator RemoveEffect(StatusEffect decayedEffect, int effectIndex, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        statusEffects.Remove(decayedEffect);
        BuffUnit(effectIndex);
    }
}