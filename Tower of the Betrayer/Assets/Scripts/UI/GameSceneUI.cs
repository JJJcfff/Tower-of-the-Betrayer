using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Inventory;

public class GameSceneUI : MonoBehaviour
{
    [Header("Health Display")]
    public HealthBar healthBar;
    public Color healthBarColor = new Color(0.8f, 0.2f, 0.2f); // Dark red for health

    [Header("Resource Display")]
    public TextMeshProUGUI gemScrapsText;
    public TextMeshProUGUI mushroomText;
    public CanvasGroup resourcePanel;
    
    [Header("Potion Display")]
    public Transform potionContainer;  // Parent object for potion buttons
    public GameObject potionButtonPrefab;  // Prefab for potion button
    public CanvasGroup potionPanel;
    
    private Dictionary<string, GameObject> activePotionButtons = new Dictionary<string, GameObject>();
    private InventoryManager inventoryManager;
    private PlayerHealth playerHealth;

    private void Start()
    {
        // Get required components
        inventoryManager = InventoryManager.Instance;
        playerHealth = FindObjectOfType<PlayerHealth>();

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found!");
            return;
        }

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth not found!");
            return;
        }

        // Set up health bar
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(playerHealth.maxHealth);
            healthBar.SetHealth(playerHealth.currentHealth);
            healthBar.SetColor(healthBarColor);
        }

        // Initial UI update
        UpdateResourceDisplay();
        UpdatePotionDisplay();

        // Update the UI every 0.5 seconds
        InvokeRepeating(nameof(UpdateUI), 0.5f, 0.5f);
    }

    private void UpdateUI()
    {
        // Update health bar
        if (healthBar != null && playerHealth != null)
        {
            healthBar.SetHealth(playerHealth.currentHealth);
        }

        UpdateResourceDisplay();
        UpdatePotionDisplay();
    }

    private void UpdateResourceDisplay()
    {
        if (gemScrapsText != null)
        {
            gemScrapsText.text = $"Gem Scraps: {inventoryManager.GetResourceAmount(ResourceType.GemScraps)}";
        }
        
        if (mushroomText != null)
        {
            mushroomText.text = $"Mushrooms: {inventoryManager.GetResourceAmount(ResourceType.Mushroom)}";
        }
    }

    private void UpdatePotionDisplay()
    {
        // Get all potions from inventory
        List<Potion> potions = inventoryManager.GetAllPotions();
        HashSet<string> currentPotions = new HashSet<string>();

        foreach (Potion potion in potions)
        {
            // Skip permanent potions as they shouldn't be shown in the quick-use UI
            if (potion.isPermanent) continue;

            currentPotions.Add(potion.id);

            // If we don't have a button for this potion yet, create one
            if (!activePotionButtons.ContainsKey(potion.id))
            {
                CreatePotionButton(potion);
            }
        }

        // Remove buttons for potions that no longer exist
        List<string> potionsToRemove = new List<string>();
        foreach (var kvp in activePotionButtons)
        {
            if (!currentPotions.Contains(kvp.Key))
            {
                Destroy(kvp.Value);
                potionsToRemove.Add(kvp.Key);
            }
        }

        foreach (string potionId in potionsToRemove)
        {
            activePotionButtons.Remove(potionId);
        }

        // Show/hide potion panel based on whether there are any potions
        if (potionPanel != null)
        {
            potionPanel.alpha = currentPotions.Count > 0 ? 1 : 0;
            potionPanel.interactable = currentPotions.Count > 0;
            potionPanel.blocksRaycasts = currentPotions.Count > 0;
        }
    }

    private void CreatePotionButton(Potion potion)
    {
        GameObject buttonObj = Instantiate(potionButtonPrefab, potionContainer);
        activePotionButtons[potion.id] = buttonObj;

        // Set up button text
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = $"{potion.name}\n{potion.description}";
        }

        // Set up button click handler
        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => OnPotionButtonClicked(potion.id));
        }
    }

    private void OnPotionButtonClicked(string potionId)
    {
        if (inventoryManager.UsePotion(potionId))
        {
            // Potion was used successfully
            // The UI will update on the next UpdateUI call
        }
    }
} 