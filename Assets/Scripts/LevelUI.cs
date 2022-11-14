using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    /*  
        Name: LevelUI.cs
        Description: This script controls all level ui elements as well as updating them

    */
    public static LevelUI instance;

    /*[Header("Static References")]*/
    GameManager gameManager;

    /*[Header("Components")]*/
    private LevelManager levelManager;
    private Level level; 

    [Header("Controller References")]
    public PlayerSpawner playerSpawner;
    public PlayerController playerController;
    public EnemySpawner enemySpawner;

    [Header("Script References")]
    public GameObject[] gameScreens;
    public GameObject Cards;
    public GameObject Units;
    public Slider[] sliders;

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
    public Image enragedClockSection;
    public GameObject enragedClockTick;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        /* SINGLETON PATTERN */
        //if the instance does exist and the instance isn't this
        if (instance != null && instance != this) 
        { 
            return;
        } 
        else 
        { 
            instance = this; 
        } 

        /* Gets the components */
        levelManager = this.gameObject.GetComponent<LevelManager>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {    
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;

        level = levelManager.GetLevel();

        /* Adds listeners for each buttons the player has */
        for(int i = 0; i < selectUnitIcons.Length; i++)
        {
            AddListeners(selectUnitIcons[i].GetComponent<Button>(), i);
            AddListeners(unitIcons[i].GetComponent<Button>(), i);
        }

        /*  Hides the unused buttons  */
        HideUI(selectUnitIcons, levelManager.GetLevelPlayerUnits().Count); 
        HideUI(unitIcons, level.unitLimit);

        UpdateSetUpText(); //Changes text for the setup UI text like the title or unit limit
        UpdateSetUpUI(); //Updates the thumbnails of the sprites
        UpdateSliders();
        startGameButton.SetActive(false);
        UpdatePlayerDeath(true);
    }

    /*---      FUNCTIONS     ---*/
    /*-  Adds listeners to each button, takes a button and the index from the for loop -*/
    private void AddListeners(Button btn, int index)
    {
        btn.onClick.AddListener(() => { OnButtonClick(index); }); //Adds a listeners a button
    }
    /*-  Checks if a button is clicked, uses an index to indicate which button, OnClick -*/
    private void OnButtonClick(int index)
    {
        //if gameStates is SETUP
        if(gameManager.GetGameState() == GameStates.SETUP)
        {
            levelManager.AddOrRemoveUnit(index);
        }
        if(gameManager.GetGameState() == GameStates.PLAYING) //if gameStates is PLAYING
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
        levelNameTxt.text = "Level " + level.levelID + " - " + level.levelName;
        levelUnitCount.text = "Pick " + level.unitLimit + " units";

        //Updates the select unit buttons sprites
        for(int i = 0; i < levelManager.GetLevelPlayerUnits().Count; i++)
        {
            selectUnitIcons[i].sprite = levelManager.GetLevelPlayerUnits()[i].unitThumbnail;
        }

        enragedClockSection.fillAmount = level.enemyEnragedTime/level.GetTotalTime(); //Updates the enragedClockSection fillAmount
    }
    /*-  Updates the sprites of chosen units when a player selects a unit in the setup screen, also does the same for the player's game sprites   -*/
    public void UpdateSetUpUI()
    {
        //Updates the chosen unit buttons sprites
        for(int i = 0; i < chosenUnitIcons.Length; i++)
        {
            //if i is less than levelManager's playerUnitCount
            if(i < levelManager.GetPlayerUnitsCount())
            {
                chosenUnitIcons[i].gameObject.SetActive(true);
                chosenUnitIcons[i].sprite = levelManager.GetPlayerUnits()[i].unitThumbnail;
            }
            else
            {
                chosenUnitIcons[i].gameObject.SetActive(false);
            }
        }
        //Updates the unit buttons sprites
        for(int k = 0; k < levelManager.GetPlayerUnits().Count; k++)
        {
            unitIcons[k].sprite = levelManager.GetPlayerUnits()[k].unitThumbnail;
        }
    }
    /*-  Updates the Game UI -*/
    public void UpdateUI()
    {
        playerHealthBar.fillAmount = playerSpawner.GetHealth()/playerSpawner.GetMaxHealth();
        enemyHealthBar.fillAmount = enemySpawner.GetHealth()/enemySpawner.GetMaxHealth();
        manaBar.fillAmount = playerController.GetMana()/100f;
        manaTxt.text = "Mana: " + playerController.GetMana(); 
    }
    /*-  Updates the Enraged Clock UI -*/
    public void UpdateClock()
    {
        enragedClockTick.transform.Rotate(Vector3.forward, 360f/level.GetTotalTime());
    }
    /*-  Turns off hud when the player dies or not -*/
    public void UpdatePlayerDeath(bool isDead)
    {
        Cards.SetActive(isDead);
        Units.SetActive(isDead);
    }
    public void CameraZoomSlide(float s)
    {
        gameManager.GetGameOptions().SetCameraZoom(s);
    }
    public void MusicVolumeSlide(float s)
    {
        gameManager.GetGameOptions().SetMusicVolume(s);
    }
    public void GameVolumeSlide(float s)
    {
        gameManager.GetGameOptions().SetEffectVolume(s);
    }
    public void VoiceVolumeSlide(float s)
    {
        gameManager.GetGameOptions().SetVoiceVolume(s);
    }
    public void BrightnessSlide(float s)
    {
        gameManager.GetGameOptions().SetBrightness(s);
    }
    public void UpdateSliders()
    {
        for(int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = gameManager.GetGameOptions().GetOptionsValues(i);
        }
    }
    /*-  Updates the Swaps the screen, takes an index for chosen screen -*/
    public void UpdateScreen(int index)
    {
        UpdateSliders();
        //Sets all game screens to false
        for(int i = 0; i < gameScreens.Length; i++)
        {
            gameScreens[i].SetActive(false);
        }
        gameScreens[index].SetActive(true);
    }
}
