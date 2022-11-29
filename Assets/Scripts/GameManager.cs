using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates {MENU, SETUP, PLAYING, PAUSED, WIN, LOSE} //The different game states the game could be in

public class GameManager : MonoBehaviour
{
    /*  
        Name: GameManager.cs
        Description: This script handles all variables and states for a the whole game

    */
    public static GameManager instance;

    [Header("Script Settings")]
    public GameStates gameState;
    public int currentLevel = 0;
    public int lastPlayedLevel = 1;
    private float transitionTime = 1f;
    private bool firstTimePlaying = true;

    private GameOptions gameOptions;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        /* SINGLETON PATTERN */
        //if the instance does exist and the instance isn't this
        if (instance != null && instance != this) 
        { 
            Destroy(this.gameObject);  
        } 
        else 
        { 
            instance = this; 
        } 

        gameOptions = this.gameObject.GetComponent<GameOptions>();
    }
    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        gameState = GameStates.MENU;
    }

    /*---      MAIN FUNCTIONS     ---*/
    /*-  Quits the main game -*/
    public void QuitGame()
    {
        Application.Quit();
    }

    /*---      LEVEL FUNCTIONS     ---*/
    /*-  Selects a level, uses an index to indicate which level -*/
    public void SelectLevel(int btnIndex)
    {
        currentLevel = btnIndex;
        string levelNum = currentLevel.ToString("D2"); //Converts level number to string format 00

        if(firstTimePlaying)
        {
            firstTimePlaying = false;
            StartCoroutine(LoadLevel("HowToPlay"));
        }
        else
        {
            StartCoroutine(LoadLevel("Level_" + levelNum));
        }
    }
    /*-  Chooses the next level -*/
    public void NextLevel()
    {
        currentLevel++;
        string levelNum = currentLevel.ToString("D2"); //Converts level number to string format 00
        StartCoroutine(LoadLevel("Level_" + levelNum));
    }
    /*-  Retries the current level -*/
    public void RetryLevel()
    {
        string levelNum = currentLevel.ToString("D2"); //Converts level number to string format 00
        StartCoroutine(LoadLevel("Level_" + levelNum));
    }
    /*-  Returns to the menuScene -*/
    public void QuitLevel()
    {
        gameState = GameStates.MENU;
        StartCoroutine(LoadLevel("Menu"));
    }
    private IEnumerator LoadLevel(string sceneName)
    {
        yield return new WaitForSeconds(transitionTime);
        if(currentLevel == 12)
        {
            gameOptions.SetColorFilter(new Color(1, 1, 1));
        }
        SceneManager.LoadSceneAsync(sceneName);
    }

    /*---      SET/GET FUNCTIONS     ---*/
    public GameOptions GetGameOptions()
    {
        return gameOptions;
    }
    /*-  Sets the gameState outside of GameManager -*/
    public void SetGameState(GameStates newStates)
    {
        gameState = newStates;
    }
    /*-  Gets the gameState -*/
    public GameStates GetGameState()
    {
        return gameState;
    }
    /*-  Sets lastPlayedLevel -*/
    public void SetLastPlayedLevel()
    {
        int completeLevel = currentLevel + 1;

        //if lastPlayedLevel is less than completeLevel
        if(lastPlayedLevel < completeLevel)
        {
            lastPlayedLevel = completeLevel;
        }
    }
    /*-  Gets lastPlayedLevel -*/
    public int GetLastPlayedLevel()
    {
        return lastPlayedLevel;
    }

    public bool CheckIfWinOrLose()
    {
        if(gameState == GameStates.WIN 
        || gameState == GameStates.LOSE)
        {
            return true;
        }
        return false;
    }
    public bool CheckIfPlaying()
    {
        if(gameState == GameStates.PLAYING)
        {
            return true;
        }
        return false;
    }
}
