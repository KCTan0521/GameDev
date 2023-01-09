using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    private const float MAX_STAMINA = 3;
    private const float STAMINA_REGEN = 1f;
    public float stamina;
    private Image staminaBar;

    private void Awake()
    {
        staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
    }

    private void Update()
    {
        Rest();
        StaminaAnimation();
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
        staminaBar.fillAmount = stamina / MAX_STAMINA;
    }
}
