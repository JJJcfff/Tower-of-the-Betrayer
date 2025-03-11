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
    // public Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        // animator.SetInteger("Health", 100);
    }

    void Update()
    {
        if (!(currentHealth <= 0)) return;
        // animator.SetInteger("Health", 0);
        //wait 1 second before destroying the object
        // Destroy(gameObject, 1);
        Destroy(gameObject);
    }
    
    // Method to apply damage to the enemy
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
