using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    private const float MAX_STAMINA = 3;
    private const float STAMINA_REGEN = 1.5f;
    public float stamina;
    private Image staminaBar;
    private bool isStrangled;
    private float struggleCount;

    private void Awake()
    {
        staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
    }

    private void Update()
    {
        StaminaAnimation();
        isStrangled = GameObject.Find("Player").GetComponent<PlayerBehaviour>().isStrangled;
        struggleCount = GameObject.Find("Player").GetComponent<PlayerBehaviour>().struggleCount;
    }

    private void FixedUpdate()
    {
        Rest();
    }

    public Stamina()
    {
        stamina = MAX_STAMINA;
    }

    public void Exhaust(float energy)
    {
        stamina -= energy;
    }

    public void Rest()
    {
        if (stamina < MAX_STAMINA)
        {
            stamina += STAMINA_REGEN * Time.fixedDeltaTime;
            
        }
    }

    private void StaminaAnimation()
    {
        if (isStrangled)
        {
            staminaBar.color = new Color32(62, 172, 180, 255);
            staminaBar.fillAmount = struggleCount / 10;
        }

        else
        {
            staminaBar.color = new Color32(140, 180, 62, 255);
            staminaBar.fillAmount = stamina / MAX_STAMINA;
        }
    }
}
