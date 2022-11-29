using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    /*  
        Name: CreditsManager.cs
        Description: This script controls the credits
        
    */
    /*[Header("Static References")]*/
    GameManager gameManager;

    public Animator transitionSlide;
    public GameObject[] gameScreens;
    public GameObject[] tutorialSlides;
    public int tutorialIndex = 0;

    /*-  Start is called before the first frame update -*/
    private void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        UpdateScreen(0);
        UpdateSlide(tutorialIndex);

        transitionSlide.SetTrigger("Start");
        gameManager.SetGameState(GameStates.PLAYING);
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
                HowToPlayPause();
            }
            //if gameStates is PAUSED
            else if(gameManager.GetGameState() == GameStates.PAUSED)             
            {
                HowToPlayUnpause();
            }
        }
    }

    /*---      FUNCTIONS     ---*/
    /*-  Moves slide forward, OnClick -*/
    public void HowToPlayForward()
    {
        if(tutorialIndex < tutorialSlides.Length)
        {
            tutorialIndex++;
            if(tutorialIndex >= tutorialSlides.Length)
            {
                HowToPlayPlay();
            }
            else
            {
                UpdateSlide(tutorialIndex);
            }
        }
    }
    /*-  Moves slide back, OnClick -*/
    public void HowToPlayBack()
    {
        if(tutorialIndex > 0)
        {
            tutorialIndex--;
            UpdateSlide(tutorialIndex);
        }
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
    /*-  Updates the Swaps the slide, takes an index for chosen screen -*/
    public void UpdateSlide(int index)
    {
        //Sets all game screens to false
        for(int i = 0; i < tutorialSlides.Length; i++)
        {
            tutorialSlides[i].SetActive(false);
        }
        tutorialSlides[index].SetActive(true);
    }
    /*-  Pauses Credits -*/
    public void HowToPlayPause()
    {
        UpdateScreen(1);
        gameManager.SetGameState(GameStates.PAUSED);
    }
    /*-  Unpauses Credits, OnClick -*/
    public void HowToPlayUnpause()
    {
        UpdateScreen(0);
        gameManager.SetGameState(GameStates.PLAYING);
    }
    /*-  Calls GameManager SelectLevel, OnClick -*/
    public void HowToPlayPlay()
    {
        transitionSlide.SetTrigger("End");
        gameManager.SelectLevel(1);
    }
    /*-  Calls GameManager QuitLevel, OnClick -*/
    public void HowToPlayQuit()
    {
        transitionSlide.SetTrigger("End");
        gameManager.QuitLevel();
    }
}
