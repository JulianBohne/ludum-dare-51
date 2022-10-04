using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BackToMainMenu : MonoBehaviour
{
    public bool GameOver = false;
    public TextMeshProUGUI gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        if (GameOver)
            gameOverText.SetText("You survived " + StormScript.timesLoaded + " day(s).\nUnfortunately you did not manage to either keep yourself alive or get a signal into space in time.\nNoone ever comes to find what remains of you or your escape pod.");
        else
            gameOverText.SetText("Congratulations, you made it in " + StormScript.timesLoaded + " day(s)!\nYou weathered the solar storm and managed to get a signal off world, and soon after a passing freighter responded andpicked you up.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
