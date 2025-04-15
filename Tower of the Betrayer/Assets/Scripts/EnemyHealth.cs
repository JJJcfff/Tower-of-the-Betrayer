// Authors: Jeff Cui, Elaine Zhao

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Manages the enemy's health and computes damage taken.
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100;
    public float currentHealth;

    [Header("Damage Flash Settings")]
    public float damageFlashDuration = 0.2f;
    public Color damageFlashColor = Color.red;
    public float damagePulseScale = 1.2f;

    [Header("Audio")]
    public AudioClip deathSound;

    private Material[] originalMaterials;
    private Material[] damageMaterials;
    private bool isFlashing = false;
    private bool hasDied = false;

    private Renderer[] enemyRenderers;
    private Vector3 originalScale;
    private Transform enemyTransform;

    private AudioSource audioSource;

    private void Start()
    {
        // Apply floor difficulty scaling
        if (FloorDifficultyManager.Instance != null)
        {
            FloorDifficultyManager.Instance.ModifyEnemyHealth(this);
        }
        else
        {
            currentHealth = maxHealth;
        }

        enemyTransform = transform;
        originalScale = enemyTransform.localScale;

        enemyRenderers = GetComponentsInChildren<Renderer>();
        if (enemyRenderers.Length == 0)
        {
            Debug.LogWarning("No renderers found on enemy or its children!");
            return;
        }

        Debug.Log($"Found {enemyRenderers.Length} renderers on enemy");

        List<Material> originalMaterialsList = new List<Material>();
        foreach (Renderer renderer in enemyRenderers)
        {
            originalMaterialsList.AddRange(renderer.materials);
        }
        originalMaterials = originalMaterialsList.ToArray();

        damageMaterials = new Material[originalMaterials.Length];
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            damageMaterials[i] = new Material(originalMaterials[i]);
            damageMaterials[i].color = damageFlashColor;
            damageMaterials[i].EnableKeyword("_EMISSION");
            damageMaterials[i].SetColor("_EmissionColor", damageFlashColor * 2f);
        }

        // Setup audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // MODIFIED: Load audio from Resources if not set in Inspector
        if (deathSound == null)
        {
            deathSound = Resources.Load<AudioClip>("enemyDeath");
            if (deathSound == null)
            {
                Debug.LogWarning("Could not load enemyDeath.wav from Resources folder!");
            }
        }
    }

    void Update()
    {
        if (!hasDied && currentHealth <= 0)
        {
            hasDied = true;

            // Play death sound
            if (deathSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            float delay = (deathSound != null) ? deathSound.length : 0f;
            Destroy(gameObject, delay);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

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

        enemyTransform.localScale = originalScale * damagePulseScale;

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

        yield return new WaitForSeconds(damageFlashDuration);

        enemyTransform.localScale = originalScale;

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
