using UnityEngine;
using System;

namespace Inventory
{
    // Authors: Jeff Cui, Elaine Zhao
    // Defines the various item types, enums (ResourceType, GemEffect, PotionType), and base classes used by the inventory system.
    
    // Resource types (raw materials)
    public enum ResourceType
    {
        GemDust,
        Mushroom
    }

    // Gem types and their effects
    public enum GemEffect
    {
        AttackDamage,
        AttackSpeed
        // Future additions: CriticalChance, LifeSteal, etc.
    }

    // Potion types and their effects
    public enum PotionType
    {
        HealthRestore,      // Used in battle
        SpeedBoost,        // Permanent upgrade
        MaxHealthBoost     // Permanent upgrade
        // Future additions: ManaRestore, DefenseBoost, etc.
    }

    // Base class for all inventory items
    [System.Serializable]
    public abstract class InventoryItem
    {
        public string id;           // Unique identifier
        public string name;         // Display name
        public string description;  // Item description
        public int quantity;        // Stack size

        public InventoryItem(string name, string description)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            this.description = description;
            this.quantity = 1;
        }
    }

    // Resource item (raw materials)
    [System.Serializable]
    public class Resource : InventoryItem
    {
        public ResourceType type;

        public Resource(ResourceType type, string name, string description) : base(name, description)
        {
            this.type = type;
        }
    }

    // Gem item
    [System.Serializable]
    public class Gem : InventoryItem
    {
        public GemEffect effect;
        public float effectValue;   // The magnitude of the effect
        public int level;           // Gem level for upgrades

        public Gem(GemEffect effect, float effectValue, int level, string name, string description) 
            : base(name, description)
        {
            this.effect = effect;
            this.effectValue = effectValue;
            this.level = level;
        }
    }

    // Potion item
    [System.Serializable]
    public class Potion : InventoryItem
    {
        public PotionType type;
        public float effectValue;   // The magnitude of the effect
        public bool isPermanent;    // Whether the effect is permanent

        public Potion(PotionType type, float effectValue, bool isPermanent, string name, string description) 
            : base(name, description)
        {
            this.type = type;
            this.effectValue = effectValue;
            this.isPermanent = isPermanent;
        }
    }
} 