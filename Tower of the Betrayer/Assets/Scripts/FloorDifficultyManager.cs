// Authors: Jeff Cui, Elaine Zhao
// Manages difficulty scaling and random modifiers for each floor

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorDifficultyManager : MonoBehaviour
{
    public static FloorDifficultyManager Instance { get; private set; }

    [Header("Base Difficulty Scaling")]
    [Tooltip("Health increase per floor (percentage)")]
    public float enemyHealthIncreasePerFloor = 10f;
    [Tooltip("Damage increase per floor (percentage)")]
    public float enemyDamageIncreasePerFloor = 10f;
    [Tooltip("Increase in number of enemies per floor (percentage)")]
    public float enemyCountIncreasePerFloor = 30f;

    [Header("Modifier Settings")]
    [Tooltip("Maximum number of modifiers that can be active on a floor (0-3)")]
    [Range(0, 3)]
    public int maxModifiersPerFloor = 3;

    // Current active modifiers
    private List<FloorModifier> activeModifiers = new List<FloorModifier>();
    
    // Flag to track if modifiers have been generated for the current floor
    private bool modifiersGeneratedForCurrentFloor = false;
    
    // Current floor's difficulty multipliers
    private float currentHealthMultiplier = 1f;
    private float currentDamageMultiplier = 1f;
    private float currentEnemyCountMultiplier = 1f;
    private float currentPlayerSpeedMultiplier = 1f;
    private float currentPlayerDamageMultiplier = 1f;
    private float currentPlayerHealthMultiplier = 1f;
    private float currentPlayerHealthRegenRate = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize modifiers for the first floor if not already generated
        if (!modifiersGeneratedForCurrentFloor && GameManager.Instance != null)
        {
            GenerateFloorModifiers();
        }
    }

    public void GenerateFloorModifiers()
    {
        int currentFloor = GameManager.Instance.currentFloor;
        
        // Clear any existing modifiers
        activeModifiers.Clear();
        
        // Calculate base difficulty scaling
        CalculateBaseDifficulty(currentFloor);
        
        // Generate random modifiers
        GenerateRandomModifiers();
        
        // Apply all modifiers
        ApplyModifiers();
        
        // Log the current floor settings
        LogFloorSettings();
        
        // Mark as generated for this floor
        modifiersGeneratedForCurrentFloor = true;
    }
    
    // Call this to apply existing modifiers without regenerating them
    public void ApplyExistingModifiers()
    {
        if (modifiersGeneratedForCurrentFloor)
        {
            Debug.Log("Applying existing modifiers to the current floor");
            
            // Just reapply the modifiers that were already generated
            ApplyModifiers();
        }
        else
        {
            // No modifiers exist yet, generate new ones
            Debug.Log("No modifiers exist for the current floor, generating new ones");
            GenerateFloorModifiers();
        }
    }
    
    // Call this when floor is completed to mark that modifiers need to be generated for the next floor
    public void MarkForModifierGeneration()
    {
        modifiersGeneratedForCurrentFloor = false;
    }

    private void CalculateBaseDifficulty(int floor)
    {
        // Calculate base difficulty increase (floor 1 is baseline)
        int floorsCompleted = floor - 1;
        
        // Calculate multipliers based on completed floors (each floor adds percentage)
        currentHealthMultiplier = 1f + (floorsCompleted * enemyHealthIncreasePerFloor / 100f);
        currentDamageMultiplier = 1f + (floorsCompleted * enemyDamageIncreasePerFloor / 100f);
        currentEnemyCountMultiplier = 1f + (floorsCompleted * enemyCountIncreasePerFloor / 100f);
        
        // Reset player-related multipliers to default
        currentPlayerSpeedMultiplier = 1f;
        currentPlayerDamageMultiplier = 1f;
        currentPlayerHealthMultiplier = 1f;
        currentPlayerHealthRegenRate = 0f;
    }

    private void GenerateRandomModifiers()
    {
        // Make sure Random is properly seeded with a varied seed
        int uniqueSeed = (int)(System.DateTime.Now.Ticks % 1000000) + 
                       (GameManager.Instance.currentFloor * 137) + 
                       Random.Range(1, 10000);
        Random.InitState(uniqueSeed);
        
        int currentFloor = GameManager.Instance.currentFloor;
        
        // Simple fixed distribution for modifier count (0-3)
        // 10% chance for 0, 20% chance for 1, 30% chance for 2, 40% chance for 3
        float randomValue = Random.value;
        int modifierCount;
        
        if (randomValue < 0.1f)
        {
            modifierCount = 0;       // 10% chance
        }
        else if (randomValue < 0.3f)
        {
            modifierCount = 1;       // 20% chance
        }
        else if (randomValue < 0.6f)
        {
            modifierCount = 2;       // 30% chance
        }
        else
        {
            modifierCount = 3;       // 40% chance
        }
        
        Debug.Log($"Generating {modifierCount} random modifiers for floor {currentFloor}");
        
        List<ModifierType> availableModifierTypes = new List<ModifierType>(System.Enum.GetValues(typeof(ModifierType)).Cast<ModifierType>());
        
        // Generate the random modifiers
        for (int i = 0; i < modifierCount; i++)
        {
            if (availableModifierTypes.Count == 0) break;
            
            // Choose a random modifier type from the available ones
            int typeIndex = Random.Range(0, availableModifierTypes.Count);
            ModifierType modifierType = availableModifierTypes[typeIndex];
            
            // Remove this type so we don't get duplicates
            availableModifierTypes.RemoveAt(typeIndex);
            
            // Randomly determine if the effect is increasing (+) or decreasing (-) the stat
            bool isIncreasing = Random.value < 0.5f;
            
            // Determine if the modifier is good for the player based on its type and direction
            bool isGood;
            
            switch (modifierType)
            {
                case ModifierType.PlayerSpeed:
                case ModifierType.PlayerDamage:
                case ModifierType.PlayerHealth:
                case ModifierType.PlayerHealthRegen:
                    // For player stats, increasing is good, decreasing is bad
                    isGood = isIncreasing;
                    break;
                
                case ModifierType.EnemyDamage:
                case ModifierType.EnemySpeed:
                    // For enemy stats, decreasing is good, increasing is bad
                    isGood = !isIncreasing;
                    break;
                
                default:
                    isGood = false;
                    break;
            }
            
            // Simple random intensity level (0-2) with equal chance
            int intensityLevel = Random.Range(0, 3);
            
            // Create the modifier
            FloorModifier modifier = new FloorModifier(modifierType, isGood, intensityLevel);
            activeModifiers.Add(modifier);
            
            string effectDirection = isGood ? "Good" : "Bad";
            Debug.Log($"Added {effectDirection} {modifierType} modifier with intensity {intensityLevel}");
        }
    }

    private void ApplyModifiers()
    {
        // Variables to track enemy speed modifications
        float enemySpeedModifier = 0f;
        
        foreach (FloorModifier modifier in activeModifiers)
        {
            // Get the base modifier value based on intensity
            float value = GetModifierValue(modifier.intensityLevel);
            
            // Apply the modifier based on its type and whether it's good or bad
            switch (modifier.type)
            {
                case ModifierType.PlayerSpeed:
                    currentPlayerSpeedMultiplier += modifier.isGood ? value : -value;
                    break;
                
                case ModifierType.PlayerDamage:
                    currentPlayerDamageMultiplier += modifier.isGood ? value : -value;
                    break;
                
                case ModifierType.PlayerHealth:
                    currentPlayerHealthMultiplier += modifier.isGood ? value : -value;
                    break;
                
                case ModifierType.PlayerHealthRegen:
                    // Apply health regen based on intensity level
                    float regenValue = modifier.intensityLevel == 0 ? 0.5f : 
                                      (modifier.intensityLevel == 1 ? 1f : 1.5f);
                                      
                    // Apply positive or negative regen based on whether it's good or bad
                    currentPlayerHealthRegenRate = modifier.isGood ? regenValue : -regenValue;
                    break;
                
                case ModifierType.EnemyDamage:
                    // If good, reduce enemy damage, if bad, increase it
                    currentDamageMultiplier += modifier.isGood ? -value : value;
                    break;
                
                case ModifierType.EnemySpeed:
                    // Track enemy speed modification - will be passed to spawners
                    // For "good" modifiers, we want to DECREASE enemy speed
                    enemySpeedModifier += modifier.isGood ? -value : value;
                    break;
            }
        }
        
        // Store enemy speed modifier somewhere it can be accessed
        // This would need to be retrieved by the WaveSpawner
        PlayerPrefs.SetFloat("EnemySpeedModifier", enemySpeedModifier);
        
        // Make sure multipliers don't go below a minimum threshold
        currentPlayerSpeedMultiplier = Mathf.Max(currentPlayerSpeedMultiplier, 0.5f);
        currentPlayerDamageMultiplier = Mathf.Max(currentPlayerDamageMultiplier, 0.5f);
        currentPlayerHealthMultiplier = Mathf.Max(currentPlayerHealthMultiplier, 0.5f);
        currentHealthMultiplier = Mathf.Max(currentHealthMultiplier, 1f);
        currentDamageMultiplier = Mathf.Max(currentDamageMultiplier, 1f);
        currentEnemyCountMultiplier = Mathf.Max(currentEnemyCountMultiplier, 1f);
        
        Debug.Log($"Applied enemy speed modifier: {enemySpeedModifier}");
    }

    private float GetModifierValue(int intensityLevel)
    {
        // Return modifier value based on intensity level
        switch (intensityLevel)
        {
            case 0: return 0.02f; // Small: 2%
            case 1: return 0.05f; // Medium: 5%
            case 2: return 0.08f; // Large: 8%
            default: return 0.05f;
        }
    }

    private void LogFloorSettings()
    {
        Debug.Log($"===== Floor {GameManager.Instance.currentFloor} Settings =====");
        Debug.Log($"Enemy Health Multiplier: {currentHealthMultiplier:F2}x");
        Debug.Log($"Enemy Damage Multiplier: {currentDamageMultiplier:F2}x");
        Debug.Log($"Enemy Count Multiplier: {currentEnemyCountMultiplier:F2}x");
        Debug.Log($"Player Speed Multiplier: {currentPlayerSpeedMultiplier:F2}x");
        Debug.Log($"Player Damage Multiplier: {currentPlayerDamageMultiplier:F2}x");
        Debug.Log($"Player Health Multiplier: {currentPlayerHealthMultiplier:F2}x");
        Debug.Log($"Player Health Regen Rate: {currentPlayerHealthRegenRate:F2} HP/sec");
        
        Debug.Log("Active Modifiers:");
        foreach (FloorModifier modifier in activeModifiers)
        {
            string intensityText = modifier.intensityLevel == 0 ? "Small" : 
                                 (modifier.intensityLevel == 1 ? "Medium" : "Large");
            Debug.Log($"- {(modifier.isGood ? "Good" : "Bad")} {modifier.type} ({intensityText})");
        }
    }

    #region Public Accessor Methods

    // Methods to get the current multipliers
    public float GetEnemyHealthMultiplier() => currentHealthMultiplier;
    public float GetEnemyDamageMultiplier() => currentDamageMultiplier;
    public float GetEnemyCountMultiplier() => currentEnemyCountMultiplier;
    public float GetPlayerSpeedMultiplier() => currentPlayerSpeedMultiplier;
    public float GetPlayerDamageMultiplier() => currentPlayerDamageMultiplier;
    public float GetPlayerHealthMultiplier() => currentPlayerHealthMultiplier;
    public float GetPlayerHealthRegenRate() => currentPlayerHealthRegenRate;
    
    // Get a list of active modifiers for display
    public List<FloorModifier> GetActiveModifiers() => activeModifiers;
    
    // Check if modifiers have already been generated for the current floor
    public bool AreModifiersGenerated() => modifiersGeneratedForCurrentFloor;

    #endregion

    #region Integration Methods

    // Called by EnemyHealth on Start
    public void ModifyEnemyHealth(EnemyHealth enemyHealth)
    {
        enemyHealth.maxHealth *= currentHealthMultiplier;
        enemyHealth.currentHealth = enemyHealth.maxHealth;
    }
    
    // Called by ContactDamager for enemy attacks
    public void ModifyEnemyDamage(ref float damage)
    {
        damage *= currentDamageMultiplier;
    }
    
    // Called by WaveSpawner to adjust spawn rate/count
    public void ModifyEnemySpawnRate(ref float spawnRate)
    {
        // Lower spawn rate means more enemies in the same time
        spawnRate /= currentEnemyCountMultiplier;
    }
    
    // Called by PlayerStats or PlayerHealth on level start
    public void ModifyPlayerHealth(PlayerHealth playerHealth)
    {
        float oldMax = playerHealth.maxHealth;
        playerHealth.maxHealth *= currentPlayerHealthMultiplier;
        
        // Adjust current health proportionally
        playerHealth.currentHealth = (playerHealth.currentHealth / oldMax) * playerHealth.maxHealth;
        
        // Update health bar
        if (playerHealth.healthBar != null)
        {
            playerHealth.healthBar.SetMaxHealth(playerHealth.maxHealth);
            playerHealth.healthBar.SetHealth(playerHealth.currentHealth);
        }
    }
    
    // Called by PlayerShooting
    public void ModifyPlayerDamage(ref float damage)
    {
        damage *= currentPlayerDamageMultiplier;
    }
    
    // Called by PlayerMovement
    public void ModifyPlayerSpeed(PlayerMovement playerMovement)
    {
        playerMovement.baseSpeed *= currentPlayerSpeedMultiplier;
        playerMovement.speed = playerMovement.baseSpeed;
    }

    // Call this from Update to apply health regen
    public void UpdatePlayerHealthRegen(PlayerHealth playerHealth)
    {
        if (currentPlayerHealthRegenRate != 0)
        {
            float regenAmount = currentPlayerHealthRegenRate * Time.deltaTime;
            
            if (regenAmount > 0)
            {
                // Health regeneration (positive value)
                playerHealth.RestoreHealth(regenAmount);
            }
            else
            {
                // Health degeneration (negative value)
                playerHealth.TakeDamage(-regenAmount); // Convert negative to positive for damage
            }
        }
    }

    #endregion
}

