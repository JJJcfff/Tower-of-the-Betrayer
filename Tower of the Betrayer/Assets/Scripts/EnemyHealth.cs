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
    public float damageFlashDuration = 0.2f; // Duration of the damage flash effect
    public Color damageFlashColor = Color.red; // Color to flash when taking damage
    public float damagePulseScale = 1.2f; // How much to scale up when taking damage
    
    private Material[] originalMaterials;
    private Material[] damageMaterials;
    private bool isFlashing = false;
    private Renderer[] enemyRenderers;
    private Vector3 originalScale;
    private Transform enemyTransform;

    private void Start()
    {
        // Apply floor difficulty scaling if available
        if (FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyEnemyHealth(this);
        }
        else
        {
            currentHealth = maxHealth;
        }
        
        // Store the original scale
        enemyTransform = transform;
        originalScale = enemyTransform.localScale;
        
        // Get ALL renderers in this object and its children
        enemyRenderers = GetComponentsInChildren<Renderer>();
        if (enemyRenderers.Length == 0)
        {
            Debug.LogWarning("No renderers found on enemy or its children!");
            return;
        }

        Debug.Log($"Found {enemyRenderers.Length} renderers on enemy");
        
        // Store original materials from all renderers
        List<Material> originalMaterialsList = new List<Material>();
        foreach (Renderer renderer in enemyRenderers)
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

    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    // Method to apply damage to the enemy
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.Log($"Enemy took {damage} damage. Health: {currentHealth}");
        
        // Show damage feedback if not already flashing
        if (!isFlashing)
        {
            StartCoroutine(DamageFlashEffect());
        }
    }
    
    private IEnumerator DamageFlashEffect()
    {
        if (enemyRenderers == null || enemyRenderers.Length == 0)
        {
            Debug.LogWarning("No renderers available for damage flash effect!");
            yield break;
        }
        
        isFlashing = true;
        Debug.Log("Starting damage flash effect");
        
        // Scale up
        enemyTransform.localScale = originalScale * damagePulseScale;
        
        // Apply damage materials to all renderers
        int materialIndex = 0;
        foreach (Renderer renderer in enemyRenderers)
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
        enemyTransform.localScale = originalScale;
        
        // Restore original materials to all renderers
        materialIndex = 0;
        foreach (Renderer renderer in enemyRenderers)
        {
            Material[] currentOriginalMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                currentOriginalMaterials[i] = originalMaterials[materialIndex++];
            }
            renderer.materials = currentOriginalMaterials;
        }
        
        isFlashing = false;
        Debug.Log("Damage flash effect complete");
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
