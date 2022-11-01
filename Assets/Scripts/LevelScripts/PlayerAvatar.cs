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
    /*[Header("Static References")]*/
    GameManager gameManager;
    List<Transform> availableTiles;

    [Header("GameObject Reference")]
    public Animator animator;
    public Image healthBar;
    public Aura playerAura;
    private Rigidbody playerRb;

    [Header("Stats")]
    public Stats stat;
    private float attack;
    private float health;
    private float speed;
    private float attackRate;
    private float attackRange;
    private List<StatusEffect> auraStatusEffects = new List<StatusEffect>();

    /*[Header("Script Settings")]*/
    private Transform closestTile;
    private Idamageable targetEngaged; //Private reference to the enemy troop the troop is engaged with
    private Vector3 velocity;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        playerRb = this.GetComponent<Rigidbody>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
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
        healthBar.fillAmount = health / stat.unitHealth;
        StartCoroutine(RegenerateHealth(1f));
        //playerAura.EnableAura(auraStatusEffects, stat.isUnitEnemy, stat.isUnitEnemy);
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        //if gameStates is WIN or LOSE
        if(gameManager.CheckIfWinOrLose())
        {
            this.gameObject.SetActive(false);
        }
        //if gameStates is not PLAYING
        if(!gameManager.CheckIfPlaying())
        {
            return;
        }

        /* Movement Code */
        velocity.x = Input.GetAxis("Horizontal");
        velocity.z = Input.GetAxis("Vertical");
        playerRb.MovePosition(playerRb.position + velocity * speed * Time.deltaTime);
        animator.SetFloat("aSpeed", Mathf.Abs(velocity.magnitude));

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
                    animator.Play("playerAttack");
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

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            /* Checks if the player is close to a tile and sets closestTilt to closets tile */
            for (int i = 0; i < availableTiles.Count; i++)
            {
                //If the player is near an availableTiles
                if (Vector3.Distance(availableTiles[i].position, this.transform.position) < 2.75f)
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
            healthBar.fillAmount = health / stat.unitHealth;
        }
        //if gameStates isn't WIN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(RegenerateHealth(1f));
        }
    }
    /*-  Handles taking damage takes a float that is the oncoming damage value -*/
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / stat.unitHealth;

        //if health is less than or equal to 0
        if (health <= 0)
        {
            // playerAura.DisableAura();
            this.gameObject.SetActive(false);
        }
    }

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Gets closestTile -*/
    public Transform GetClosestTile()
    {
        return closestTile;
    }
}
