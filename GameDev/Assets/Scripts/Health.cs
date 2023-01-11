using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private const float MAX_HEALTH = 3;
    private const float HEALTH_REGEN = .5f;
    private const float REGEN_TIME = 5;
    public float health;
    public Animator animator;
    private float timePassed;

    private void Update()
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
    }

    public void HealthRegen()
    {
        if (health <= 0)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene("GameOver");
        }

        else if (health < MAX_HEALTH)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= REGEN_TIME)
            {
                health += HEALTH_REGEN;
                timePassed = 0;
            }
        }
    }

    public void PalpitatingAnimation()
    {
        if (health == MAX_HEALTH)
        {
            animator.SetBool("isHealing", false);
        }
        else
        {
            animator.SetBool("isHealing", true);
        }

        switch (health)
        {
            case 3f:
                GameObject.Find("Heart1").GetComponent<Image>().sprite = fullHeart;
                GameObject.Find("Heart2").GetComponent<Image>().sprite = fullHeart;
                GameObject.Find("Heart3").GetComponent<Image>().sprite = fullHeart;
                break;
            case 2.5f:
                GameObject.Find("Heart1").GetComponent<Image>().sprite = fullHeart;
                GameObject.Find("Heart2").GetComponent<Image>().sprite = fullHeart;
                GameObject.Find("Heart3").GetComponent<Image>().sprite = halfHeart;
                break;
            case 2f:
                GameObject.Find("Heart1").GetComponent<Image>().sprite = fullHeart;
                GameObject.Find("Heart2").GetComponent<Image>().sprite = fullHeart;
                GameObject.Find("Heart3").GetComponent<Image>().sprite = emptyHeart;
                break;
            case 1.5f:
                GameObject.Find("Heart1").GetComponent<Image>().sprite = fullHeart;
                GameObject.Find("Heart2").GetComponent<Image>().sprite = halfHeart;
                GameObject.Find("Heart3").GetComponent<Image>().sprite = emptyHeart;
                break;
            case 1f:
                GameObject.Find("Heart1").GetComponent<Image>().sprite = fullHeart;
                GameObject.Find("Heart2").GetComponent<Image>().sprite = emptyHeart;
                GameObject.Find("Heart3").GetComponent<Image>().sprite = emptyHeart;
                break;
            case 0.5f:
                GameObject.Find("Heart1").GetComponent<Image>().sprite = halfHeart;
                GameObject.Find("Heart2").GetComponent<Image>().sprite = emptyHeart;
                GameObject.Find("Heart3").GetComponent<Image>().sprite = emptyHeart;
                break;
            case 0f:
                GameObject.Find("Heart1").GetComponent<Image>().sprite = emptyHeart;
                GameObject.Find("Heart2").GetComponent<Image>().sprite = emptyHeart;
                GameObject.Find("Heart3").GetComponent<Image>().sprite = emptyHeart;
                break;
        }
    }
}
