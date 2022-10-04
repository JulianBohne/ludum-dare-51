using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject introText;
    public GameObject controls;
    public GameObject tutorial;
    public GameObject nextButton;

    private TextMeshProUGUI nextText;

    enum State
    {
        INTRO,
        CONTROLS,
        TUTORIAL
    }

    private State currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentState = State.INTRO;
        mainMenu.SetActive(true);
        nextButton.SetActive(false);
        introText.SetActive(false);
        controls.SetActive(false);
        tutorial.SetActive(false);


        nextText = nextButton.GetComponent<TextMeshProUGUI>();
        nextText.SetText("Next");
    }

    public void OnStartClicked()
    {
        mainMenu.SetActive(false);
        nextButton.SetActive(true);
        introText.SetActive(true);
    }

    public void OnNextClicked()
    {
        switch (currentState)
        {
            case State.INTRO:
                introText.SetActive(false);
                controls.SetActive(true);
                currentState = State.CONTROLS;
                break;
            case State.CONTROLS:
                controls.SetActive(false);
                tutorial.SetActive(true);
                currentState = State.TUTORIAL;
                nextText.SetText("Begin Adventure");
                break;
            case State.TUTORIAL:
                StormScript.timesLoaded = 0;
                InventoryManager.resetInventory();
                SceneManager.LoadScene("GameScene");
                break;
        }
    }
}
