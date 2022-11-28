using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TowerController : MonoBehaviour, Idamageable, Ieffectable
{
    /*  
        Name: TowerController.cs
        Description: This script contains and handles the variables used for the behaviour of a tower unit

    */
    /*[Header("Static References")]*/
    GameManager gameManager;
    LevelManager levelManager;

    /*[Header("Components")]*/
    private TowerBehaviour towerBehaviour;

    [Header("GameObject Reference")]
    public Animator animator;
    public SpriteRenderer thisSprite; 
    public BoxCollider thisCollider; 
    private AudioSource audioSource;

    [Header("UI References")]
    public Image healthBar; 
    public GameObject[] statusUI; //Array of each status effect symbol
    private Color healthColor;
    private Color buffedHpColor = Color.yellow;
    private Color enragedColor = new Color(1f, 0.16f, 0.14f);

    /*[Header("Stats")]*/
    private Stats stat;
    private float attack;
    private float health; 
    private float attackRate;
    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        towerBehaviour = this.GetComponent<TowerBehaviour>();
        audioSource = this.GetComponent<AudioSource>();
        healthColor = healthBar.color;
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        levelManager = LevelManager.instance;
    }
    /*-  Sets the units stats when the object has spawned from pool using the newStats Stats variables -*/
    public void SetUnit(Stats newStats)
    {
        stat = newStats;
        attack = newStats.unitAttack;
        health = newStats.unitHealth;
        attackRate = newStats.unitAttackRate;
        thisSprite.sprite = newStats.unitUtils.unitSprite;
        thisCollider.size =  newStats.unitUtils.unitSize;
        healthBar.fillAmount = health/newStats.unitHealth;

        if(stat.unitUtils.unitsSfx != null)
        {
            audioSource.clip = stat.unitUtils.unitsSfx.GetRandomSfx();
        }
        this.gameObject.tag = newStats.unitUtils.unitTag;
    }
    /*-  Starts the unit's behaviour and movement -*/
    public void StartController()
    {
        for(int i = 0; i < statusUI.Length; i++)
        {
            statusUI[i].SetActive(false);
        }
        towerBehaviour.StartBehaviour(); //Starts the unit's Behaviour
        audioSource.Play();
    }

    /*---      FUNCTIONS     ---*/
    /*-  Handles applying a status effect for a unit takes a StatusEffect for the applied effect -*/
    public void ApplyEffect(StatusEffect appliedEffect)
    {
        //if statusEffects doesn't contain appliedEffect
        if(!statusEffects.Contains(appliedEffect))
        {
            statusEffects.Add(appliedEffect);
            statusUI[appliedEffect.effectId].SetActive(true);
            BuffUnit();
        }
    }
    /*-  Buffs the units stats when applying a status effect -*/
    private void BuffUnit()
    {
        /*  Makes new variables for the new attack  */
        float newAttack = stat.unitAttack;
        float newHealth = health > stat.unitHealth ? newHealth = stat.unitHealth: newHealth = health; //If health is greater than set newHealth to stat.unitHealth, else set newHealth to health 
        float newAttackRate = stat.unitAttackRate;

        /*  Applies each status buff in each status effect for the unit's stats  */
        for(int i = 0; i < statusEffects.Count; i++)
        {
            for(int k = 0; k < statusEffects[i].effects.Count; k++)
            {
                newAttack = newAttack * statusEffects[i].effects[k].GetStatusBonus(BuffedStats.attack);
                newHealth = newHealth * statusEffects[i].effects[k].GetStatusBonus(BuffedStats.health);
                newAttackRate = newAttackRate * statusEffects[i].effects[k].GetStatusBonus(BuffedStats.attackRate);
            }
        }

        /*  Sets stat values to newStats values */
        attack = newAttack;
        health = newHealth;
        attackRate = newAttackRate;
        UpdateHealthUI();
    }
    /*-  Calls DecayEffect when called from another script -*/
    public void StartDecayEffect(StatusEffect decayingEffect, float lifeTime)
    {
        StartCoroutine(DecayEffect(decayingEffect, lifeTime));
    }
    /*-  Handles the lifetime of a status effect, takes the StatusEffect that is decaying and float for the life time -*/
    public IEnumerator DecayEffect(StatusEffect decayingEffect, float lifeTime)
    {
        //if statusEffects contains decayingEffect
        if(statusEffects.Contains(decayingEffect))
        {
            yield return new WaitForSeconds(lifeTime);
            RemoveEffect(decayingEffect);
        }
    }
    /*-  Handles removing a status effect from a unit takes the StatusEffect that will be removed  -*/
    public void RemoveEffect(StatusEffect removedEffect)
    {
        //if statusEffects contains removedEffect
        if(statusEffects.Contains(removedEffect))
        {
            statusUI[removedEffect.effectId].SetActive(false);
            statusEffects.Remove(removedEffect);
            BuffUnit();
        }
    }
    /*-  Updates the health bar of a unit -*/
    private void UpdateHealthUI()
    {
        //if health is greater than stat.unitHealth and levelManager's isEnraged is true
        if(health > stat.unitHealth && levelManager.GetIsEnraged())
        {
            healthBar.color = enragedColor;
        }
        else if(health > stat.unitHealth && !levelManager.GetIsEnraged()) //if health is greater than stat.unitHealth and levelManager's isEnraged is false
        {
            healthBar.color = buffedHpColor;
        }
        else
        {
            healthBar.color = healthColor;
            healthBar.fillAmount = health/stat.unitHealth; //Resets healthBar
        }
    }

    /*-  When a GameObject collides with another GameObject, Unity calls OnTriggerEnter. -*/
    private void OnTriggerEnter(Collider other)
    {
        //if the boss collides with an opposing bullet
        if (other.gameObject.CompareTag(stat.unitUtils.sharedTags.oncomingBulletTag))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>(); 
            TakeDamage(bullet.GetAttack()); //Transfer bulletAttack to the this script's TakeDamage function
            bullet.DestroyBullet();
        }
    }

    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthUI();

        //if health is less than or equal to 0
        if(health <= 0 && this.gameObject.activeSelf)
        {
            towerBehaviour.VoidTargets();
            
            if(stat.unitUtils.unitsSfx != null)
            {
                AudioSource.PlayClipAtPoint(stat.unitUtils.unitsSfx.deathSfx, this.transform.position, gameManager.GetGameOptions().GetVoiceClipVolume());
            }
            this.gameObject.SetActive(false); 
        }
    }
    /*-  OnDisable is called when the object becomes disabled -*/
    private void OnDisable()
    {
        statusEffects.Clear(); //Clears statusEffects
        audioSource.Stop();
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
