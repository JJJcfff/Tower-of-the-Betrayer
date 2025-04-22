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
        
        // Count of upgrades for each stat
        public int damageUpgradeCount = 0;
        public int speedUpgradeCount = 0;
        public int rangeUpgradeCount = 0;
        
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
    
    // Base cost of upgrading a weapon stat
    private const int BASE_UPGRADE_COST = 10;
    
    // Calculate upgrade cost based on the number of upgrades already applied
    private int CalculateUpgradeCost(int upgradeCount)
    {
        // If it's the first upgrade, cost is exactly BASE_UPGRADE_COST
        if (upgradeCount == 0)
            return BASE_UPGRADE_COST;
            
        // For subsequent upgrades, apply the scaling formula: base * 1.05^upgradeCount
        return Mathf.RoundToInt(BASE_UPGRADE_COST * Mathf.Pow(1.06f, upgradeCount));
    }

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
        
        // Load sword upgrade counts
        swordStats.damageUpgradeCount = PlayerPrefs.GetInt("SwordDamageUpgrades", 0);
        swordStats.speedUpgradeCount = PlayerPrefs.GetInt("SwordSpeedUpgrades", 0);
        swordStats.rangeUpgradeCount = PlayerPrefs.GetInt("SwordRangeUpgrades", 0);

        // Load staff stats
        staffStats.damage = PlayerPrefs.GetFloat("StaffDamage", 10f);
        staffStats.attackSpeed = PlayerPrefs.GetFloat("StaffSpeed", 5f);
        staffStats.attackRange = PlayerPrefs.GetFloat("StaffRange", 10f);
        
        // Load staff gem dust spent
        staffStats.gemDustSpentOnDamage = PlayerPrefs.GetInt("StaffDamageGemDust", 0);
        staffStats.gemDustSpentOnSpeed = PlayerPrefs.GetInt("StaffSpeedGemDust", 0);
        staffStats.gemDustSpentOnRange = PlayerPrefs.GetInt("StaffRangeGemDust", 0);
        
        // Load staff upgrade counts
        staffStats.damageUpgradeCount = PlayerPrefs.GetInt("StaffDamageUpgrades", 0);
        staffStats.speedUpgradeCount = PlayerPrefs.GetInt("StaffSpeedUpgrades", 0);
        staffStats.rangeUpgradeCount = PlayerPrefs.GetInt("StaffRangeUpgrades", 0);
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
        
        // Save sword upgrade counts
        PlayerPrefs.SetInt("SwordDamageUpgrades", swordStats.damageUpgradeCount);
        PlayerPrefs.SetInt("SwordSpeedUpgrades", swordStats.speedUpgradeCount);
        PlayerPrefs.SetInt("SwordRangeUpgrades", swordStats.rangeUpgradeCount);

        // Save staff stats
        PlayerPrefs.SetFloat("StaffDamage", staffStats.damage);
        PlayerPrefs.SetFloat("StaffSpeed", staffStats.attackSpeed);
        PlayerPrefs.SetFloat("StaffRange", staffStats.attackRange);
        
        // Save staff gem dust spent
        PlayerPrefs.SetInt("StaffDamageGemDust", staffStats.gemDustSpentOnDamage);
        PlayerPrefs.SetInt("StaffSpeedGemDust", staffStats.gemDustSpentOnSpeed);
        PlayerPrefs.SetInt("StaffRangeGemDust", staffStats.gemDustSpentOnRange);
        
        // Save staff upgrade counts
        PlayerPrefs.SetInt("StaffDamageUpgrades", staffStats.damageUpgradeCount);
        PlayerPrefs.SetInt("StaffSpeedUpgrades", staffStats.speedUpgradeCount);
        PlayerPrefs.SetInt("StaffRangeUpgrades", staffStats.rangeUpgradeCount);

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
        WeaponStats stats = type == WeaponType.Sword ? swordStats : staffStats;
        int upgradeCost = CalculateUpgradeCost(stats.damageUpgradeCount);
        
        // Try to spend gem dust
        if (!InventoryManager.Instance.UseResource(ResourceType.GemDust, upgradeCost))
        {
            Debug.Log($"Not enough Gem Dust for upgrade! Need {upgradeCost}");
            return false;
        }
        
        // Apply upgrade and track cost
        if (type == WeaponType.Sword)
        {
            swordStats.damage = CalculateUpgradedStat(type, swordStats.damage, "Damage");
            swordStats.gemDustSpentOnDamage += upgradeCost;
            swordStats.damageUpgradeCount++;
            Debug.Log($"Sword damage upgraded. New cost will be: {CalculateUpgradeCost(swordStats.damageUpgradeCount)}");
        }
        else
        {
            staffStats.damage = CalculateUpgradedStat(type, staffStats.damage, "Damage");
            staffStats.gemDustSpentOnDamage += upgradeCost;
            staffStats.damageUpgradeCount++;
            Debug.Log($"Staff damage upgraded. New cost will be: {CalculateUpgradeCost(staffStats.damageUpgradeCount)}");
        }
        SaveStats();
        return true;
    }

    public bool UpgradeWeaponSpeed(WeaponType type)
    {
        WeaponStats stats = type == WeaponType.Sword ? swordStats : staffStats;
        int upgradeCost = CalculateUpgradeCost(stats.speedUpgradeCount);
        
        // Try to spend gem dust
        if (!InventoryManager.Instance.UseResource(ResourceType.GemDust, upgradeCost))
        {
            Debug.Log($"Not enough Gem Dust for upgrade! Need {upgradeCost}");
            return false;
        }
        
        // Apply upgrade and track cost
        if (type == WeaponType.Sword)
        {
            swordStats.attackSpeed = CalculateUpgradedStat(type, swordStats.attackSpeed, "Speed");
            swordStats.gemDustSpentOnSpeed += upgradeCost;
            swordStats.speedUpgradeCount++;
            Debug.Log($"Sword speed upgraded. New cost will be: {CalculateUpgradeCost(swordStats.speedUpgradeCount)}");
        }
        else
        {
            staffStats.attackSpeed = CalculateUpgradedStat(type, staffStats.attackSpeed, "Speed");
            staffStats.gemDustSpentOnSpeed += upgradeCost;
            staffStats.speedUpgradeCount++;
            Debug.Log($"Staff speed upgraded. New cost will be: {CalculateUpgradeCost(staffStats.speedUpgradeCount)}");
        }
        SaveStats();
        return true;
    }

    public bool UpgradeWeaponRange(WeaponType type)
    {
        WeaponStats stats = type == WeaponType.Sword ? swordStats : staffStats;
        int upgradeCost = CalculateUpgradeCost(stats.rangeUpgradeCount);
        
        // Try to spend gem dust
        if (!InventoryManager.Instance.UseResource(ResourceType.GemDust, upgradeCost))
        {
            Debug.Log($"Not enough Gem Dust for upgrade! Need {upgradeCost}");
            return false;
        }
        
        // Apply upgrade and track cost
        if (type == WeaponType.Sword)
        {
            swordStats.attackRange = CalculateUpgradedStat(type, swordStats.attackRange, "Range");
            swordStats.gemDustSpentOnRange += upgradeCost;
            swordStats.rangeUpgradeCount++;
            Debug.Log($"Sword range upgraded. New cost will be: {CalculateUpgradeCost(swordStats.rangeUpgradeCount)}");
        }
        else
        {
            staffStats.attackRange = CalculateUpgradedStat(type, staffStats.attackRange, "Range");
            staffStats.gemDustSpentOnRange += upgradeCost;
            staffStats.rangeUpgradeCount++;
            Debug.Log($"Staff range upgraded. New cost will be: {CalculateUpgradeCost(staffStats.rangeUpgradeCount)}");
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
                attackRange = 4f,
                // Reset all upgrade counters to zero
                gemDustSpentOnDamage = 0,
                gemDustSpentOnSpeed = 0,
                gemDustSpentOnRange = 0,
                damageUpgradeCount = 0,
                speedUpgradeCount = 0,
                rangeUpgradeCount = 0
            };
        }
        else
        {
            staffStats = new WeaponStats()
            {
                damage = 10f,
                attackSpeed = 5f,
                attackRange = 10f,
                // Reset all upgrade counters to zero
                gemDustSpentOnDamage = 0,
                gemDustSpentOnSpeed = 0,
                gemDustSpentOnRange = 0,
                damageUpgradeCount = 0,
                speedUpgradeCount = 0,
                rangeUpgradeCount = 0
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
            attackRange = 4f,
            // Reset all upgrade counters to zero
            gemDustSpentOnDamage = 0,
            gemDustSpentOnSpeed = 0,
            gemDustSpentOnRange = 0,
            damageUpgradeCount = 0,
            speedUpgradeCount = 0,
            rangeUpgradeCount = 0
        };
        staffStats = new WeaponStats()
        {
            damage = 10f,
            attackSpeed = 5f,
            attackRange = 10f,
            // Reset all upgrade counters to zero
            gemDustSpentOnDamage = 0,
            gemDustSpentOnSpeed = 0,
            gemDustSpentOnRange = 0,
            damageUpgradeCount = 0,
            speedUpgradeCount = 0,
            rangeUpgradeCount = 0
        };

        // Save the reset stats
        SaveStats();
        
        Debug.Log("Player stats reset to default values");
    }

    // Get the current cost to upgrade a specific weapon stat
    public int GetUpgradeCost(WeaponType type, string statType)
    {
        WeaponStats stats = type == WeaponType.Sword ? swordStats : staffStats;
        
        switch (statType)
        {
            case "Damage":
                return CalculateUpgradeCost(stats.damageUpgradeCount);
            case "Speed":
                return CalculateUpgradeCost(stats.speedUpgradeCount);
            case "Range":
                return CalculateUpgradeCost(stats.rangeUpgradeCount);
            default:
                Debug.LogError($"Unknown stat type: {statType}");
                return 0;
        }
    }
} 