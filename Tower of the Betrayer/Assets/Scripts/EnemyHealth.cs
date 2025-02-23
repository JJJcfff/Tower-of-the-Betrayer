// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Manages the enemy's health and computes damage taken.
public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!(currentHealth <= 0)) return;
        Destroy(gameObject);
    }
    
    // Method to apply damage to the enemy
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