#region Supporting Classes and Enums

public enum ModifierType
{
    PlayerSpeed,
    PlayerDamage,
    PlayerHealth,
    PlayerHealthRegen,
    EnemyDamage,
    EnemySpeed
}

public class FloorModifier
{
    public ModifierType type;
    public bool isGood;
    public int intensityLevel; // 0 = Small, 1 = Medium, 2 = Large
    
    public FloorModifier(ModifierType type, bool isGood, int intensityLevel)
    {
        this.type = type;
        this.isGood = isGood;
        this.intensityLevel = intensityLevel;
    }
    
    public string GetDescription()
    {
        string intensityText = intensityLevel == 0 ? "Slightly" : 
                              (intensityLevel == 1 ? "Moderately" : "Greatly");
        
        string effectText = "";
        
        // For player attributes, isGood means the effect is positive for the player
        // For enemy attributes, isGood means the effect is negative for enemies (which is positive for player)
        switch (type)
        {
            case ModifierType.PlayerSpeed:
                effectText = isGood ? "Increases player movement speed" : "Decreases player movement speed";
                break;
            case ModifierType.PlayerDamage:
                effectText = isGood ? "Increases player damage" : "Decreases player damage";
                break;
            case ModifierType.PlayerHealth:
                effectText = isGood ? "Increases player health" : "Decreases player health";
                break;
            case ModifierType.PlayerHealthRegen:
                effectText = isGood ? "Player health slowly regenerates" : "Player health slowly decreases";
                break;
            case ModifierType.EnemyDamage:
                effectText = isGood ? "Decreases enemy damage" : "Increases enemy damage";
                break;
            case ModifierType.EnemySpeed:
                effectText = isGood ? "Decreases enemy speed" : "Increases enemy speed";
                break;
        }
        
        return $"{intensityText} {effectText}";
    }
}

#endregion 