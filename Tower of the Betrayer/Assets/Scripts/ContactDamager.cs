// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

// Applies damage to any PlayerHealth or EnemyHealth component on objects that collide with this object.
public class ContactDamager : MonoBehaviour
{
    public float damage; // Amount of damage to apply on contact.
    private bool isPlayerBullet; // Whether this is a player's bullet
    
    private void Start()
    {
        // Check if this bullet is on the PlayerBullet layer
        isPlayerBullet = gameObject.layer == LayerMask.NameToLayer("PlayerBullet");
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Contact with: {other.name}");
        // If this is a player bullet and it hit the player, ignore the collision
        if (isPlayerBullet && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        // Scale player damage if applicable
        if (isPlayerBullet && FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyPlayerDamage(ref damage);
        }
        // Scale enemy damage if applicable
        else if (!isPlayerBullet && FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyEnemyDamage(ref damage);
        }
        
        // Apply damage to enemy
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }
        
        // Apply damage to player - SIMPLIFIED DIRECT APPROACH
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            // Get current health for verification
            float healthBefore = playerHealth.currentHealth;
            
            // Direct forced damage application - bypass any potential issues
            playerHealth.currentHealth -= damage;
            playerHealth.currentHealth = Mathf.Max(playerHealth.currentHealth, 0);
            Debug.Log($"Player took {damage} damage. Health: {playerHealth.currentHealth}");
            
            // Update health bar
            if (playerHealth.healthBar != null)
            {
                playerHealth.healthBar.SetHealth(playerHealth.currentHealth);
            }
        }
        
        Destroy(gameObject);
    }
}
