using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Healthbar : MonoBehaviour
{
    public float MaxHealth = 100;
    public float Health = 100;

    public Gradient healthColor;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        Health = MaxHealth;
    }


    // Update is called once per frame
    void Update()
    {
        if(Health <= 0.001f)
        {
            SceneManager.LoadScene("GameOver");
        }
        Health = Mathf.Clamp(Health, 0, MaxHealth);
        float relativeHealth = Health / MaxHealth;

        image.color = healthColor.Evaluate(relativeHealth);
        transform.localScale = new Vector3(relativeHealth, 1, 1);

    }
}
