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

        // Ignore self-damage to player
        if (isPlayerBullet && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        // Apply damage modifiers
        if (isPlayerBullet && FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyPlayerDamage(ref damage);
        }
        else if (!isPlayerBullet && FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyEnemyDamage(ref damage);
        }

        // Apply damage to enemy
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }

        // Apply damage to player and play sound
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            // Deal damage manually
            playerHealth.currentHealth -= damage;
            playerHealth.currentHealth = Mathf.Max(playerHealth.currentHealth, 0);
            Debug.Log($"Player took {damage} damage. Health: {playerHealth.currentHealth}");

            if (playerHealth.healthBar != null)
            {
                playerHealth.healthBar.SetHealth(playerHealth.currentHealth);
            }

            // ✅ Play sound manually
            AudioClip hitSound = Resources.Load<AudioClip>("playerTakingDamage");
            if (hitSound != null)
            {
                AudioSource audio = other.GetComponent<AudioSource>();
                if (audio == null)
                {
                    audio = other.gameObject.AddComponent<AudioSource>();
                    audio.spatialBlend = 0f; // 2D
                }
                audio.PlayOneShot(hitSound);
            }
            else
            {
                Debug.LogWarning("❌ Could not load playerTakingDamage.mp3 from Resources!");
            }
        }

        Destroy(gameObject);
    }
}
