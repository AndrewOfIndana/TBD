using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates {MENU, SETUP, PLAYING, PAUSED, WIN, LOSE} //The different game states the level could be in

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;

    public int currentLevel = 0;

    private string menuScene = "MenuScene";
    private string levelScene = "Level";

    public GameStates gameState;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Awake is called when the script is being loaded -*/
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (gameInstance != null && gameInstance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            gameInstance = this; 
        } 
    }
    private void Start()
    {
        // gameState = GameStates.MENU;
    }

    public void SelectLevel(int btnIndex)
    {
        currentLevel = btnIndex;
        string levelNum = currentLevel.ToString("D2");
        SceneManager.LoadScene(levelScene + levelNum);
    }
    public void UpdateLevel()
    {
        currentLevel++;
        string levelNum = currentLevel.ToString("D2");
        Debug.Log(levelScene + levelNum);
        SceneManager.LoadScene(levelScene + levelNum);
    }
    public void RetryLevel()
    {
        string levelNum = currentLevel.ToString("D2");
        Debug.Log(levelScene + levelNum);
        SceneManager.LoadScene(levelScene + levelNum);
    }
    public void QuitLevel()
    {
        // gameState = GameStates.MENU;
        SceneManager.LoadScene(menuScene);
    }

    /*---      SET/GET FUNCTIONS     ---*/
    public void SetGameState(GameStates newStates)
    {
        gameState = newStates;
    }
    public GameStates GetGameState()
    {
        return gameState;
    }
}
