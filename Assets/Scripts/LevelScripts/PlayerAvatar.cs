using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviour, Idamageable
{
    /*  
        Name: PlayerAvatar.cs
        Description: This script controls the player avatar and the counts as a unit
        
    */
    /*[Header("Static Variables")]*/
    List<Transform> availableTiles;

    [Header("GameObject References")]
    public Image healthBar;
    private Rigidbody playerRb;

    [Header("Stats Variables")]
    public Stats stat;
    [HideInInspector] public float attack;
    [HideInInspector] public float health;
    [HideInInspector] public float speed;
    [HideInInspector] public float attackRate;
    [HideInInspector] public float attackRange;

    [Header("Script References")]
    public Transform closestTile;
    private Idamageable targetEngaged; //Private reference to the enemy troop the troop is engaged with
    private Vector3 velocity;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        playerRb = this.GetComponent<Rigidbody>();
        availableTiles = Tile.GetTiles(); //Gets the list of transform from Tile
        closestTile = availableTiles[0]; //Sets closestTile to the first availableTiles list item
    }
    /*-  OnEnable is called when the object becomes enabled -*/
    private void OnEnable()
    {
        /* Gets the Stats from stats and store them in Stats Variables */
        attack = stat.unitAttack;
        health = stat.unitHealth;
        speed = stat.unitSpeed;
        attackRate = stat.unitAttackRate;
        attackRange = stat.unitAttackRange;
        StartCoroutine(RegenerateHealth(1f)); //Calls RegenerateMana IEnumerator at 1 second
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        /* Movement Code */
        velocity.x = Input.GetAxis("Horizontal");
        velocity.z = Input.GetAxis("Vertical");
        playerRb.MovePosition(playerRb.position + velocity * speed * Time.deltaTime);

        /* Combat Code */
        //if the player clicks the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Fires a raycast at where the player clicks the mouse
            if (Physics.Raycast(ray, out hit, attackRange))
            {
                //If what the player hit is the same as the player's stat.targetTags[0 ,1 and 2]
                if (hit.transform.gameObject.tag == stat.targetTags[0] || hit.transform.gameObject.tag == stat.targetTags[1] || hit.transform.gameObject.tag == stat.targetTags[2])
                {
                    targetEngaged = hit.transform.gameObject.GetComponent<Idamageable>();
                    targetEngaged.TakeDamage(attack); //Transfer the players's attack to the  targetEngaged script's TakeDamage function
                }
            }
        }
    }

    /*---      FUNCTIONS     ---*/
    /*-  Repeatedly regenerates health, takes a float for the time -*/
    private IEnumerator RegenerateHealth(float time)
    {
        yield return new WaitForSeconds(time);

        /* Checks if the player is close to a tile and sets closestTilt to closets tile */
        for (int i = 0; i < availableTiles.Count; i++)
        {
            //If he player is near an availableTiles
            if (Vector3.Distance(availableTiles[i].position, this.transform.position) < 2.5f)
            {
                closestTile = availableTiles[i];
            }
        }

        /* Health regeneration */

        //if the mana plus manaRegen is less than 100
        if ((health + 1) <= stat.unitHealth)
        {
            health += 1;
        }
        healthBar.fillAmount = health / stat.unitHealth; //Resets healthBar
        StartCoroutine(RegenerateHealth(1f));
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / stat.unitHealth; //Resets healthBar

        //if health is less than or equal to 0
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
