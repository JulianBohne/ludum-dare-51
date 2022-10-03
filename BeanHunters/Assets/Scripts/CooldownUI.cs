using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    private Image actionImage;

    public float cooldownTime;
    private float timeLeft;
    private bool isCooldown = false;
    private bool enabledUsage = true;
    private bool enabledCooldown = true;

    public bool tryToUse()
    {
        if (isCooldown || !enabledUsage)
        {
            return false;
        }
        else if(enabledCooldown)
        {
            isCooldown = true;
            actionImage.color = new Color32(255, 0, 0, 127);
            actionImage.transform.localScale = new Vector3(1,1,1);
            timeLeft = cooldownTime;
        }
        return true;
    }

    public void disableUsage()
    {
        enabledUsage = false;
    }

    public void disableCooldown()
    {
        enabledCooldown = false;
    }

    public void enableUsage()
    {
        enabledUsage = true;
    }

    public void enableCooldown()
    {
        enabledCooldown = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        actionImage = transform.Find("Overlay").GetComponent<Image>();
        actionImage.color = new Color32(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown && enabledUsage)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft < 0)
            {
                isCooldown = false;
                actionImage.color = new Color32(0, 0, 0, 0);
            }
            actionImage.transform.localScale = new Vector3(1, timeLeft/cooldownTime, 1);
        }
    }

    private Color32 getColor(float time)
    {
        return new Color32();
    }
}
