using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    /*  
        Name: LevelManager.cs
        Description: This script handles all variables and states for a level

    */
    /*[Header("Static References")]*/
    GameManager gameManager;

    public static LevelManager instance;

    /*[Header("Components")]*/
    private LevelUI levelUI;

    [Header("Level Settings")]
    public Level level;
    private int levelUnitLimit;
    private List<Stats> levelPlayerUnits = new List<Stats>();
    private List<Stats> playerUnits = new List<Stats>(); //List of units that the player has chosen
    private int playerUnitCount = 0;
    private List<Stats> levelEnemyUnits = new List<Stats>();

    [Header("Controller References")]
    public PlayerController playerController;
    public PlayerSpawner playerSpawner;
    public EnemySpawner enemySpawner;
    public DeckCreator deckCreator;

    [Header("Player Avatar Settings")]
    public GameObject playerPrefab;
    private PlayerAvatar playerAvatar;
    private float respawnTime = 10f;
    private bool hasPlayerRespawned = true;

    [Header("Enraged Clock Settings")]
    public StatusEffect enragedStatus;
    private bool isEnraged; 
    private float clockCalmTime;
    private float clockEnragedTime;
    private float clockTime = 0;
    private int enragedCount = 0;

    [Header("Script Settings")]
    public Animator transitionSlide;
    public CinemachineVirtualCamera topdownCamera; 
    public CinemachineVirtualCamera playerCamera;
    private AudioListener mainAudioListener;
    private CinemachineTransposer cameraOffset;
    private bool isSetupActive = true;

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
        /*  Gets the components  */
        levelUI = this.gameObject.GetComponent<LevelUI>();

        SetLevel();
    }
    /*-  Retrieves values from level package and sets variables  -*/
    private void SetLevel()
    {
        levelPlayerUnits = level.availbleUnits.statsLists;
        levelUnitLimit = level.unitLimit;
        levelEnemyUnits = level.enemyUnits.statsLists;
        clockCalmTime = level.enemyCalmTime;
        clockEnragedTime = level.enemyEnragedTime;
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;

        cameraOffset = playerCamera.GetCinemachineComponent<CinemachineTransposer>();
        GameObject cam = Camera.main.gameObject;
        mainAudioListener = cam.GetComponent<AudioListener>();
        mainAudioListener.enabled = true;
        transitionSlide.SetTrigger("Start");

        /* Sets game to setup */
        gameManager.SetGameState(GameStates.SETUP);
        levelUI.UpdateScreen(0);
        SwitchCameras(1, 0);
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        /* Pauses game  */

        //If player press escape
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //if gameStates is PLAYING
            if(gameManager.CheckIfPlaying() || gameManager.GetGameState() == GameStates.SETUP)
            {
                LevelPaused();
            }
            else if(gameManager.GetGameState() == GameStates.PAUSED)             //if gameStates is PAUSED
            {
                LevelUnpause();
            }
        }

        /* Checks if the player is dead */
        
        //if the player avatar exists
        if(playerAvatar != null)
        {
            //if the player avatar isn't active, gameState is at playing and  hasPlayerRespawned is true
            if(playerAvatar.isActiveAndEnabled == false && gameManager.CheckIfPlaying() && hasPlayerRespawned == true)
            {
                mainAudioListener.enabled = true;
                hasPlayerRespawned = false;
                StartCoroutine(RespawnPlayer(respawnTime)); //Begins RespawnPlayer IEnumerator at respawnTime
                return;
            }
        }
    }

    /*---      FUNCTIONS     ---*/
    /*--    LEVEL STATE CHANGE MANAGEMENT   --*/
    /*-  Changes the level based game state based on what has happened in game -*/
    public void ChangeState()
    {
        //if the gameState is PLAYING
        if(gameManager.GetGameState() == GameStates.PLAYING)
        {
            /* Starts game */
            levelUI.UpdateScreen(1);
            levelUI.UpdateUI();
            SwitchCameras(0, 1);
            isSetupActive = false;
            SpawnPlayer();
            AdjustPlayerCamera();
            deckCreator.buildDeck();
            playerController.StartGame();
            enemySpawner.StartGame();
            StartCoroutine(EnragedCycle(1f)); //Begins EnragedCycle IEnumerator at 1 second
        }
        else if(gameManager.GetGameState() == GameStates.WIN) //if the gameState is WIN
        {
            /* Player WINS */
            mainAudioListener.enabled = true;
            gameManager.SetLastPlayedLevel();
            levelUI.UpdateScreen(4);
            SwitchCameras(1, 0);
        }
        else if(gameManager.GetGameState() == GameStates.LOSE) //if the gameState is LOSE
        {
            /* Player LOSES */
            mainAudioListener.enabled = true;
            levelUI.UpdateScreen(5);
            SwitchCameras(1, 0);
        }
    }
    /*-  Switch the each camera's priority, takes two ints for which cameras should be on or off  -*/
    private void SwitchCameras(int cam1, int cam2)
    {
        topdownCamera.Priority = cam1;
        playerCamera.Priority = cam2;
    }

    /*--    SETUP MANAGEMENT   --*/
    /*-  Adds or removes unit in the playerUnits list and checks if the player is ready or not  -*/
    public void AddOrRemoveUnit(int index)
    {
        //If playerUnits contains this unit
        if(playerUnits.Contains(levelPlayerUnits[index]))
        {
            playerUnitCount--;
            playerUnits.Remove(levelPlayerUnits[index]);
            levelUI.UpdateSetUpUI(); //Updates the UI list of units
        }
        else if(!playerUnits.Contains(levelPlayerUnits[index]) && playerUnitCount < levelUnitLimit) //If playerUnits doesn't contains this unit and playerUnitCount is less than the levelUnitLimit
        {
            playerUnitCount++;
            playerUnits.Add(levelPlayerUnits[index]);
            levelUI.UpdateSetUpUI();  //Updates the UI list of units
        }
        
        //if playerUnitCount is greater than or equal to levelUnitLimit
        if(playerUnitCount >= levelUnitLimit)
        {
            levelUI.startGameButton.SetActive(true); //Sets startGameButton active
        }
        else if(playerUnitCount < levelUnitLimit) //if playerUnitCount is less than levelUnitLimit
        {
            levelUI.startGameButton.SetActive(false); //Sets startGameButton inactive
        }
    }
    /*-  Starts Game, OnClick  -*/
    public void StartGame()
    {
        /* Sets game to playing */
        gameManager.SetGameState(GameStates.PLAYING);
        ChangeState();
    }

    /*--    PLAYER AVATAR MANAGEMENT   --*/
    /*-  Spawns the player avatar  -*/
    private void SpawnPlayer()
    {
        GameObject playerAvatarObj = Instantiate(playerPrefab, playerSpawner.transform.position, playerSpawner.transform.rotation); 
        playerAvatar = playerAvatarObj.GetComponent<PlayerAvatar>(); 
        playerCamera.Follow = playerAvatar.transform; //sets player camera's follow to the player's transform
        mainAudioListener.enabled = false;

    }
    /*-  respawns the player avatar, takes a float for the respawn time  -*/
    private IEnumerator RespawnPlayer(float waitTime)
    {
        SwitchCameras(1, 0);
        levelUI.UpdatePlayerDeath(false);
        yield return new WaitForSeconds(waitTime); //Waits for rate

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            playerAvatar.gameObject.SetActive(true);
            playerController.UpdateAura();
            playerAvatar.transform.position = playerSpawner.transform.position;
            SwitchCameras(0, 1);
            mainAudioListener.enabled = false;
            levelUI.UpdateUI();
            levelUI.UpdatePlayerDeath(true);
            hasPlayerRespawned = true;
        }
    }
    public void AdjustPlayerCamera()
    {
        cameraOffset.m_FollowOffset = new Vector3(0, gameManager.GetGameOptions().GetCameraZoomY(), gameManager.GetGameOptions().GetCameraZoom());
    }

    /*--    ENRAGED SYSTEM MANAGEMENT   --*/
    /*-  Repeatedly Updates the Enraged Cycle, takes a float for the time -*/
    private IEnumerator EnragedCycle(float time)
    {
        yield return new WaitForSeconds(time);

        //if gameStates is PLAYING
        if(gameManager.CheckIfPlaying())
        {
            clockTime++;
            levelUI.UpdateClock(); //Updates the UI's Enraged Clock

            //if clockTime is less than or equal clockCalmTime
            if(clockTime <= clockCalmTime)
            {
                /*  Calms Enemies  */
                isEnraged = false; 
            }
            else if(clockTime > clockCalmTime && clockTime <= level.GetTotalTime()) //if clockTime is greater than clockCalmTime and clockTime is less than or equal to clockTimeTotal 
            {
                /*  Enrages Enemies  */
                isEnraged = true;
                GlobalStatusEffect();
            }
            else if(clockTime > level.GetTotalTime()) //if clockTime is greater than clockTimeTotal
            {
                /*  Calms Enemies and resets clockTime  */
                isEnraged = false; 
                GlobalStatusEffect();
                clockTime = 0;
                enragedCount++;
            }
        }

        //if gameStates isn't WiN or LOSE
        if(!gameManager.CheckIfWinOrLose())
        {
            StartCoroutine(EnragedCycle(1f)); //Recalls EnragedCycle IEnumerator at 1 second
        }
    }
    private void GlobalStatusEffect()
    {
        foreach(Unit unit in Unit.GetUnitList())
        {            
            if(unit.IsEnemy() && unit.GetComponent<Ieffectable>() != null)
            {
                Ieffectable enemy = unit.GetComponent<Ieffectable>();

                if(isEnraged)
                {
                    enemy.ApplyEffect(enragedStatus);
                }
                else if(!isEnraged)
                {
                    enemy.RemoveEffect(enragedStatus);
                }
            }
        }
    }

    /*--    LEVEL BUTTONS MANAGEMENT   --*/
    /*-  Pauses Game -*/
    public void LevelPaused()
    {
        SwitchCameras(1, 0);
        levelUI.UpdateScreen(2);
        gameManager.gameState = GameStates.PAUSED;
    }
    /*-  Unpauses Game, OnClick -*/
    public void LevelUnpause()
    {
        AdjustPlayerCamera();
        if(playerAvatar != null && playerAvatar.isActiveAndEnabled == true)
        {
            playerAvatar.SetAttackRange();
        }

        if(isSetupActive)
        {
            levelUI.UpdateScreen(0);
            gameManager.gameState = GameStates.SETUP;
        }
        else if(!isSetupActive)
        {
            SwitchCameras(0, 1);
            levelUI.UpdateScreen(1);
            gameManager.gameState = GameStates.PLAYING;
        }
    }
    /*-  Opens options menu, OnClick   -*/
    public void Options()
    {
        levelUI.UpdateScreen(3);
        levelUI.UpdateSliders();
    }
    /*-  Calls GameManager RetryLevel, OnClick -*/
    public void LevelRetry()
    {
        transitionSlide.SetTrigger("End");
        gameManager.RetryLevel();
    }
    /*-  Calls GameManager QuitLevel, OnClick -*/
    public void LevelQuit()
    {
        transitionSlide.SetTrigger("End");
        gameManager.QuitLevel();
    }
    /*-  Calls GameManager NextLevel, OnClick -*/
    public void LevelComplete()
    {
        transitionSlide.SetTrigger("End");
        gameManager.NextLevel();
    }

    /*---      SET/GET FUNCTIONS     ---*/
    /*-  Gets level variables -*/
    public Level GetLevel()
    {
        return level;
    }
    /*-  Gets levelPlayerUnits -*/
    public List<Stats> GetLevelPlayerUnits()
    {
        return levelPlayerUnits;
    }
    /*-  Gets levelEnemyUnits -*/
    public List<Stats> GetEnemyUnits()
    {
        return levelEnemyUnits;
    }
    /*-  Gets playerUnits -*/
    public List<Stats> GetPlayerUnits()
    {
        return playerUnits;
    }
    /*-  Gets playerUnitCount -*/
    public int GetPlayerUnitsCount()
    {
        return playerUnitCount;
    }
    /*-  Gets PlayerAvatar -*/
    public PlayerAvatar GetPlayerAvatar()
    {
        return playerAvatar;
    }
    /*-  Gets enragedStatus -*/
    public StatusEffect GetEnragedStatus()
    {
        return enragedStatus;
    }
    /*-  Gets IsEnraged -*/
    public bool GetIsEnraged()
    {
        return isEnraged;
    }
}