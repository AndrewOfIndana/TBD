using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviour, IUnitController
{
    /*  
        Name: PlayerAvatar.cs
        Description: This script controls the player avatar
        
    */
    public Image healthBar; //Reference to the health bar image of the troop
    private Rigidbody playerRb; //Reference to a rigidbody

    /*-  UnitStats Values -*/
    public Stats stat; //Stores the Stats of the troop
    [HideInInspector] public float attack; //Store the attack of the troop
    [HideInInspector] public float health; //Store the health of the troop
    [HideInInspector] public float attackRate; //Store the attack rate of the troop
    [HideInInspector] public float attackRange; //Store the attack range of the troop

    /*-  Script References -*/
    private IUnitController targetEngaged; //Private reference to the enemy troop the troop is engaged with

    [Header("Movement Variables")]
    [HideInInspector] public float speed; //Store the speed of the troop
    private Vector3 velocity; //Stores the velocity of the player

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts on the first frame -*/
    private void Start()
    {
        playerRb = this.GetComponent<Rigidbody>(); //Gets the rigidbody component
        attack = stat.unitAttack;
        health = stat.unitHealth;
        speed = stat.unitSpeed;
        attackRate = stat.unitAttackRate;
        attackRange = stat.unitAttackRange;
        StartCoroutine(RegenerateHealth(1f)); //Calls RegenerateMana IEnumerator at 1 second
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Is called every frame -*/
    private void Update()
    {
        velocity.x = Input.GetAxis("Horizontal"); //Set velocity x from the input horizontal axis
        velocity.z = Input.GetAxis("Vertical"); //Set velocity z from the input vertical axis

        playerRb.MovePosition(playerRb.position + velocity * speed * Time.deltaTime); //Moves player


        if(Input.GetMouseButtonDown(0))
        { 
            RaycastHit hit; 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

            if (Physics.Raycast(ray, out hit, attackRange))
            {
                if(hit.transform.gameObject.tag == stat.targetTags[0] || hit.transform.gameObject.tag == stat.targetTags[1] || hit.transform.gameObject.tag == stat.targetTags[2])
                {
                    targetEngaged = hit.transform.gameObject.GetComponent<IUnitController>(); //Gets the TroopController component and stores it in targetEngaged
                    targetEngaged.TakeDamage(attack);
                }
            }
        }
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage; //Subtracts from health with damage
        healthBar.fillAmount = health/stat.unitHealth; //Resets healthBar by dividing health by maxHealth

        //if health is less than or equal to 0
        if(health <= 0)
        {
            this.gameObject.SetActive(false); //deactivate the troop
            StartCoroutine(RespawnPlayer(10));
        }
    }
    private IEnumerator RegenerateHealth(float time)
    {
        yield return new WaitForSeconds(time); //Waits for time

        //if the mana plus manaRegen is less than 100
        if((health + 1) <= stat.unitHealth)
        {
            health += 1; //Adds manaRegen to mana
        }
        healthBar.fillAmount = health/stat.unitHealth; //Resets healthBar by dividing health by maxHealth
        StartCoroutine(RegenerateHealth(1f)); //Recalls RegenerateMana IEnumerator at 1 second
    }
    public IEnumerator RespawnPlayer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); //Waits for rate
        this.gameObject.SetActive(true);
    }
}
