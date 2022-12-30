using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    private const float MAX_STAMINA = 3;
    private const float STAMINA_REGEN = .5f;
    private const float REGEN_TIME = .5f;
    public float stamina;
    private float timePassed;

    private void Update()
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
        Debug.Log(energy + "J exhausted");
    }

    public void Rest()
    {
        if (stamina < MAX_STAMINA)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= REGEN_TIME)
            {
                stamina += STAMINA_REGEN;
                Debug.Log(stamina);
                timePassed = 0;
            }
        }
    }
}
