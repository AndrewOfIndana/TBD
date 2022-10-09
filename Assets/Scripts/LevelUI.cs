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
    private LevelManager levelManager;

    [Header("Script References")]
    public GameObject[] gameScreens;
    public GameObject Cards;
    public GameObject Units;

    [Header("SetupUI References")]
    public Image[] selectUnitIcons;
    public Image[] chosenUnitIcons;
    public TextMeshProUGUI levelNameTxt;
    public TextMeshProUGUI levelUnitCount;
    public GameObject startGameButton;

    [Header("GameUI References")]
    public Image[] unitIcons;
    public Image playerHealthBar;
    public Image enemyHealthBar;
    public Image manaBar;
    public TextMeshProUGUI manaTxt;

    public Image EnragedCycle;
    public GameObject arrow;

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
        levelManager = this.gameObject.GetComponent<LevelManager>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {        
        /* Adds listeners for each buttons the player has */
        for(int i = 0; i < selectUnitIcons.Length; i++)
        {
            AddListeners(selectUnitIcons[i].GetComponent<Button>(), i);
            AddListeners(unitIcons[i].GetComponent<Button>(), i);
        }

        HideUI(selectUnitIcons, levelManager.levelPlayerUnits.Count); //Hides the unused buttons for select units
        HideUI(unitIcons, levelManager.levelUnitLimit);  //Hides the unused buttons for units
        UpdateSetUpText(); //Changes text for the setup UI text like the title or unit limit
        UpdateSetUpUI(); //Updates the thumbnails of the sprites
        startGameButton.SetActive(false);
        UpdatePlayerDeath(true);
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
        if(levelManager.gameState == GameStates.SETUP)
        {
            levelManager.AddOrRemoveUnit(index);
        }
        if(levelManager.gameState == GameStates.PLAYING)
        {
            playerController.SpawnUnit(index); //Sends index to playerController
        }
    }
    /*-  Hides unused buttons or images by setting the images inactive in reverse order, takes a Image[] for the array of images affected, and a int for the listSize   -*/
    public void HideUI(Image[] icons, int listSize)
    {
        for(int i = icons.Length - 1; i >= listSize; i--)
        {
            icons[i].gameObject.SetActive(false);
        }
    }
    /*-  Updates the setup texts and even the unit select thumbnails, should only be called once   -*/
    private void UpdateSetUpText()
    {
        levelNameTxt.text = "Level " + levelManager.levelNum + " - " + levelManager.levelName;
        levelUnitCount.text = "Pick " + levelManager.levelUnitLimit + " units";

        //Updates the select unit buttons sprites
        for(int i = 0; i < levelManager.levelPlayerUnits.Count; i++)
        {
            selectUnitIcons[i].sprite = levelManager.levelPlayerUnits[i].unitThumbnail;
        }
    }
    /*-  Updates the sprites of chosen units when a player selects a unit in the setup screen, also does the same for the player's game sprites   -*/
    public void UpdateSetUpUI()
    {
        //Updates the chosen unit buttons sprites
        for(int i = 0; i < chosenUnitIcons.Length; i++)
        {
            //if i is less than levelManager's playerUnitCount
            if(i < levelManager.playerUnitCount)
            {
                chosenUnitIcons[i].gameObject.SetActive(true);
                chosenUnitIcons[i].sprite = levelManager.playerUnits[i].unitThumbnail;
            }
            else
            {
                chosenUnitIcons[i].gameObject.SetActive(false);
            }
        }
        //Updates the unit buttons sprites
        for(int k = 0; k < levelManager.playerUnits.Count; k++)
        {
            unitIcons[k].sprite = levelManager.playerUnits[k].unitThumbnail;
        }
    }
    /*-  Updates the Game UI -*/
    public void UpdateUI()
    {
        playerHealthBar.fillAmount = playerSpawner.health/playerSpawner.maxHealth;
        enemyHealthBar.fillAmount = enemySpawner.health/enemySpawner.maxHealth;
        manaBar.fillAmount = playerController.mana/100f;
        manaTxt.text = "Mana: " + playerController.mana; 
    }
    /*-  Turns off hud when the player dies or not -*/
    public void UpdatePlayerDeath(bool isDead)
    {
        Cards.SetActive(isDead);
        Units.SetActive(isDead);
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

    // private void Update()
    // {
        // tick.transform.Rotate(Vector3.forward, (Time.deltaTime * (360f/(LevelManager.levelManagerInstance.HordeCalmTime + LevelManager.levelManagerInstance.HordeEnragedTime))));
        // timeE += Time.deltaTime;
        // Debug.Log(timeE);
    // }
    // public void SetKnobs()
    // {
    //     Debug.Log("CKICK");
    // }
}
