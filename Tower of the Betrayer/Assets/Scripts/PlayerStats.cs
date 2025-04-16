// Authors: Jeff Cui, Elaine Zhao
// Singleton class that manages persistent player stats (health, speed, weapon stats) and handles saving/loading.

using UnityEngine;
using Inventory;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Base Stats")]
    public float MaxHealth = 100f;
    public float Speed = 10f;

    [System.Serializable]
    public class WeaponStats
    {
        public float damage = 10f;
        public float attackSpeed = 5f;
        public float attackRange = 10f;
        
        // Track gem dust spent on each stat
        public int gemDustSpentOnDamage = 0;
        public int gemDustSpentOnSpeed = 0;
        public int gemDustSpentOnRange = 0;
        
        // Total gem dust spent on this weapon
        public int TotalGemDustSpent => gemDustSpentOnDamage + gemDustSpentOnSpeed + gemDustSpentOnRange;
    }

    [Header("Weapon Stats")]
    public WeaponStats swordStats = new WeaponStats() 
    { 
        damage = 20f,
        attackSpeed = 3f,
        attackRange = 4f
    };
    public WeaponStats staffStats = new WeaponStats()
    {
        damage = 10f,
        attackSpeed = 5f,
        attackRange = 10f
    };
    
    // Cost of upgrading a weapon stat
    private const int UPGRADE_COST = 10;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadStats()
    {
        // Load saved stats from PlayerPrefs
        MaxHealth = PlayerPrefs.GetFloat("MaxHealth", 100f);
        Speed = PlayerPrefs.GetFloat("Speed", 10f);

        // Load sword stats with new default values
        swordStats.damage = PlayerPrefs.GetFloat("SwordDamage", 20f);
        swordStats.attackSpeed = PlayerPrefs.GetFloat("SwordSpeed", 3f);
        swordStats.attackRange = PlayerPrefs.GetFloat("SwordRange", 4f);
        
        // Load sword gem dust spent
        swordStats.gemDustSpentOnDamage = PlayerPrefs.GetInt("SwordDamageGemDust", 0);
        swordStats.gemDustSpentOnSpeed = PlayerPrefs.GetInt("SwordSpeedGemDust", 0);
        swordStats.gemDustSpentOnRange = PlayerPrefs.GetInt("SwordRangeGemDust", 0);

        // Load staff stats
        staffStats.damage = PlayerPrefs.GetFloat("StaffDamage", 10f);
        staffStats.attackSpeed = PlayerPrefs.GetFloat("StaffSpeed", 5f);
        staffStats.attackRange = PlayerPrefs.GetFloat("StaffRange", 10f);
        
        // Load staff gem dust spent
        staffStats.gemDustSpentOnDamage = PlayerPrefs.GetInt("StaffDamageGemDust", 0);
        staffStats.gemDustSpentOnSpeed = PlayerPrefs.GetInt("StaffSpeedGemDust", 0);
        staffStats.gemDustSpentOnRange = PlayerPrefs.GetInt("StaffRangeGemDust", 0);
    }

    private void SaveStats()
    {
        // Save base stats
        PlayerPrefs.SetFloat("MaxHealth", MaxHealth);
        PlayerPrefs.SetFloat("Speed", Speed);

        // Save sword stats
        PlayerPrefs.SetFloat("SwordDamage", swordStats.damage);
        PlayerPrefs.SetFloat("SwordSpeed", swordStats.attackSpeed);
        PlayerPrefs.SetFloat("SwordRange", swordStats.attackRange);
        
        // Save sword gem dust spent
        PlayerPrefs.SetInt("SwordDamageGemDust", swordStats.gemDustSpentOnDamage);
        PlayerPrefs.SetInt("SwordSpeedGemDust", swordStats.gemDustSpentOnSpeed);
        PlayerPrefs.SetInt("SwordRangeGemDust", swordStats.gemDustSpentOnRange);

        // Save staff stats
        PlayerPrefs.SetFloat("StaffDamage", staffStats.damage);
        PlayerPrefs.SetFloat("StaffSpeed", staffStats.attackSpeed);
        PlayerPrefs.SetFloat("StaffRange", staffStats.attackRange);
        
        // Save staff gem dust spent
        PlayerPrefs.SetInt("StaffDamageGemDust", staffStats.gemDustSpentOnDamage);
        PlayerPrefs.SetInt("StaffSpeedGemDust", staffStats.gemDustSpentOnSpeed);
        PlayerPrefs.SetInt("StaffRangeGemDust", staffStats.gemDustSpentOnRange);

        PlayerPrefs.Save();
    }

    public void IncreaseMaxHealth(float amount)
    {
        MaxHealth += amount;
        SaveStats();
    }

    public void IncreaseSpeed(float amount)
    {
        Speed += amount;
        SaveStats();
        
        // Update active PlayerMovement if it exists
        PlayerMovement playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            // Update both base speed and current speed to maintain any temporary boosts
            float tempBoostAmount = playerMovement.speed - playerMovement.baseSpeed;
            playerMovement.baseSpeed = Speed;
            playerMovement.speed = Speed + tempBoostAmount;
        }
    }

    public float GetWeaponDamage(WeaponType type)
    {
        return type == WeaponType.Sword ? swordStats.damage : staffStats.damage;
    }

    public float GetWeaponSpeed(WeaponType type)
    {
        return type == WeaponType.Sword ? swordStats.attackSpeed : staffStats.attackSpeed;
    }

    public float GetWeaponRange(WeaponType type)
    {
        return type == WeaponType.Sword ? swordStats.attackRange : staffStats.attackRange;
    }
  
    private float CalculateUpgradedStat(WeaponType type, float currentValue, string statType)
    {
        // Sword upgrade logic
        if (type == WeaponType.Sword)
        {
            switch (statType)
            {
                case "Damage":
                    return currentValue * 1.2f;
                    
                case "Speed":
                    return currentValue * 1.2f;
                    
                case "Range":
                    return currentValue * 1.2f;
                    
                default:
                    Debug.LogError($"Unknown stat type: {statType}");
                    return currentValue;
            }
        }
        // Staff upgrade logic
        else
        {
            switch (statType)
            {
                case "Damage":
                    return currentValue + 2f;
                    
                case "Speed":
                    return currentValue + 2f;
                    
                case "Range":
                    return currentValue + 0.5f;
                    
                default:
                    Debug.LogError($"Unknown stat type: {statType}");
                    return currentValue;
            }
        }
    }

    public bool UpgradeWeaponDamage(WeaponType type)
    {
        // Try to spend gem dust
        if (!InventoryManager.Instance.UseResource(ResourceType.GemDust, UPGRADE_COST))
        {
            Debug.Log("Not enough Gem Dust for upgrade!");
            return false;
        }
        
        // Apply upgrade and track cost
        if (type == WeaponType.Sword)
        {
            swordStats.damage = CalculateUpgradedStat(type, swordStats.damage, "Damage");
            swordStats.gemDustSpentOnDamage += UPGRADE_COST;
        }
        else
        {
            staffStats.damage = CalculateUpgradedStat(type, staffStats.damage, "Damage");
            staffStats.gemDustSpentOnDamage += UPGRADE_COST;
        }
        SaveStats();
        return true;
    }

    public bool UpgradeWeaponSpeed(WeaponType type)
    {
        // Try to spend gem dust
        if (!InventoryManager.Instance.UseResource(ResourceType.GemDust, UPGRADE_COST))
        {
            Debug.Log("Not enough Gem Dust for upgrade!");
            return false;
        }
        
        // Apply upgrade and track cost
        if (type == WeaponType.Sword)
        {
            swordStats.attackSpeed = CalculateUpgradedStat(type, swordStats.attackSpeed, "Speed");
            swordStats.gemDustSpentOnSpeed += UPGRADE_COST;
        }
        else
        {
            staffStats.attackSpeed = CalculateUpgradedStat(type, staffStats.attackSpeed, "Speed");
            staffStats.gemDustSpentOnSpeed += UPGRADE_COST;
        }
        SaveStats();
        return true;
    }

    public bool UpgradeWeaponRange(WeaponType type)
    {
        // Try to spend gem dust
        if (!InventoryManager.Instance.UseResource(ResourceType.GemDust, UPGRADE_COST))
        {
            Debug.Log("Not enough Gem Dust for upgrade!");
            return false;
        }
        
        // Apply upgrade and track cost
        if (type == WeaponType.Sword)
        {
            swordStats.attackRange = CalculateUpgradedStat(type, swordStats.attackRange, "Range");
            swordStats.gemDustSpentOnRange += UPGRADE_COST;
        }
        else
        {
            staffStats.attackRange = CalculateUpgradedStat(type, staffStats.attackRange, "Range");
            staffStats.gemDustSpentOnRange += UPGRADE_COST;
        }
        SaveStats();
        return true;
    }
    
    // Get total gem dust spent on a weapon
    public int GetTotalGemDustSpent(WeaponType type)
    {
        return type == WeaponType.Sword ? swordStats.TotalGemDustSpent : staffStats.TotalGemDustSpent;
    }
    
    // Reset a weapon's stats and refund 80% of gem dust spent
    public void ResetWeaponStats(WeaponType type)
    {
        WeaponStats stats = type == WeaponType.Sword ? swordStats : staffStats;
        
        // Calculate refund amount (80% of total spent)
        int refundAmount = Mathf.FloorToInt(stats.TotalGemDustSpent * 0.8f);
        
        if (refundAmount > 0)
        {
            // Add refund to inventory
            InventoryManager.Instance.AddResource(ResourceType.GemDust, refundAmount);
            Debug.Log($"Refunded {refundAmount} Gem Dust for resetting {type} upgrades");
        }
        
        // Reset the weapon stats to default
        if (type == WeaponType.Sword)
        {
            swordStats = new WeaponStats()
            {
                damage = 20f,
                attackSpeed = 3f,
                attackRange = 4f
            };
        }
        else
        {
            staffStats = new WeaponStats()
            {
                damage = 10f,
                attackSpeed = 5f,
                attackRange = 10f
            };
        }
        
        SaveStats();
    }

    public void ResetToDefault()
    {
        // Reset base stats
        MaxHealth = 100f;
        Speed = 10f;

        // Reset weapon stats with proper defaults
        swordStats = new WeaponStats()
        {
            damage = 20f,
            attackSpeed = 3f,
            attackRange = 4f
        };
        staffStats = new WeaponStats()
        {
            damage = 10f,
            attackSpeed = 5f,
            attackRange = 10f
        };

        // Save the reset stats
        SaveStats();
        
        Debug.Log("Player stats reset to default values");
    }
} 