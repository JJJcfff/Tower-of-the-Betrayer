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

        // Destroy bullet if it hits a wall
        if (other.CompareTag("Wall"))
        {
            Debug.Log("💥 Bullet hit a wall and is destroyed");
            Destroy(gameObject);
            return;
        }

        // Prevent self-damage to player
        if (isPlayerBullet && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }

        // Modify damage if difficulty manager exists
        if (isPlayerBullet && FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyPlayerDamage(ref damage);
        }
        else if (!isPlayerBullet && FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyEnemyDamage(ref damage);
        }

        // Apply damage to enemies
        if (other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }

        // Apply damage to player
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);

            // Camera shake
            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.ShakeCamera(0.2f, 0.25f);
            }

            AudioClip hitSound = Resources.Load<AudioClip>("playerTakingDamage");
            if (hitSound != null)
            {
                AudioSource audio = other.GetComponent<AudioSource>();
                if (audio == null)
                {
                    audio = other.gameObject.AddComponent<AudioSource>();
                    audio.spatialBlend = 0f;
                }
                audio.PlayOneShot(hitSound);
            }
            
        }

        // Destroy bullet after any hit
        Destroy(gameObject);
    }
}
