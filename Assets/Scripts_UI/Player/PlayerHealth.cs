using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; set; }
    private bool dead = false;


    [Header("Invincibility Frame")]
    [SerializeField] private float invincibilityDuration;
    private float invincibilityCounter;

    [SerializeField] private float flashDuration;
    private float flashCounter;
    private MeshRenderer mr;

    [Header("GameOver Screen")]
    [SerializeField] private GameObject gameoverScreen;


    void Awake()
    {
        mr = GetComponentInChildren<MeshRenderer>();
        currentHealth = startingHealth;
    }

    public void TakeDamage(float damage)
    {
        if(invincibilityCounter <= 0)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

            if (currentHealth > 0)
            {
                //IFrame
                invincibilityCounter = invincibilityDuration;
            }
            else
            {
                if (!dead)
                {
                    gameObject.SetActive(false);
                    dead = true;

                    // Game Over UI
                    if (gameoverScreen != null)
                    {
                        gameoverScreen.SetActive(true);
                    }
                }
            }
        }

        UIController.instance.UpdateHealth(currentHealth, startingHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Invunerability();
    }

    private void Invunerability()
    {
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;

            if (flashCounter <= 0)
            {
                mr.enabled = !mr.enabled;
                flashCounter = flashDuration;
            }

            // Make Player Sprite Appears at the End of Iframes
            if (invincibilityCounter <= 0)
            {
                mr.enabled = true;
                flashCounter = 0f;
            }
        }
    }

    public void HealPlayer(float amountHealth)
    {
        currentHealth = Mathf.Clamp(currentHealth + amountHealth, 0, startingHealth);
        UIController.instance.UpdateHealth(currentHealth, startingHealth);

    }

}
