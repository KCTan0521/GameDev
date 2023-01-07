using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private const float MAX_HEALTH = 3;
    private const float HEALTH_REGEN = .5f;
    private const float REGEN_TIME = 5;
    private float health;
    private float timePassed;

    public void Update()
    {
        HealthRegen();
    }

    public Health()
    {
        health = MAX_HEALTH;
    }

    public void Damage(float damage)
    {
        health -= damage;
        Debug.Log(health);
    }

    public void HealthRegen()
    {
        if (health <= 0)
        {
            Debug.Log("Game Over");
        }

        else if (health < MAX_HEALTH)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= REGEN_TIME)
            {
                health += HEALTH_REGEN;
                Debug.Log(health);
                timePassed = 0;
            }
        }

        
    }
}
