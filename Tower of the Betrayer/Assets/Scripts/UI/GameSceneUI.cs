// Authors: Jeff Cui, Elaine Zhao
// Manages the UI elements in the game scene, including health bar, resource display, floor number, potion counts, and speed display.

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Inventory;
using System.Linq;

public class GameSceneUI : MonoBehaviour
{
    [Header("Health Display")]
    public HealthBar healthBar;
    public Color healthBarColor = new Color(0.8f, 0.2f, 0.2f); // Dark red for health

    [Header("Resource Display")]
    public TextMeshProUGUI gemScrapsText;
    public TextMeshProUGUI mushroomText;
    public CanvasGroup resourcePanel;
    
    [Header("Floor Display")]
    public TextMeshProUGUI floorText;
    
    [Header("Potion Display")]
    public TextMeshProUGUI healthPotionCountText;
    public TextMeshProUGUI speedPotionCountText;
    public Image healthPotionImage;
    public Image speedPotionImage;
    public TextMeshProUGUI healthPotionHotkeyText;
    public TextMeshProUGUI speedPotionHotkeyText;
    
    [Header("Speed Display")]
    public TextMeshProUGUI speedText;
    
    [Header("Floor Modifiers Display")]
    public TextMeshProUGUI modifiersText;
    public GameObject modifiersPanel;
    public float modifiersDisplayDuration = 5f;
    public Color goodModifierColor = Color.green;
    public Color badModifierColor = Color.red;
    public KeyCode toggleModifiersKey = KeyCode.Tab;
    
    private InventoryManager inventoryManager;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private List<FloorModifier> activeModifiers = new List<FloorModifier>();

    private void Start()
    {
        // Get required components
        inventoryManager = InventoryManager.Instance;
        playerHealth = FindObjectOfType<PlayerHealth>();
        playerMovement = FindObjectOfType<PlayerMovement>();

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

        // Set up potion hotkey texts
        if (healthPotionHotkeyText != null) healthPotionHotkeyText.text = "[Q]";
        if (speedPotionHotkeyText != null) speedPotionHotkeyText.text = "[E]";

        // Get active floor modifiers
        if (FloorDifficultyManager.Instance != null)
        {
            activeModifiers = FloorDifficultyManager.Instance.GetActiveModifiers();
            
            // Show modifiers panel at start
            if (modifiersPanel != null)
            {
                modifiersPanel.SetActive(true);
                // Auto-hide after a delay
                Invoke(nameof(HideModifiersPanel), modifiersDisplayDuration);
            }
        }

        // Initial UI update
        UpdateResourceDisplay();
        UpdatePotionDisplay();
        UpdateFloorDisplay();
        UpdateSpeedDisplay();
        UpdateModifiersDisplay();

        // Update the UI every 0.5 seconds
        InvokeRepeating(nameof(UpdateUI), 0.5f, 0.5f);
    }

    private void Update()
    {
        // Check for potion hotkeys
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseFirstPotionOfType(PotionType.HealthRestore);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            UseFirstPotionOfType(PotionType.SpeedBoost);
        }
        
        // Toggle modifiers display
        if (Input.GetKeyDown(toggleModifiersKey))
        {
            ToggleModifiersPanel();
        }

        // Update UI
        UpdateUI();
    }

    private void UseFirstPotionOfType(PotionType type)
    {
        var potions = inventoryManager.GetAllPotions();
        var potion = potions.FirstOrDefault(p => !p.isPermanent && p.type == type);
        
        if (potion != null)
        {
            inventoryManager.UsePotion(potion.id);
            UpdatePotionDisplay();
        }
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
        UpdateFloorDisplay();
        UpdateSpeedDisplay();
    }

    private void UpdateFloorDisplay()
    {
        if (floorText != null && GameManager.Instance != null)
        {
            floorText.text = $"Floor {GameManager.Instance.currentFloor}";
        }
    }

    private void UpdateResourceDisplay()
    {
        if (gemScrapsText != null)
        {
            gemScrapsText.text = $"Gem Dust: {inventoryManager.GetResourceAmount(ResourceType.GemDust)}";
        }
        
        if (mushroomText != null)
        {
            mushroomText.text = $"Mushrooms: {inventoryManager.GetResourceAmount(ResourceType.Mushroom)}";
        }
    }

    private void UpdatePotionDisplay()
    {
        var potions = inventoryManager.GetAllPotions();
        
        // Update health potion count
        int healthPotionCount = potions.Count(p => !p.isPermanent && p.type == PotionType.HealthRestore);
        if (healthPotionCountText != null)
        {
            healthPotionCountText.text = healthPotionCount.ToString();
        }
        
        // Update speed potion count
        int speedPotionCount = potions.Count(p => !p.isPermanent && p.type == PotionType.SpeedBoost);
        if (speedPotionCountText != null)
        {
            speedPotionCountText.text = speedPotionCount.ToString();
        }
    }

    private void UpdateSpeedDisplay()
    {
        if (speedText != null && playerMovement != null)
        {
            string speedInfo = $"Speed: {playerMovement.speed:F1}";
            
            // Add active boost count if any boosts are active
            if (playerMovement.speedBoosted)
            {
                speedInfo += $" (+{(playerMovement.speed - playerMovement.baseSpeed):F1})";
                int boostCount = playerMovement.GetActiveBoostCount();
                if (boostCount > 1)
                {
                    speedInfo += $" [{boostCount} active]";
                }
                speedText.color = new Color(0.2f, 0.8f, 0.2f); // Green for boosted speed
            }
            else
            {
                speedText.color = Color.white; // Default color
            }
            
            // Add info about permanent speed if available
            if (PlayerStats.Instance != null)
            {
                speedInfo += $" [Base: {PlayerStats.Instance.Speed:F1}]";
            }
            
            speedText.text = speedInfo;
        }
    }
    
    private void UpdateModifiersDisplay()
    {
        if (modifiersText == null) return;
        
        if (activeModifiers.Count == 0)
        {
            modifiersText.text = "No active modifiers";
        }
        else
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            foreach (FloorModifier modifier in activeModifiers)
            {
                string description = modifier.GetDescription();
                string colorHex = ColorUtility.ToHtmlStringRGB(modifier.isGood ? goodModifierColor : badModifierColor);
                
                sb.AppendLine($"<color=#{colorHex}>{description}</color>");
            }
            
            modifiersText.text = sb.ToString();
        }
    }
    
    public void ShowModifiersPanel()
    {
        if (modifiersPanel != null)
        {
            modifiersPanel.SetActive(true);
        }
    }
    
    public void HideModifiersPanel()
    {
        if (modifiersPanel != null)
        {
            modifiersPanel.SetActive(false);
        }
    }
    
    public void ToggleModifiersPanel()
    {
        if (modifiersPanel != null)
        {
            modifiersPanel.SetActive(!modifiersPanel.activeSelf);
        }
    }
} 