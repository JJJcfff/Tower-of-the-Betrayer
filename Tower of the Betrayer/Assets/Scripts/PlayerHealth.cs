// Authors: Jeff Cui, Elaine Zhao

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Inventory;

// Manages the player's health and computes damage taken.
public class PlayerHealth: MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;
    public UnityEvent onDeath;
    public HealthBar healthBar;
    
    [Header("Damage Feedback")]
    public float damageFlashDuration = 0.2f; // Duration of the damage flash effect
    public Color damageFlashColor = Color.red; // Color to flash when taking damage
    public float damagePulseScale = 1.05f; // How much to scale up when taking damage
    
    private Material[] originalMaterials;
    private Material[] damageMaterials;
    private bool isFlashing = false;
    private Renderer[] playerRenderers;
    private Vector3 originalScale;
    private Transform playerTransform;

    private void Start()
    {
        if (PlayerStats.Instance == null)
        {
            Debug.LogError("PlayerHealth: PlayerStats not found!");
            return;
        }

        // Get max health from PlayerStats
        maxHealth = PlayerStats.Instance.MaxHealth;
        currentHealth = maxHealth;

        // Store the original scale
        playerTransform = transform;
        originalScale = playerTransform.localScale;
        
        // Get ALL renderers in this object and its children
        playerRenderers = GetComponentsInChildren<Renderer>();
        if (playerRenderers.Length == 0)
        {
            Debug.LogWarning("No renderers found on player or its children!");
        }
        else
        {
            // Store original materials from all renderers
            List<Material> originalMaterialsList = new List<Material>();
            foreach (Renderer renderer in playerRenderers)
            {
                originalMaterialsList.AddRange(renderer.materials);
            }
            originalMaterials = originalMaterialsList.ToArray();
            
            // Create damage materials (red versions of original materials)
            damageMaterials = new Material[originalMaterials.Length];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                damageMaterials[i] = new Material(originalMaterials[i]);
                damageMaterials[i].color = damageFlashColor;
                damageMaterials[i].EnableKeyword("_EMISSION");
                damageMaterials[i].SetColor("_EmissionColor", damageFlashColor * 2f);
            }
        }

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogWarning("PlayerHealth: HealthBar reference not set!");
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
            Debug.Log("PlayerHealth: Player died");
            onDeath.Invoke();
            gameObject.SetActive(false); // Disable instead of destroy to allow WavesGameMode to handle death
        }
    }
    
    public void TakeDamage(float damage)
    {
        // Apply damage
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        
        // Update health bar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        
        // Show damage feedback if not already flashing
        if (!isFlashing && damage > 0)
        {
            StartCoroutine(DamageFlashEffect());
        }
        
        // Check for death
        if (currentHealth <= 0 && gameObject.activeSelf)
        {
            onDeath.Invoke();
            gameObject.SetActive(false);
        }
    }
    
    private IEnumerator DamageFlashEffect()
    {
        if (playerRenderers == null || playerRenderers.Length == 0 || damageMaterials == null)
        {
            yield break;
        }
        
        isFlashing = true;
        
        // Scale up slightly
        playerTransform.localScale = originalScale * damagePulseScale;
        
        // Apply damage materials to all renderers
        int materialIndex = 0;
        foreach (Renderer renderer in playerRenderers)
        {
            Material[] currentDamageMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                currentDamageMaterials[i] = damageMaterials[materialIndex++];
            }
            renderer.materials = currentDamageMaterials;
        }
        
        // Wait for flash duration
        yield return new WaitForSeconds(damageFlashDuration);
        
        // Restore original scale
        playerTransform.localScale = originalScale;
        
        // Restore original materials to all renderers
        materialIndex = 0;
        foreach (Renderer renderer in playerRenderers)
        {
            Material[] currentOriginalMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                currentOriginalMaterials[i] = originalMaterials[materialIndex++];
            }
            renderer.materials = currentOriginalMaterials;
        }
        
        isFlashing = false;
    }
    
    public void RestoreHealth(float amount)
    {
        float previousHealth = currentHealth;
        
        // Add health but don't exceed max health
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        
        // Update health bar
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        
        float actualHeal = currentHealth - previousHealth;
        Debug.Log($"PlayerHealth: Health restored by {actualHeal} (of {amount} attempted). Current health: {currentHealth}/{maxHealth}");
    }
    
    private void OnDestroy()
    {
        // Clean up damage materials
        if (damageMaterials != null)
        {
            foreach (Material mat in damageMaterials)
            {
                if (mat != null)
                {
                    Destroy(mat);
                }
            }
        }
    }
}