using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    /*  
        Name: LevelUI.cs
        Description: This script controls all global ui elements as well as updating them

    */
    public static LevelUI levelUIinstance;

    [Header("Controller References")]
    public PlayerSpawner playerSpawner;
    public PlayerController playerController;
    public EnemySpawner enemySpawner;

    [Header("UI References")]
    public GameObject[] gameScreens;
    public Image playerHealthBar;
    public Image enemyHealthBar;
    public TextMeshProUGUI manaTxt; 
    public Button[] unitButtons;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        //if another levelUIinstance exists 
        if(levelUIinstance != null)
        {
            return; //exit if statement
        }
        levelUIinstance = this;
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Adds listeners for each buttons the player has */
        for(int i = 0; i < unitButtons.Length; i++)
        {
            AddListeners(unitButtons[i], i); 
        }
    }

    /*---      FUNCTIONS     ---*/
    /*-  Adds listeners to each button, takes a button and the index from the for loop -*/
    private void AddListeners(Button btn, int index)
    {
        btn.onClick.AddListener(() => { OnButtonClick(index); }); //Adds a listeners a button
    }
    /*-  Checks if a button is clicked, uses an index to indicate which button -*/
    private void OnButtonClick(int index)
    {
        playerController.SpawnUnit(index); //Sends index to playerController
    }
    /*-  Updates the Game UI -*/
    public void UpdateUI()
    {
        playerHealthBar.fillAmount = playerSpawner.health/playerSpawner.maxHealth;
        enemyHealthBar.fillAmount = enemySpawner.health/enemySpawner.maxHealth;
        manaTxt.text = "Mana: " + playerController.mana; 
    }
    /*-  Updates the Swaps the screen, takes an index for chosen screen -*/
    public void UpdateScreen(int index)
    {
        //Sets all game screens to false
        for(int i = 0; i < gameScreens.Length; i++)
        {
            gameScreens[i].SetActive(false);
        }
        gameScreens[index].SetActive(true);
    }
}
