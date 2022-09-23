using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    /*  
        Name: PlayerController.cs
        Description: This script allows the player to choose what troops and towers the player can choose. The script also handles mana and the ui

    */

    public static PlayerController playerControllerInstance; //A static instance of this script that can be accessed in any script 

    [Header("Script References")]
    public PlayerTroopSpawner playerTroopSpawner; //A reference to the Player's troop spawner
    public Stats[] units; //The types of unit the player has access to
    private Stats towerToPlace; //The current tower the player has selected

    [Header("PlayerController Values")]
    public float mana = 100; //Stores the current amount of mana
    public float manaRegen = 2; //Stores the mana regenerate rate

    [Header("UI References")]
    public Button[] unitButtons; //Array of buttons that spawns the units
    public Image[] unitButtonImgs; //Array of images of said buttons
	public TextMeshProUGUI manaTxt; //Reference to the mana text
    private Color selected = Color.green; //Stores the color of a selected button
    private Color unselected; //Stores the color of an unselected button
    private int lastSelection; //Stores the previous index

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Starts when the script is first awake -*/
    void Awake()
    {
        //if another playerUnitsInstance exists 
        if(playerControllerInstance != null)
        {
            Debug.Log("Error more than one playerControllerInstance");
            return; //exit if statement
        }
        playerControllerInstance = this; //Sets playerUnitsInstance to this script
    }
    /*-  Starts on the first frame -*/
    void Start()
    {
        UpdateUI(); //Calls the UpdateUI function
        lastSelection = 6; //Sets the lastSelection to 6
        towerToPlace = units[lastSelection]; //Sets towerToPlace to units key lastSelection

        for(int i = 0; i < unitButtons.Length; i++)
        {
            AddListeners(unitButtons[i], i); //Calls AddListeners in a for loop
        }
        unselected = unitButtonImgs[0].color; //Sets the unselected to the default img color
        unitButtonImgs[lastSelection].color = selected; //Sets the current towerToPlace button color to the selected img color
        StartCoroutine(RegenerateMana(1f)); //Calls RegenerateMana IEnumerator at 1 second
    }
    /*-  Adds listeners to each button, takes a button and the index from the for loop -*/
    private void AddListeners(Button btn, int index)
    {
        btn.onClick.AddListener(() => { OnButtonClick(index); }); //Adds a listeners a button
    }

    /*---      FUNCTIONS     ---*/
        /*-  Updates UI -*/
    public void UpdateUI()
    {
        manaTxt.text = "Mana: " + mana; //Updates mana count
    }
    /*-  Checks if a button is clicked, uses an index to indicate which button -*/
    private void OnButtonClick(int index)
    {
        //if the mana minus the unitCost isn't less than or equal to 0
        if((mana - units[index].unitCost) >= 0)
        {
            //if the buttons are troop deploying buttons
            if(index < 6)
            {
                mana -= units[index].unitCost; //Subtracts the unitCost from the mana
                playerTroopSpawner.SpawnTroop(units[index]); //Calls the spawnTroop function in the playerTroopSpawner script
                UpdateUI(); //Calls the UpdateUI function
            }
            else if(index >= 6) //if the buttons are tower deploying buttons
            {
                towerToPlace = units[index]; //Changes towerToPlace to the selected tower
                unitButtonImgs[index].color = selected; //Changes the selected button to the selected color
                
                //if the lastSelection isn't the index
                if(lastSelection != index)
                {
                    unitButtonImgs[lastSelection].color = unselected; //Changes the button image to unselected
                    lastSelection = index; //Sets lastSelection to index
                }
            }
        }
    }
    /*-  Gets the tower to place -*/
    public Stats GetTowerToPlace()
    {
        return towerToPlace;
    }
    /*-  Repeatedly regenerates mana, takes a float for the time -*/
    private IEnumerator RegenerateMana(float time)
    {
        yield return new WaitForSeconds(time); //Waits for time

        //if the mana plus manaRegen is less than 100
        if((mana + manaRegen) <= 100)
        {
            mana += manaRegen; //Adds manaRegen to mana
        }
        UpdateUI(); //Calls the UpdateUI function
        StartCoroutine(RegenerateMana(1f)); //Recalls RegenerateMana IEnumerator at 1 second
    }
}
