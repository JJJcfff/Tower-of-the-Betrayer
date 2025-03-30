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
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    void Update()
    {
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
        healthBar.SetHealth(currentHealth);
        print("PlayerHealth: " + currentHealth);
    }
}