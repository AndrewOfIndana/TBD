using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStartManager : MenuManager
{
    public GameObject startingScreen;
    public CanvasGroup blankSlide;
    public CanvasGroup companySlide;
    public AudioSource audioSource;

    /*---      SETUP FUNCTIONS     ---*/
    /*-  Start is called before the first frame update -*/
    protected override void Start()
    {
        /* Gets the static instances and stores them in the Static References */
        gameManager = GameManager.instance;
        startingScreen.SetActive(true);
        blankSlide.gameObject.SetActive(true);
        companySlide.gameObject.SetActive(false);
        StartCoroutine(OpenMenu(2f));
    }
    private IEnumerator OpenMenu(float time)
    {
        companySlide.gameObject.SetActive(true);
        blankSlide.interactable = false;
        blankSlide.alpha = 1;
        companySlide.interactable = false;
        companySlide.alpha = 0;
        yield return new WaitForSeconds(time); //Waits for rate
        StartCoroutine(FadeScreen());

    }

    private IEnumerator FadeScreen()
    {
        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            companySlide.alpha = i;
            yield return null;
        }
 
        //Temp to Fade Out
        yield return new WaitForSeconds(1);

        // loop over 1 second backwards
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            companySlide.alpha = i;
            yield return null;
        }
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            blankSlide.alpha = i;
            yield return null;
        }
        blankSlide.gameObject.SetActive(false);
        StartMenu();
    }

    private void StartMenu()
    {
        startingScreen.SetActive(false);
        UpdateScreen(0);
        audioSource.Play();

        /* Adds listeners for each buttons for each level */
        for(int i = 0; i < levelIcons.Length; i++)
        {
            AddListeners(levelIcons[i].GetComponent<Button>(), i);
        }
    
        /*  Hides the level icons that haven't been unlocked  */
        HideUI(levelIcons, gameManager.GetLastPlayedLevel()); 
        transitionSlide.SetTrigger("Start");
    }
    private void HideScreen()
    {
        for(int i = 0; i < menuScreens.Length; i++)
        {
            menuScreens[i].SetActive(false);
        }
    }
}
