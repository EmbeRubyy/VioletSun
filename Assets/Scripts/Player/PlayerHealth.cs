using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private GameOverScreen gameOverScreen; // Cache the GameOverScreen reference

    [Header("Health")]
    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Damage Overlay")]
    public Image overlay; 
    public float duration;
    public float fadeSpeed;

    private float durationTimer;

    void Start()
    {
        health = maxHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        gameOverScreen = FindObjectOfType<GameOverScreen>(); // Find and cache the GameOverScreen reference
        Debug.Log("Player Health initialized. Max Health: " + maxHealth + ", Current Health: " + health);
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        Debug.Log("Current Health: " + health);  // Log current health

        // Check if health reaches zero or below
        if (health <= 0)
        {
            Debug.Log("Health reached zero! Triggering Game Over...");  // Debug message
            TriggerGameOver();
        }

        // Temporary input to test damage application
       /// if (Input.GetKeyDown(KeyCode.D))  // Press 'D' to take damage for testing
        ///{
        ///    TakeDamage(10f);  // Adjust the damage value as necessary
        ///}
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete * percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete * percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Damage Taken: " + damage + ", New Health: " + health);  // Log damage info

        // Check if health is below 0 after taking damage
        if (health <= 0)
        {
            Debug.Log("Health is now 0 or less after taking damage.");
        }

        lerpTimer = 0f;
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    private void TriggerGameOver()
    {
        if (gameOverScreen != null) // Check if the reference is valid
        {
            Debug.Log("Triggering Game Over!");  // Log when game over is triggered
            gameOverScreen.ShowGameOver();  // Call the show method
        }
        else
        {
            Debug.LogWarning("GameOverScreen reference not found!"); // Log a warning if not found
        }
    }
}
