using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Inventory
{
    [System.Serializable]
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        // Resource inventory
        private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
        
        // Gem and Potion inventories
        private List<Gem> gems = new List<Gem>();
        private List<Potion> potions = new List<Potion>();

        // Constants for gem operations
        private const int GEM_TRANSFER_COST = 5;  // Gem scraps needed to transfer a gem
        private const int GEM_CREATION_COST = 10; // Gem scraps needed to create a new gem

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeInventory();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeInventory()
        {
            // Initialize resources with 0 quantity
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 0;
            }
        }

        #region Resource Management
        public void AddResource(ResourceType type, int amount)
        {
            if (!resources.ContainsKey(type))
            {
                resources[type] = 0;
            }
            resources[type] += amount;
            Debug.Log($"Added {amount} {type}. New total: {resources[type]}");
        }

        public bool UseResource(ResourceType type, int amount)
        {
            if (!resources.ContainsKey(type) || resources[type] < amount)
            {
                Debug.Log($"Not enough {type}. Required: {amount}, Available: {resources[type]}");
                return false;
            }
            
            resources[type] -= amount;
            Debug.Log($"Used {amount} {type}. Remaining: {resources[type]}");
            return true;
        }

        public int GetResourceAmount(ResourceType type)
        {
            return resources.ContainsKey(type) ? resources[type] : 0;
        }
        #endregion

        #region Gem Management
        public Gem CreateGem(GemEffect effect, float effectValue, int level)
        {
            if (!UseResource(ResourceType.GemScraps, GEM_CREATION_COST))
            {
                Debug.Log("Not enough gem scraps to create a new gem!");
                return null;
            }

            Gem newGem = new Gem(
                effect,
                effectValue,
                level,
                $"Level {level} {effect} Gem",
                $"Increases {effect} by {effectValue}"
            );
            
            gems.Add(newGem);
            return newGem;
        }

        public bool TransferGem(string gemId, WeaponType fromWeapon, WeaponType toWeapon)
        {
            if (!UseResource(ResourceType.GemScraps, GEM_TRANSFER_COST))
            {
                Debug.Log("Not enough gem scraps to transfer the gem!");
                return false;
            }

            Gem gem = gems.Find(g => g.id == gemId);
            if (gem == null)
            {
                Debug.Log("Gem not found!");
                return false;
            }

            // Here you would implement the logic to remove the gem from one weapon
            // and add it to another. This will need to be coordinated with your
            // weapon system.

            return true;
        }

        public List<Gem> GetAllGems()
        {
            return new List<Gem>(gems);
        }

        public Gem GetGemById(string id)
        {
            return gems.Find(g => g.id == id);
        }
        #endregion

        #region Potion Management
        public Potion CreatePotion(PotionType type, float effectValue, bool isPermanent)
        {
            // Check if we have enough mushrooms (cost could vary based on potion type)
            int mushroomCost = isPermanent ? 5 : 2;
            
            if (!UseResource(ResourceType.Mushroom, mushroomCost))
            {
                Debug.Log("Not enough mushrooms to create the potion!");
                return null;
            }

            Potion newPotion = new Potion(
                type,
                effectValue,
                isPermanent,
                $"{type} Potion",
                GetPotionDescription(type, effectValue, isPermanent)
            );
            
            potions.Add(newPotion);
            return newPotion;
        }

        public bool UsePotion(string potionId)
        {
            Potion potion = potions.Find(p => p.id == potionId);
            if (potion == null)
            {
                Debug.Log("Potion not found!");
                return false;
            }

            // Apply potion effect (this will need to be coordinated with your player stats system)
            ApplyPotionEffect(potion);

            // Remove the potion if it's not permanent
            if (!potion.isPermanent)
            {
                potions.Remove(potion);
            }

            return true;
        }

        private void ApplyPotionEffect(Potion potion)
        {
            // This method will need to be implemented to coordinate with your
            // player stats system. For now, we'll just log the effect
            Debug.Log($"Applied potion effect: {potion.type} with value {potion.effectValue}");
        }

        private string GetPotionDescription(PotionType type, float value, bool isPermanent)
        {
            string duration = isPermanent ? "Permanently" : "Temporarily";
            switch (type)
            {
                case PotionType.HealthRestore:
                    return $"Restores {value} health points";
                case PotionType.SpeedBoost:
                    return $"{duration} increases movement speed by {value}%";
                case PotionType.MaxHealthBoost:
                    return $"{duration} increases maximum health by {value}";
                default:
                    return "Unknown potion effect";
            }
        }

        public List<Potion> GetAllPotions()
        {
            return new List<Potion>(potions);
        }

        public Potion GetPotionById(string id)
        {
            return potions.Find(p => p.id == id);
        }
        #endregion

        #region Save/Load System
        public void SaveInventory()
        {
            // Implement save logic here
            // You might want to use PlayerPrefs or a file-based system
        }

        public void LoadInventory()
        {
            // Implement load logic here
        }
        #endregion
    }

    // Enum for weapon types (used in gem transfer)
    public enum WeaponType
    {
        Sword,
        Staff
    }
} 