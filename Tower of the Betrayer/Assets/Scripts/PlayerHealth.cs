// Authors: Jeff Cui, Elaine Zhao

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Manages the player's health and computes damage taken.
public class PlayerHealth: MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public UnityEvent onDeath;
    public HealthBar healthBar;

    private void Start()
    {
        if (PlayerStats.Instance == null)
        {
            Debug.LogError("PlayerStats not found!");
            return;
        }

        // Get max health from PlayerStats
        maxHealth = PlayerStats.Instance.MaxHealth;
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogWarning("HealthBar reference not set!");
        }
    }

    void Update()
    {
        // Update maxHealth from PlayerStats if it changed
        if (PlayerStats.Instance != null && maxHealth != PlayerStats.Instance.MaxHealth)
        {
            float oldMaxHealth = maxHealth;
            maxHealth = PlayerStats.Instance.MaxHealth;
            
            // Adjust current health proportionally
            currentHealth = (currentHealth / oldMaxHealth) * maxHealth;
            
            if (healthBar != null)
            {
                healthBar.SetMaxHealth(maxHealth);
                healthBar.SetHealth(currentHealth);
            }
        }

        if (currentHealth <= 0 && gameObject.activeSelf) 
        {
            print("PlayerHealth: " + currentHealth);
            print("Player died");
            onDeath.Invoke();
            gameObject.SetActive(false); // Disable instead of destroy to allow WavesGameMode to handle death
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        print("PlayerHealth: " + currentHealth);
    }
    
    public void RestoreHealth(float amount)
    {
        // Add health but don't exceed max health
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        
        // Update health bar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        
        Debug.Log($"Health restored: {amount}. Current health: {currentHealth}/{maxHealth}");
    }
}