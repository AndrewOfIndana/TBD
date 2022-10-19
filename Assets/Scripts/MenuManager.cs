using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    GameManager gameManager;

    public GameObject[] menuScreens;
    public Button quitButton;
    public Button[] levelButtons;
    public Animator creditsRoll;

    private void Start()
    {
        gameManager = GameManager.gameInstance;

        UpdateScreen(0);
        quitButton.gameObject.SetActive(!(Application.platform == RuntimePlatform.WebGLPlayer));

        for(int i = 0; i < levelButtons.Length; i++)
        {
            AddListeners(levelButtons[i].GetComponent<Button>(), i);
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
        gameManager.SelectLevel((index + 1));
    }
    private void UpdateScreen(int index)
    {
        for(int i = 0; i < menuScreens.Length; i++)
        {
            menuScreens[i].SetActive(false);
        }
        menuScreens[index].SetActive(true);
    }
    public void LevelSelect()
    {
        UpdateScreen(1);
    }
    public void Options()
    {
        UpdateScreen(2);
    }
    public void Back()
    {
        UpdateScreen(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
