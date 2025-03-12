// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // If this is a player bullet and it hit the player, ignore the collision
        if (isPlayerBullet && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }
        
        // Apply damage and destroy the bullet
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
        
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }
        
        Destroy(gameObject);
    }
}
