using UnityEngine;
using Inventory;

public class ItemDropper : MonoBehaviour
{
    [System.Serializable]
    public class DropChance
    {
        public ResourceType resourceType;
        [Range(0, 1)] public float chance = 0.5f;
        public int minAmount = 1;
        public int maxAmount = 3;
    }

    public DropChance[] possibleDrops;

    private void OnDestroy()
    {
        // Only drop items if this is being destroyed during gameplay
        if (Application.isPlaying)
        {
            DropItems();
        }
    }

    private void DropItems()
    {
        foreach (DropChance drop in possibleDrops)
        {
            if (Random.value <= drop.chance)
            {
                int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);
                InventoryManager.Instance.AddResource(drop.resourceType, amount);
            }
        }
    }
} 