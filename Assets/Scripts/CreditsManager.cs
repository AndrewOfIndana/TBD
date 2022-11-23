using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    /*  
        Name: CreditsManager.cs
        Description: This script controls the credits
        
    */
    /*[Header("Static References")]*/
    GameManager gameManager;

    public GameObject[] gameScreens;
    public RectTransform credits;
    public float yPos;
    public float yMax;
    public bool isCreditsMoving = false;
    public float scrollSpeed = .25f;

    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;

        yPos = credits.localPosition.y;
        yMax = yPos * -1;
        UpdateScreen(0);

        Invoke("StartCredits", 5f);
    }
    private void StartCredits()
    {
        isCreditsMoving = true;
        gameManager.SetGameState(GameStates.PLAYING);
        Debug.Log("Start");
    }

    /*---      UPDATE FUNCTIONS     ---*/
    /*-  Update is called once per frame -*/
    private void Update()
    {
        //If player press escape
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //if gameStates is PLAYING
            if(gameManager.CheckIfPlaying())
            {
                CreditsPause();
            }
            //if gameStates is PAUSED
            else if(gameManager.GetGameState() == GameStates.PAUSED)             
            {
                CreditsUnpause();
            }
        }

        //if gameStates is not PLAYING
        if(!gameManager.CheckIfPlaying())
        {
            return;
        }

        if(yPos < yMax && isCreditsMoving)
        {
            credits.localPosition = new Vector2(0f, yPos); 
            yPos += scrollSpeed;
        }
        else if(yPos >= yMax && isCreditsMoving)
        {
            isCreditsMoving = false;
            CreditQuit();
        }
    }

    /*---      FUNCTIONS     ---*/
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
    /*-  Pauses Credits -*/
    public void CreditsPause()
    {
        UpdateScreen(1);
        gameManager.SetGameState(GameStates.PAUSED);
    }
    /*-  Unpauses Credits, OnClick -*/
    public void CreditsUnpause()
    {
        UpdateScreen(0);
        gameManager.SetGameState(GameStates.PLAYING);
    }
    /*-  Calls GameManager QuitLevel, OnClick -*/
    public void CreditQuit()
    {
        gameManager.QuitLevel();
    }
}
