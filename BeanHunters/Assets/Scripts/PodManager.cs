using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PodManager : MonoBehaviour
{
    public GameObject leave;
    public GameObject nextDay;
    public GameObject die;
    public TextMeshProUGUI deathReason;

    InventoryManager invManager;

    void Start()
    {
        // load stuff
        invManager = FindObjectOfType<InventoryManager>();

        // check can leave?
        if(invManager.getCount("Components") >= 75
            && invManager.getCount("Fuel") >= 50
            && invManager.getCount("Scrap") >= 75)
        {
            leave.GetComponent<Button>().interactable = true;
        }
        else
        {
            leave.GetComponent<Button>().interactable = false;
        }


        nextDay.GetComponent<TextMeshProUGUI>().SetText("Start day " + (StormScript.timesLoaded + 1));
        if(StormScript.timesLoaded > 5 || invManager.getCount("Ration") <= 0)
        {
            deathReason.enabled = true;
            if (StormScript.timesLoaded > 5)
            {
                deathReason.SetText("Unfortunately you didn't make in within 5 days");
            }
            else
            {
                deathReason.SetText("Unfortunately you starved because you didn't have any rations left");
            }
            nextDay.GetComponent<Button>().interactable = false;
            die.SetActive(true);
        }
        else
        {
            deathReason.enabled = false;
            nextDay.GetComponent<Button>().interactable = true;
            die.SetActive(false);
            invManager.removeItem("Ration", 1);
        }
    }

    public void StartNextDay()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void WinGame()
    {
        SceneManager.LoadScene("Victory");
    }

    public void Die()
    {
        SceneManager.LoadScene("GameOver");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
